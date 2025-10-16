using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.GUI.Texto2
{
    public class TCPListernerSimulador
    {
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
                    var client = await AcceptClientAsync(listener, token);
                    if (client == null) break; // cancelado

                    // Cada cliente é tratado em sua própria tarefa (não bloqueia o loop)
                    _ = Task.Run(() => HandleClientAsync(client, token), token);
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
        private static bool EstaAberto(TcpClient client)
        {
            // return TCPClient?.Connected ?? false;
            var conectado = client?.Connected ?? false;
            var socket = client?.Client;
            return (client?.Connected ?? false)
                && (client?.Client).Connected;
        }

        private static async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            var endpoint = client.Client.RemoteEndPoint?.ToString() ?? "??";
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Cliente conectado: {endpoint}");

            try
            {
                string line = string.Empty;
                NetworkStream network;
                int tamanho = 0;
                bool liberado = false;
                network = client.GetStream();

                if (!token.IsCancellationRequested && EstaAberto(client))
                {
                    tamanho = client.Available;
                    liberado = network.DataAvailable;
                    if (tamanho == 0)
                    {
                        await Task.Delay(100).ConfigureAwait(false);
                        tamanho = client.Available;
                        liberado = network.DataAvailable;
                    }
                }

                //var reader = new StreamReader(network, Encoding.UTF8, false);
                while (!token.IsCancellationRequested && EstaAberto(client))
                {

                    // Lê uma linha de forma assíncrona (null = cliente fechou)
                    //line = await reader.ReadLineAsync().ConfigureAwait(false);

                    tamanho = client.Available;
                    liberado = network.DataAvailable;
                    if (tamanho == 0)
                    {
                        await Task.Delay(100).ConfigureAwait(false);
                        tamanho = client.Available;
                        liberado = network.DataAvailable;
                    }

                    byte[] buffer = new byte[tamanho];
                    var lido = await network.ReadAsync(buffer, 0, tamanho).ConfigureAwait(false);
                    if (lido == 0)
                    {
                        await Task.Delay(100).ConfigureAwait(false);
                        tamanho = client.Available;
                        liberado = network.DataAvailable;
                        if (tamanho == 0)
                        {
                            if (!liberado)
                            {
                                break; // cliente fechou 
                            }
                        }
                    }

                    line = Encoding.UTF8.GetString(buffer);
                    if (string.IsNullOrEmpty(line)) break;

                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Recebido de {endpoint}: {line}");
                }

                network = client.GetStream();
                var writer = new StreamWriter(network, Encoding.UTF8, 4096, leaveOpen: false) { AutoFlush = true };

                // Loop de leitura – só sai quando o cliente fecha a conexão ou o token é cancelado
                while (!token.IsCancellationRequested && EstaAberto(client))
                {
                    // Responde **apenas** depois de receber algo
                    var teste = writer.BaseStream.CanWrite;
                    string response = $"ECHO: {line}";
                    await writer.WriteLineAsync(response).ConfigureAwait(false);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Enviado a {endpoint}: {response}");
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
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Cliente desconectado: {endpoint}\n");
            }
        }

        public async Task<int> LerComTimeoutAsync(
            NetworkStream network,
            byte[] buffer,
            int offset,
            int tamanho,
            int timeoutMs,  // 0 = sem timeout, >0 = timeout em milissegundos
            CancellationToken cancellationToken = default)
        {
            CancellationTokenSource timeoutCts = default;
            CancellationTokenSource linkedCts = default;

            try
            {
                CancellationToken tokenFinal;

                if (timeoutMs > 0)
                {
                    // Cria CTS para timeout
                    timeoutCts = new CancellationTokenSource(timeoutMs);

                    // Combina timeout + cancelamento externo
                    linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                        timeoutCts.Token,
                        cancellationToken);

                    tokenFinal = linkedCts.Token;
                }
                else
                {
                    // Sem timeout, usa apenas o token externo
                    tokenFinal = cancellationToken;
                }

                // Verifica cancelamento antes de iniciar
                tokenFinal.ThrowIfCancellationRequested();

                var lido = await network.ReadAsync(buffer, offset, tamanho, tokenFinal)
                                        .ConfigureAwait(false);

                return lido;
            }
            catch (OperationCanceledException ex)
            {
                // Verifica qual token causou o cancelamento
                if (timeoutCts?.IsCancellationRequested == true)
                {
                    throw new TimeoutException($"Timeout de {timeoutMs}ms na leitura", ex);
                }
                else
                {
                    // Cancelamento manual pelo usuário
                    throw; // Re-lança OperationCanceledException
                }
            }
            finally
            {
                timeoutCts?.Dispose();
                linkedCts?.Dispose();
            }
        }

    }
}
