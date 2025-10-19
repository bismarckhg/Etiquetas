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
            var timeout = 0;
            var bufferSize = 4096; // Valor padrão
            var cancellationBreak = default(CancellationToken);
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

        private async Task HandleClientAsync(Encoding encoding, CancellationToken token =  default)
        {
            var endpoint = TCPClient.Client.RemoteEndPoint?.ToString() ?? "??";
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Cliente conectado: {endpoint}");

            var contaLido0 = 0;

            try
            {
                string line = string.Empty;
                int tamanho = 0;
                bool liberado = false;
                var netstream = TCPClient.GetStream();
                NetStream = netstream;

                if (!token.IsCancellationRequested && EstaAberto())
                {
                    var bufferZero = new byte[0];
                    var vazio = await LerComTimeoutAsync(
                        bufferZero,
                        tamanho,
                        2000,
                        throwOnTimeout: false,
                        token).ConfigureAwait(false);
                }

                tamanho = TCPClient.Available;
                liberado = PossuiDados();

                //var reader = new StreamReader(network, Encoding.UTF8, false);
                while (!token.IsCancellationRequested && EstaAberto())
                {
                    // Lê uma linha de forma assíncrona (null = cliente fechou)
                    //line = await reader.ReadLineAsync().ConfigureAwait(false);

                    tamanho = TCPClient.Available;
                    liberado = PossuiDados();
                    if (tamanho == 0)
                    {
                        await Task.Delay(100).ConfigureAwait(false);
                        tamanho = TCPClient.Available;
                        liberado = PossuiDados();
                        tamanho = 1;
                    }

                    byte[] buffer = new byte[tamanho];
                    //var lido = await network.ReadAsync(buffer, 0, tamanho).ConfigureAwait(false);
                    var lido = await LerComTimeoutAsync(
                        buffer,
                        tamanho,
                        2000,
                        throwOnTimeout: false,
                        token).ConfigureAwait(false);
                    if (lido == 0)
                    {
                        contaLido0++;
                        await Task.Delay(100).ConfigureAwait(false);
                        if (contaLido0 > 10)
                        {
                            // Opcional: enviar ACK (será ignorado)
                            await NetStream.WriteAsync(new byte[] { 0x06 }, 0, 1);
                            await NetStream.FlushAsync();
                            await FecharAsync().ConfigureAwait(false);
                            break;
                        }
                        continue;
                    }
                    else
                    {
                        contaLido0 = 0;
                    }

                    line = encoding.GetString(buffer);
                    if (string.IsNullOrEmpty(line)) break;

                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Recebido de {endpoint}: {line}");
                }
            }
            catch (OperationCanceledException)
            {
                // Cancelamento solicitado – nada a fazer
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Erro com {endpoint}: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"ContaLido0 = {contaLido0}");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Cliente desconectado: {endpoint}\n");
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

        public override bool PossuiDados()
        {
            var possuiDados = base.PossuiDados();
            possuiDados = possuiDados && NetStream.CanRead;
            possuiDados = possuiDados && NetStream.DataAvailable;
            return true;
        }
    }
}
