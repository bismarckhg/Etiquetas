using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.GUI.Texto2
{
    public class TCPListernerSimulador
    {
        protected static TcpClient TCPClient { get; set; }
        protected static NetworkStream NetStream { get; set; }

        public static async Task Execute()
        {
            // Cancelamento com Ctrl+C
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;               // impede o processo de terminar imediatamente
                cts.Cancel();                  // sinaliza cancelamento
                Console.WriteLine("\nEncerrando servidor…");
            };

            await RunTcpListenerAsync("127.0.0.1", 9100, cts.Token);
        }

        private static async Task RunTcpListenerAsync(string ip, int port, CancellationToken token)
        {
            var listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();

            Console.WriteLine($"Servidor TCP iniciado em {ip}:{port}");
            Console.WriteLine("Aguardando conexões (Ctrl+C para sair)…\n");

            try
            {
                while (!token.IsCancellationRequested)
                {
                    // AcceptTcpClientAsync não aceita token em .NET 4.5, então fazemos o “wrap”
                    var TCPClient = await AcceptClientAsync(listener, token);
                    if (TCPClient == null) break; // cancelado

                    // Cada cliente é tratado em sua própria tarefa (não bloqueia o loop)
                    _ = Task.Run(() => HandleClientAsync(TCPClient, token), token);
                }
            }
            finally
            {
                listener.Stop();
                Console.WriteLine("\nServidor parado.");
            }
        }

        private static async Task<TcpClient> AcceptClientAsync(TcpListener listener, CancellationToken token)
        {
            var acceptTask = listener.AcceptTcpClientAsync();

            // Espera a aceitação ou o cancelamento
            var completed = await Task.WhenAny(acceptTask, Task.Delay(Timeout.Infinite, token));
            if (completed == acceptTask) return await acceptTask;   // cliente aceito
            return null;                                          // cancelado
        }

        /// <inheritdoc/>
        private static bool EstaAberto(TcpClient client, NetworkStream nestream)
        {
            // return TCPClient?.Connected ?? false;
            //var conectado = TCPClient?.Connected ?? false;
            //var socket = TCPClient?.Client;
            //return (TCPClient?.Connected ?? false)
            //    && (TCPClient?.Client).Connected;
            var retorno = (client?.Connected ?? false) && (client?.Client.Connected ?? false);
            return retorno;
        }

        private static async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            var endpoint = client.Client.RemoteEndPoint?.ToString() ?? "??";
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Cliente conectado: {endpoint}");

            var contaZero = 0;
            var contaLido0 = 0;

            try
            {
                string line = string.Empty;
                int tamanho = 0;
                bool liberado = false;
                var netstream = client.GetStream();
                NetStream = netstream;

                if (!token.IsCancellationRequested && EstaAberto(client, netstream))
                {
                    var bufferZero = new byte[0];
                    var vazio = await LerComTimeoutAsync(client, bufferZero, tamanho, 2000, throwOnTimeout: false, token).ConfigureAwait(false);
                }

                tamanho = client.Available;
                liberado = NetStream.DataAvailable;

                //var reader = new StreamReader(network, Encoding.UTF8, false);
                while (!token.IsCancellationRequested && EstaAberto(client, netstream))
                {
                    // Lê uma linha de forma assíncrona (null = cliente fechou)
                    //line = await reader.ReadLineAsync().ConfigureAwait(false);

                    tamanho = client.Available;
                    liberado = NetStream.DataAvailable;
                    if (tamanho == 0)
                    {
                        await Task.Delay(100).ConfigureAwait(false);
                        tamanho = client.Available;
                        liberado = NetStream.DataAvailable;
                        tamanho = 1;
                        contaZero++;
                    }

                    byte[] buffer = new byte[tamanho];
                    //var lido = await network.ReadAsync(buffer, 0, tamanho).ConfigureAwait(false);
                    var lido = await LerComTimeoutAsync(client, buffer, tamanho, 2000, throwOnTimeout: false, token).ConfigureAwait(false);
                    if (lido == 0)
                    {
                        contaLido0++;
                        await Task.Delay(100).ConfigureAwait(false);
                        if (contaLido0 > 10)
                        {
                            // Opcional: enviar ACK (será ignorado)
                            await NetStream.WriteAsync(new byte[] { 0x06 }, 0, 1);
                            await NetStream.FlushAsync();
                            client.Close();
                            break;
                        }
                        continue;
                    }

                    //if (lido < 0)
                    //{
                    //    await Task.Delay(100).ConfigureAwait(false);
                    //    tamanho = client.Available;
                    //    liberado = NetStream.DataAvailable;
                    //    if (tamanho == 0)
                    //    {
                    //        if (!liberado)
                    //        {
                    //            break; // cliente fechou 
                    //        }
                    //    }
                    //}

                    line = Encoding.UTF8.GetString(buffer);
                    if (string.IsNullOrEmpty(line)) break;

                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Recebido de {endpoint}: {line}");
                }

                ////NetStream = TCPClient.GetStream();
                ////var writer = new StreamWriter(NetStream, Encoding.UTF8, 4096, leaveOpen: false) { AutoFlush = true };

                ////// Loop de leitura – só sai quando o cliente fecha a conexão ou o token é cancelado
                ////while (!token.IsCancellationRequested && EstaAberto(client))
                ////{
                ////    // Responde **apenas** depois de receber algo
                ////    var teste = writer.BaseStream.CanWrite;
                ////    string response = $"ECHO: {line}";
                ////    await writer.WriteLineAsync(response).ConfigureAwait(false);
                ////    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Enviado a {endpoint}: {response}");
                ////}
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
                Console.WriteLine($"ContaZero = {contaZero} ContaLido0 = {contaLido0}");
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Cliente desconectado: {endpoint}\n");
            }
        }

        // Helper para ReadLine com timeout individual e cancellation
        private static async Task<int> ReadLineWithTimeoutAsync(
            byte[] buffer,
            int tamanho,
            int timeoutMilliseconds,  // 0 = sem timeout, >0 = timeout em milissegundos
            CancellationToken cancellationBreak = default)
        {
            // Se timeout = 0, Timeout.Infinite ou negativo, aguarda indefinidamente
            if (timeoutMilliseconds <= 0)
            {
                var readTask = Task.Run(async () =>
                {
                    try
                    {
                        return await NetStream.ReadAsync(buffer, 0, tamanho, cancellationBreak).ConfigureAwait(false);
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
                    var readTask = Task.Run(async () =>
                    {
                        try
                        {
                            return await NetStream.ReadAsync(buffer, 0, tamanho, cancellationBreak).ConfigureAwait(false);
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

        public static async Task<int> LerComTimeoutAsync(
            TcpClient client,
                byte[] buffer,
                int tamanho,
                int timeoutMs,
                bool throwOnTimeout = false,
                CancellationToken cancellationToken = default)
        {
            // Verifica cancelamento antes de começar
            cancellationToken.ThrowIfCancellationRequested();

            int streamTimeoutOriginal = NetStream.ReadTimeout > 0 ? client.Client.ReceiveTimeout : Timeout.Infinite;

            if (timeoutMs > 0)
            {
                // Define timeout no socket
                NetStream.ReadTimeout = timeoutMs;
            }
            var x = 1;

            try
            {
                // Executa Read síncrono em thread separada
                return await Task.Run(() =>
                {
                    var dadosLidos = 0;
                    try
                    {
                        var available = client.Available;
                        var connected = client.Connected;
                        var dataavailable = NetStream.DataAvailable;

                        dadosLidos = NetStream.Read(buffer, 0, tamanho);
                        var socket = client.Client;
                        var netstream = NetStream;
                        var teste4 = netstream.CanRead;
                        var teste5 = socket.Connected;
                        var teste6 = socket.Poll(100000, SelectMode.SelectRead);
                        var teste7 = socket.Poll(1, SelectMode.SelectRead);

                        return dadosLidos;
                    }
                    catch (IOException ex) when (ex.InnerException is SocketException se &&
                                                  se.SocketErrorCode == SocketError.TimedOut)
                    {
                        if (!throwOnTimeout)
                        {
                            if (dadosLidos > 0)
                            {
                                return dadosLidos; // Retorna bytes lidos antes do timeout
                            }
                            return -1; // Saida por Timeout sem exceção
                        }

                        throw new TimeoutException($"Timeout de {timeoutMs}ms na leitura", ex);
                    }
                }, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                NetStream.ReadTimeout = streamTimeoutOriginal;
            }
        }
    }
}
