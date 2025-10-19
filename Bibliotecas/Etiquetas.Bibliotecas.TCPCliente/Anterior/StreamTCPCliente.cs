using Etiquetas.Bibliotecas.Comum.Geral;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    public class StreamTCPCliente : StreamBaseTCPCliente, IStreamLeitura, IStreamEscrita
    {
        protected NetworkStream NetStream;
        protected StreamReader Reader;
        protected StreamWriter Writer;

        public Task<T> LerAsync<T>(params object[] parametros)
        {
            var posicao = 0;
            var posicao1 = 0;
            var timeout = 0;
            var bufferSize = 4096; // Valor padrão
            var cancellationBreak = default(CancellationToken);
            var cancellationStop = default(CancellationToken);
            var encoding = ConversaoEncoding.UTF8BOM;

            foreach (var item in parametros)
            {
                switch (item)
                {
                    case int valor:
                        switch (posicao)
                        {
                            case 0:
                                posicao++;
                                timeout = valor;
                                break;
                            case 1:
                                posicao++;
                                bufferSize = valor;
                                break;
                            default:
                                break;
                        }
                        break;
                    case Encoding encode:
                        encoding = encode;
                        break;
                    case CancellationToken cancellation:
                        cancellationBreak = cancellation;
                        break;
                    default:
                        break;
                }
            }

            if (typeof(T) == typeof(string))
            {
                return (Task<T>)(object)ReadLinesManuallyAsync(
                    bufferSize,
                    timeout,
                    encoding,
                    cancellationBreak);
            }

            throw new InvalidCastException($"Tipo de retorno não suportado: {typeof(T).FullName}");

        }

        public Task<T> LerAsync<T>(ITaskParametros parametros)
        {
            var timeout = parametros.RetornaSeExistir<int>("Timeout");
            var bufferSize = parametros.RetornaSeExistir<int>("BufferSize");
            var encoding = parametros.RetornaSeExistir<Encoding>("Encoding") ?? ConversaoEncoding.UTF8BOM;
            var cancellationBreak = parametros.RetornaSeExistir<CancellationToken>("CancellationBreak");

            if (typeof(T) == typeof(string))
            {
                return (Task<T>)(object)ReadLinesManuallyAsync(
                    bufferSize,
                    timeout,
                    encoding,
                    cancellationBreak);
            }

            throw new InvalidCastException($"Tipo de retorno não suportado: {typeof(T).FullName}");
        }

        ///// <summary>
        ///// Envia dados para o servidor TCP de forma assíncrona
        ///// </summary>
        ///// <param name="TCPClient">Cliente TCP conectado</param>
        ///// <param name="message">Mensagem a ser enviada</param>
        ///// <param name="cancellationBreak">Token de cancelamento</param>
        //private async Task SendDataAsync(TcpClient TCPClient, string message, Encoding encoding, CancellationToken cancellationBreak = default)
        //{
        //    try
        //    {
        //        cancellationBreak.ThrowIfCancellationRequested();
        //        using (var NetStream = TCPClient.GetStream())
        //        {
        //            // Converte a mensagem para bytes
        //            var data = encoding.GetBytes(message);

        //            // .NET 4.5 não tem WriteAsync nativo para NetworkStream, usamos Task.Run
        //            await Task.Run(() =>
        //            {
        //                NetStream.Write(data, 0, data.Length);
        //                NetStream.Flush();
        //            }, cancellationBreak).ConfigureAwait(false);
        //        }
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Lê dados de uma conexão TCP de forma assíncrona
        /// </summary>
        /// <param name="bufferSize">Tamanho do buffer para leitura de dados</param>
        /// <param name="lineTimeoutMilliseconds">Timeout de leitura</param>
        /// <param name="encoding">Encoding do texto lido</param>
        /// <param name="cancellationBreak">Token de cancelamento</param>
        /// <returns>Dados lidos da conexão</returns>
        private async Task<T> ReadLinesFromConnectionAsync<T>(
            int lineTimeoutMilliseconds,
            Encoding encoding,
            CancellationToken cancellationBreak = default)
        {
            // var lines = new List<string>();
            var lines = new StringBuilder();
            encoding = encoding ?? ConversaoEncoding.UTF8BOM;

            try
            {
                cancellationBreak.ThrowIfCancellationRequested();

                // Remove o timeout do stream para controlar manualmente
                NetStream.ReadTimeout = Timeout.Infinite;

                this.Reader = new StreamReader(NetStream, encoding, detectEncodingFromByteOrderMarks: false, bufferSize: 8192, leaveOpen: false);

                try
                {
                    cancellationBreak.ThrowIfCancellationRequested();

                    // ReadLine com timeout individual por linha
                    string line = await ReadLineWithTimeoutAsync(lineTimeoutMilliseconds, cancellationBreak)
                        .ConfigureAwait(false);

                    //if (line == null) // Fim do stream
                    //    break;

                    lines.Append(line);

                    // Se quiser processar linha por linha em tempo real:
                    // OnLineReceived(line);
                }
                catch (TimeoutException ex)
                {
                    throw;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)(lines.ToString());
                }

                throw new InvalidCastException($"Tipo de retorno não suportado: {typeof(T).FullName}");
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        // Helper para ReadLine com timeout individual e cancellation
        private async Task<string> ReadLineWithTimeoutAsync(
            int timeoutMilliseconds,
            CancellationToken cancellationBreak)
        {
            // Se timeout = 0, Timeout.Infinite ou negativo, aguarda indefinidamente
            if (timeoutMilliseconds <= 0)
            {
                var readTask = Task.Run(() =>
                {
                    try
                    {
                        return Reader.ReadLine();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }, cancellationBreak);

                return await readTask.ConfigureAwait(false);
            }

            using (var timeoutCts = new CancellationTokenSource(timeoutMilliseconds))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationBreak, timeoutCts.Token))
            {
                try
                {
                    var readTask = Task.Run(() =>
                    {
                        try
                        {
                            return Reader.ReadLine();
                        }
                        catch (TimeoutException)
                        {
                            throw;
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                    }, linkedCts.Token);

                    return await readTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested && !cancellationBreak.IsCancellationRequested)
                {
                    // Timeout específico - não é cancelamento do usuário
                    throw new TimeoutException($"Timeout de {timeoutMilliseconds}ms excedido ao ler linha");
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                // Se for cancellationBreak, deixa a exceção subir para ser tratada no loop externo
            }
        }

        /// <summary>
        /// Lê dados de uma conexão TCP de forma assíncrona
        /// </summary>
        /// <param name="bufferSize">Tamanho do buffer para leitura de dados</param>
        /// <param name="lineTimeoutMilliseconds">Timeout de leitura</param>
        /// <param name="encoding">Encoding do texto lido</param>
        /// <param name="cancellationBreak">Token de cancelamento</param>
        /// <param name="cancellationStop">Token de cancelamento</param>
        /// <returns>Dados lidos da conexão</returns>
        private async Task<string> ReadLinesManuallyAsync(
                int bufferSize,
                int lineTimeoutMilliseconds,
                Encoding encoding,
                CancellationToken cancellationBreak = default)
        {
            //var lines = new List<string>();
            var lines = new StringBuilder();

            try
            {
                NetStream = TCPClient.GetStream();

                // Remove timeout do stream para controlar manualmente
                NetStream.ReadTimeout = Timeout.Infinite;
                
                var buffer = new byte[bufferSize];
                var incompleteLineBuffer = new List<byte>(); // Buffer persistente para linha incompleta
                var lastReadTime = DateTime.UtcNow;

                try
                {
                    cancellationBreak.ThrowIfCancellationRequested();

                    // Lê dados com timeout individual
                    var bytesRead = await ReadWithTimeoutAsync(
                        buffer,
                        lineTimeoutMilliseconds,
                        cancellationBreak
                    ).ConfigureAwait(false);

                    // if (bytesRead == 0)
                    //    break; // Conexão fechada

                    lastReadTime = DateTime.UtcNow;

                    // Processa os bytes lidos
                    for (int i = 0; i < bytesRead; i++)
                    {
                        byte b = buffer[i];

                        if (b == '\n') // Fim de linha (LF)
                        {
                            // Remove \r se existir no final (Não remover)
                            // if (incompleteLineBuffer.Count > 0 && incompleteLineBuffer[incompleteLineBuffer.Count - 1] == '\r')
                            //    incompleteLineBuffer.RemoveAt(incompleteLineBuffer.Count - 1);

                            // Converte para string e adiciona à lista
                            string line = encoding.GetString(incompleteLineBuffer.ToArray());
                            lines.Append(line);

                            // Reseta o timer de timeout para a próxima linha
                            lastReadTime = DateTime.UtcNow;

                            // Limpa o buffer para a próxima linha
                            incompleteLineBuffer.Clear();
                        }
                        else
                        {
                            incompleteLineBuffer.Add(b);
                        }
                    }

                    // Verifica timeout de linha incompleta
                    if (incompleteLineBuffer.Count > 0)
                    {
                        var elapsed = DateTime.UtcNow - lastReadTime;
                        if (elapsed.TotalMilliseconds > lineTimeoutMilliseconds)
                        {
                            throw new TimeoutException($"Timeout de {lineTimeoutMilliseconds}ms aguardando fim de linha");
                        }
                    }
                }
                catch (TimeoutException ex)
                {
                    throw;
                    //// Timeout na leitura individual - loga e continua
                    //OnErrorOccurred(new Exception($"Timeout de {lineTimeoutMilliseconds}ms ao ler dados", ex));

                    //// Limpa buffer incompleto se necessário
                    //if (incompleteLineBuffer.Count > 0)
                    //{
                    //    // Opcional: salva linha incompleta
                    //    string incompleteLine = encoding.GetString(incompleteLineBuffer.ToArray());
                    //    throw new Exception($"Linha incompleta descartada após timeout: {incompleteLine}");
                    //    incompleteLineBuffer.Clear();
                    //}

                    //// Continua verificando cancellation
                    //continue;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }

                // Se sobrou dados no buffer (linha sem \n no final)
                if (incompleteLineBuffer.Count > 0)
                {
                    string lastLine = encoding.GetString(incompleteLineBuffer.ToArray());
                    lines.Append(lastLine);
                }

                return lines.ToString();

            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        // Helper para Read com timeout individual
        private async Task<int> ReadWithTimeoutAsync(
            byte[] buffer,
            int timeoutMilliseconds,
            CancellationToken cancellationBreak)
        {
            // Se timeout = 0, Timeout.Infinite ou negativo, aguarda indefinidamente
            if (timeoutMilliseconds <= 0)
            {
                var readTask = Task.Run(() =>
                {
                    try
                    {
                        return NetStream.Read(buffer, 0, buffer.Length);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }, cancellationBreak);

                return await readTask.ConfigureAwait(false);
            }

            // Com timeout válido (> 0 e diferente de Timeout.Infinite)
            using (var timeoutCts = new CancellationTokenSource(timeoutMilliseconds))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationBreak, timeoutCts.Token))
            {
                try
                {
                    var readTask = Task.Run(() =>
                    {
                        try
                        {
                            return NetStream.Read(buffer, 0, buffer.Length);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }, linkedCts.Token);

                    return await readTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
                {
                    // Timeout específico - não é cancelamento do usuário
                    throw new TimeoutException($"Timeout de {timeoutMilliseconds}ms excedido ao ler dados");
                    //return 0; // Conexão fechada
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                // Se for cancellationBreak, deixa a exceção subir
            }
        }

        public Task EscreverAsync<T>(params object[] parametros)
        {
            throw new NotImplementedException();
        }

        public Task EscreverAsync<T>(ITaskParametros parametros)
        {
            var dados = parametros.RetornaSeExistir<string>("Dados");
            var timeout = parametros.RetornaSeExistir<int>("Timeout");
            var bufferSize = parametros.RetornaSeExistir<int>("BufferSize");
            var encoding = parametros.RetornaSeExistir<Encoding>("Encoding") ?? ConversaoEncoding.UTF8BOM;
            var cancellationBreak = parametros.RetornaSeExistir<CancellationToken>("CancellationBreak");
            var addLineBreak = parametros.RetornaSeExistir<bool>("AddLineBreak");

            return SendDataAsync(
                dados,
                bufferSize,
                timeout,
                encoding,
                addLineBreak,
                cancellationBreak);
        }

        /// <summary>
        /// Envia uma string para o TcpClient com timeout
        /// </summary>
        private async Task SendDataAsync(
            string data,
             int bufferSize,
            int timeoutMilliseconds = 0,
            Encoding encoding = null,
            bool addLineBreak = true,
            CancellationToken cancellationBreak = default)
        {
            encoding = encoding ?? ConversaoEncoding.UTF8BOM;

            try
            {
                // Verifica se a conexão está ativa
                if (!EstaAberto())
                {
                    //OnErrorOccurred(new Exception("Conexão TCP não está ativa para envio"));
                    throw new Exception("Conexão TCP não está ativa para envio");
                }

                NetStream = TCPClient.GetStream();

                NetStream.WriteTimeout = timeoutMilliseconds > 0 ? timeoutMilliseconds : Timeout.Infinite;

                // Adiciona quebra de linha se solicitado
                string dataToSend = addLineBreak ? data + "\r\n" : data;
                byte[] buffer = encoding.GetBytes(dataToSend);

                // Envia com timeout
                if (timeoutMilliseconds > 0 && timeoutMilliseconds != Timeout.Infinite)
                {
                    using (var timeoutCts = new CancellationTokenSource(timeoutMilliseconds))
                    using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationBreak, timeoutCts.Token))
                    {
                        await WriteWithTimeoutAsync(buffer, linkedCts.Token).ConfigureAwait(false);
                    }
                }
                else
                {
                    await WriteWithTimeoutAsync(buffer, cancellationBreak).ConfigureAwait(false);
                }

                // Garante que os dados foram enviados
                await FlushStreamAsync(cancellationBreak).ConfigureAwait(false);
            }
            catch (TimeoutException ex)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Helper para escrever no stream com cancellation
        /// </summary>
        private async Task WriteWithTimeoutAsync(
            byte[] buffer,
            CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    NetStream.Write(buffer, 0, buffer.Length);
                }
                catch (Exception)
                {
                    throw;
                }
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Flush do stream de forma assíncrona
        /// </summary>
        private async Task FlushStreamAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    NetStream.Flush();
                }
                catch (Exception)
                {
                    throw;
                }
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Envia dados e aguarda resposta (uma linha)
        /// </summary>
        private async Task<string> SendAndReceiveLineAsync(
            string data,
            int sendTimeoutMilliseconds = 5000,
            int receiveTimeoutMilliseconds = 5000,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            encoding = encoding ?? ConversaoEncoding.UTF8BOM;

            try
            {
                // Envia dados
                await SendDataAsync(
                    data,
                    bufferSize: 4096,
                    sendTimeoutMilliseconds,
                    encoding,
                    addLineBreak: true,
                    cancellationToken
                ).ConfigureAwait(false);

                // Aguarda resposta
                NetStream.ReadTimeout = Timeout.Infinite;

                this.Reader = new StreamReader(NetStream, encoding, detectEncodingFromByteOrderMarks: false, bufferSize: 8192, leaveOpen: true);
                string response = await ReadLineWithTimeoutAsync(
                    receiveTimeoutMilliseconds,
                    cancellationToken
                ).ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new Exception("Erro ao enviar e receber dados", ex));
                return null;
            }
        }

        public override bool PossuiDados()
        {
            var teste = base.PossuiDados();
            var teste1 = TCPClient.Client.Poll(1000000, SelectMode.SelectRead);
            var teste2 = TCPClient.Connected;
            var teste3 = TCPClient.Available;
            return true;
        }
    }
}
