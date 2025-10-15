using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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

        private static async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            var endpoint = client.Client.RemoteEndPoint?.ToString() ?? "??";
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Cliente conectado: {endpoint}");

            try
            {
                using (client)
                using (var network = client.GetStream())
                using (var reader = new StreamReader(network, Encoding.UTF8, false))
                using (var writer = new StreamWriter(network, Encoding.UTF8, 4096, leaveOpen: false) { AutoFlush = true })
                {
                    // Loop de leitura – só sai quando o cliente fecha a conexão ou o token é cancelado
                    while (!token.IsCancellationRequested && client.Connected)
                    {
                        // Lê uma linha de forma assíncrona (null = cliente fechou)
                        var line = await reader.ReadLineAsync().ConfigureAwait(false);
                        if (line == null) break;

                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Recebido de {endpoint}: {line}");

                        // Responde **apenas** depois de receber algo
                        string response = $"ECHO: {line}";
                        await writer.WriteLineAsync(response).ConfigureAwait(false);
                        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Enviado a {endpoint}: {response}");
                    }
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
    }
}
