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
                        switch (posicao)
                        {
                            case 0:
                                posicao1++;
                                cancellationBreak = cancellation;
                                break;
                            case 1:
                                posicao1++;
                                cancellationStop = cancellation;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            return (Task<T>)(object)ReadLinesManuallyAsync(
                TCPClient,
                bufferSize,
                timeout,
                encoding,
                cancellationBreak,
                cancellationStop);
        }

        public Task<T> LerAsync<T>(ITaskParametros parametros)
        {
            var timeout = parametros.RetornaSeExistir<int>("Timeout");
            var bufferSize = parametros.RetornaSeExistir<int>("BufferSize");
            var encoding = parametros.RetornaSeExistir<Encoding>("Encoding") ?? ConversaoEncoding.UTF8BOM;
            var cancellationBreak = parametros.RetornaSeExistir<CancellationToken>("CancellationBreak");
            var cancellationStop = parametros.RetornaSeExistir<CancellationToken>("CancellationStop");

            return (Task<T>)(object)ReadLinesManuallyAsync(
                TCPClient,
                bufferSize,
                timeout,
                encoding,
                cancellationBreak,
                cancellationStop);
        }

        /// <summary>
        /// Envia dados para o servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="tcpClient">Cliente TCP conectado</param>
        /// <param name="message">Mensagem a ser enviada</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        private async Task SendDataAsync(TcpClient tcpClient, string message, Encoding encoding, CancellationToken cancellationToken)
        {
            try
            {
                using (var networkStream = tcpClient.GetStream())
                {
                    // Converte a mensagem para bytes
                    var data = encoding.GetBytes(message);

                    // .NET 4.5 não tem WriteAsync nativo para NetworkStream, usamos Task.Run
                    await Task.Run(() =>
                    {
                        networkStream.Write(data, 0, data.Length);
                        networkStream.Flush();
                    }, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao enviar dados: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lê dados de uma conexão TCP de forma assíncrona
        /// </summary>
        /// <param name="tcpClient">Cliente TCP</param>
        /// <param name="bufferSize">Tamanho do buffer para leitura de dados</param>
        /// <param name="lineTimeoutMilliseconds">Timeout de leitura</param>
        /// <param name="encoding">Encoding do texto lido</param>
        /// <param name="cancellationBreak">Token de cancelamento</param>
        /// <param name="cancellationStop">Token de cancelamento</param>
        /// <returns>Dados lidos da conexão</returns>
        private async Task<string> ReadLinesFromConnectionAsync(
            TcpClient tcpClient,
            int lineTimeoutMilliseconds,
            Encoding encoding,
            CancellationToken cancellationBreak = default,
            CancellationToken cancellationStop = default)
        {
            // var lines = new List<string>();
            var lines = new StringBuilder();
            encoding = encoding ?? ConversaoEncoding.UTF8BOM;

            try
            {
                using (var networkStream = tcpClient.GetStream())
                {
                    // Remove o timeout do stream para controlar manualmente
                    networkStream.ReadTimeout = Timeout.Infinite;

                    using (var reader = new StreamReader(networkStream, encoding, detectEncodingFromByteOrderMarks: false, bufferSize: 8192, leaveOpen: false))
                    {
                        while (!cancellationStop.IsCancellationRequested)
                        {
                            try
                            {
                                cancellationBreak.ThrowIfCancellationRequested();

                                // Verifica se a conexão ainda está viva
                                if (!PossuiDados())
                                {
                                    break; // Sai do loop
                                }

                                // ReadLine com timeout individual por linha
                                string line = await ReadLineWithTimeoutAsync(reader, lineTimeoutMilliseconds, cancellationBreak)
                                    .ConfigureAwait(false);

                                //if (line == null) // Fim do stream
                                //    break;

                                lines.Append(line);

                                // Se quiser processar linha por linha em tempo real:
                                // OnLineReceived(line);
                            }
                            catch (TimeoutException ex)
                            {
                                // Timeout na linha individual - loga e continua no loop
                                //OnErrorOccurred(new Exception($"Timeout de {lineTimeoutMilliseconds}ms ao ler linha individual", ex));

                                // Continua verificando cancellation e tentando ler próxima linha
                                continue;
                            }
                            catch (IOException ex)
                            {
                                // Conexão pode ter sido fechada
                                throw new Exception("Erro de IO ao ler linha", ex);
                            }
                        }
                    }
                }

                return lines.ToString();
            }
            catch (OperationCanceledException)
            {
                return lines.ToString(); // Retorna o que foi lido até agora
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex);
                return null;
            }
        }

        // Helper para ReadLine com timeout individual e cancellation
        private async Task<string> ReadLineWithTimeoutAsync(
            StreamReader reader,
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
                        return reader.ReadLine();
                    }
                    catch (IOException)
                    {
                        return null;
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
                            return reader.ReadLine();
                        }
                        catch (IOException)
                        {
                            return null; // Stream fechado
                        }
                    }, linkedCts.Token);

                    return await readTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested && !cancellationBreak.IsCancellationRequested)
                {
                    // Timeout específico - não é cancelamento do usuário
                    throw new TimeoutException($"Timeout de {timeoutMilliseconds}ms excedido ao ler linha");
                }
                // Se for cancellationToken, deixa a exceção subir para ser tratada no loop externo
            }
        }

        /// <summary>
        /// Lê dados de uma conexão TCP de forma assíncrona
        /// </summary>
        /// <param name="tcpClient">Cliente TCP</param>
        /// <param name="bufferSize">Tamanho do buffer para leitura de dados</param>
        /// <param name="lineTimeoutMilliseconds">Timeout de leitura</param>
        /// <param name="encoding">Encoding do texto lido</param>
        /// <param name="cancellationBreak">Token de cancelamento</param>
        /// <param name="cancellationStop">Token de cancelamento</param>
        /// <returns>Dados lidos da conexão</returns>
        private async Task<string> ReadLinesManuallyAsync(
                TcpClient tcpClient,
                int bufferSize,
                int lineTimeoutMilliseconds,
                Encoding encoding,
                CancellationToken cancellationBreak = default,
                CancellationToken cancellationStop = default)
        {
            //var lines = new List<string>();
            var lines = new StringBuilder();

            try
            {
                using (var networkStream = tcpClient.GetStream())
                {
                    // Remove timeout do stream para controlar manualmente
                    networkStream.ReadTimeout = Timeout.Infinite;

                    var buffer = new byte[bufferSize];
                    var incompleteLineBuffer = new List<byte>(); // Buffer persistente para linha incompleta
                    var lastReadTime = DateTime.UtcNow;

                    while (!cancellationStop.IsCancellationRequested)
                    {
                        try
                        {
                            cancellationBreak.ThrowIfCancellationRequested();

                            // Lê dados com timeout individual
                            var bytesRead = await ReadWithTimeoutAsync(
                                networkStream,
                                buffer,
                                lineTimeoutMilliseconds,
                                cancellationBreak
                            ).ConfigureAwait(false);

                            if (bytesRead == 0)
                                break; // Conexão fechada

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
                            // Timeout na leitura individual - loga e continua
                            OnErrorOccurred(new Exception($"Timeout de {lineTimeoutMilliseconds}ms ao ler dados", ex));

                            // Limpa buffer incompleto se necessário
                            if (incompleteLineBuffer.Count > 0)
                            {
                                // Opcional: salva linha incompleta
                                string incompleteLine = encoding.GetString(incompleteLineBuffer.ToArray());
                                throw new Exception($"Linha incompleta descartada após timeout: {incompleteLine}");
                                incompleteLineBuffer.Clear();
                            }

                            // Continua verificando cancellation
                            continue;
                        }
                        catch (IOException ex)
                        {
                            throw new Exception("Erro de IO ao ler dados", ex);
                            break;
                        }
                    }

                    // Se sobrou dados no buffer (linha sem \n no final)
                    if (incompleteLineBuffer.Count > 0)
                    {
                        string lastLine = encoding.GetString(incompleteLineBuffer.ToArray());
                        lines.Append(lastLine);
                    }

                    return lines.ToString();
                }
            }
            catch (OperationCanceledException)
            {
                return lines.ToString(); // Retorna o que foi lido até agora
            }
        }

        // Helper para Read com timeout individual
        private async Task<int> ReadWithTimeoutAsync(
            NetworkStream networkStream,
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
                        return networkStream.Read(buffer, 0, buffer.Length);
                    }
                    catch (IOException)
                    {
                        return 0;
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
                            return networkStream.Read(buffer, 0, buffer.Length);
                        }
                        catch (IOException)
                        {
                            return 0; // Conexão fechada
                        }
                    }, linkedCts.Token);

                    return await readTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
                {
                    // Timeout específico - não é cancelamento do usuário
                    // throw new TimeoutException($"Timeout de {timeoutMilliseconds}ms excedido ao ler dados");
                    return 0; // Conexão fechada
                }
                // Se for cancellationToken, deixa a exceção subir
            }
        }

        public Task EscreverAsync<T>(params object[] parametros)
        {
            throw new NotImplementedException();
        }

        public Task EscreverAsync<T>(ITaskParametros parametros)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Envia uma string para o TcpClient com timeout
        /// </summary>
        private async Task<bool> SendDataAsync(
            TcpClient tcpClient,
            string data,
            int timeoutMilliseconds = 5000,
            Encoding encoding = null,
            bool addLineBreak = true,
            CancellationToken cancellationToken = default)
        {
            encoding = encoding ?? Encoding.UTF8;

            try
            {
                // Verifica se a conexão está ativa
                if (!IsConnectionAlive(tcpClient))
                {
                    OnErrorOccurred(new Exception("Conexão TCP não está ativa para envio"));
                    return false;
                }

                using (var networkStream = tcpClient.GetStream())
                {
                    networkStream.WriteTimeout = timeoutMilliseconds > 0 ? timeoutMilliseconds : Timeout.Infinite;

                    // Adiciona quebra de linha se solicitado
                    string dataToSend = addLineBreak ? data + "\r\n" : data;
                    byte[] buffer = encoding.GetBytes(dataToSend);

                    // Envia com timeout
                    if (timeoutMilliseconds > 0 && timeoutMilliseconds != Timeout.Infinite)
                    {
                        using (var timeoutCts = new CancellationTokenSource(timeoutMilliseconds))
                        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token))
                        {
                            await WriteWithTimeoutAsync(networkStream, buffer, linkedCts.Token).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        await WriteWithTimeoutAsync(networkStream, buffer, cancellationToken).ConfigureAwait(false);
                    }

                    // Garante que os dados foram enviados
                    await FlushStreamAsync(networkStream, cancellationToken).ConfigureAwait(false);

                    return true;
                }
            }
            catch (TimeoutException ex)
            {
                OnErrorOccurred(new Exception($"Timeout de {timeoutMilliseconds}ms ao enviar dados", ex));
                return false;
            }
            catch (IOException ex)
            {
                OnErrorOccurred(new Exception("Erro de IO ao enviar dados - conexão perdida", ex));
                return false;
            }
            catch (SocketException ex)
            {
                OnErrorOccurred(new Exception("Erro de socket ao enviar dados", ex));
                return false;
            }
            catch (ObjectDisposedException ex)
            {
                OnErrorOccurred(new Exception("Stream foi fechado durante envio", ex));
                return false;
            }
            catch (OperationCanceledException)
            {
                OnErrorOccurred(new Exception("Envio cancelado"));
                return false;
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex);
                return false;
            }
        }

        /// <summary>
        /// Helper para escrever no stream com cancellation
        /// </summary>
        private async Task WriteWithTimeoutAsync(
            NetworkStream networkStream,
            byte[] buffer,
            CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    networkStream.Write(buffer, 0, buffer.Length);
                }
                catch (IOException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Flush do stream de forma assíncrona
        /// </summary>
        private async Task FlushStreamAsync(NetworkStream networkStream, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                try
                {
                    networkStream.Flush();
                }
                catch (IOException)
                {
                    throw;
                }
                catch (ObjectDisposedException)
                {
                    throw;
                }
            }, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Envia dados e aguarda resposta (uma linha)
        /// </summary>
        private async Task<string> SendAndReceiveLineAsync(
            TcpClient tcpClient,
            string data,
            int sendTimeoutMilliseconds = 5000,
            int receiveTimeoutMilliseconds = 5000,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            encoding = encoding ?? Encoding.UTF8;

            try
            {
                // Envia dados
                bool sent = await SendDataAsync(
                    tcpClient,
                    data,
                    sendTimeoutMilliseconds,
                    encoding,
                    addLineBreak: true,
                    cancellationToken
                ).ConfigureAwait(false);

                if (!sent)
                {
                    OnErrorOccurred(new Exception("Falha ao enviar dados"));
                    return null;
                }

                // Aguarda resposta
                using (var networkStream = tcpClient.GetStream())
                {
                    networkStream.ReadTimeout = Timeout.Infinite;

                    using (var reader = new StreamReader(networkStream, encoding, detectEncodingFromByteOrderMarks: false, bufferSize: 8192, leaveOpen: true))
                    {
                        string response = await ReadLineWithTimeoutAsync(
                            reader,
                            receiveTimeoutMilliseconds,
                            cancellationToken
                        ).ConfigureAwait(false);

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred(new Exception("Erro ao enviar e receber dados", ex));
                return null;
            }
        }
    }
}
