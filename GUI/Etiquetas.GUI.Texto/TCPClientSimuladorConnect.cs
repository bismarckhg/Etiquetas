using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.GUI.Texto
{
    public static class TCPClientSimuladorConnect
    {
        public static async Task ExecutaTodosExemplos()
        {
            await Exemplo1ConnectWithTimeoutAsync().ConfigureAwait(false);
            await Exemplo2ConnectWithTimeoutAsync().ConfigureAwait(false);
            await Exemplo3ConnectWithTimeoutAsync().ConfigureAwait(false);
            await Exemplo4ConnectWithTimeoutAsync().ConfigureAwait(false);
        }

        public static async Task Exemplo1ConnectWithTimeoutAsync()
        {
            // Uso:
            var client = new TcpClient();
            try
            {
                Console.WriteLine("=== Exemplo1ConnectWithTimeoutAsync ===");
                await ConnectWithTimeoutAsync1(client, "192.168.1.100", 9100, 2000);
                Console.WriteLine("✅ ConnectWithTimeoutAsync1 - Conectado");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"⏱️ TIMEOUT: {ex.Message}");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused)
            {
                // ❌ Porta fechada/recusada
                Console.WriteLine("❌ Conexão recusada");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
            {
                // ❌ Host não encontrado (DNS falhou)
                Console.WriteLine("❌ Host não encontrado");
            }
            catch (SocketException ex)
            {
                // ❌ Outros erros de socket
                Console.WriteLine($"❌ Erro Code: {ex.SocketErrorCode} Message {ex.Message}");
            }
        }

        public static async Task Exemplo2ConnectWithTimeoutAsync()
        {
            // Uso:
            var client = new TcpClient();
            try
            {
                Console.WriteLine("=== Exemplo2ConnectWithTimeoutAsync ===");
                await ConnectWithTimeoutAsync2(client, "192.168.1.100", 9100, 2000);
                Console.WriteLine("✅ ConnectWithTimeoutAsync2 - Conectado");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"⏱️ TIMEOUT: {ex.Message}");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused)
            {
                // ❌ Porta fechada/recusada
                Console.WriteLine("❌ Conexão recusada");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
            {
                // ❌ Host não encontrado (DNS falhou)
                Console.WriteLine("❌ Host não encontrado");
            }
            catch (SocketException ex)
            {
                // ❌ Outros erros de socket
                Console.WriteLine($"❌ Erro Code: {ex.SocketErrorCode} Message {ex.Message}");
            }
        }

        public static async Task Exemplo3ConnectWithTimeoutAsync()
        {
            // Uso:
            Console.WriteLine("=== Exemplo3ConnectWithTimeoutAsync ===");
            var client = new TcpClient();
            try
            {
                await ConnectWithTimeoutAsync3(client, "192.168.1.100", 9100, 2000);
                Console.WriteLine("✅ ConnectWithTimeoutAsync3 - Conectado");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"⏱️ TIMEOUT: {ex.Message}");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused)
            {
                // ❌ Porta fechada/recusada
                Console.WriteLine("❌ Conexão recusada");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
            {
                // ❌ Host não encontrado (DNS falhou)
                Console.WriteLine("❌ Host não encontrado");
            }
            catch (SocketException ex)
            {
                // ❌ Outros erros de socket
                Console.WriteLine($"❌ Erro Code: {ex.SocketErrorCode} Message {ex.Message}");
            }
        }

        public static async Task Exemplo4ConnectWithTimeoutAsync()
        {
            // Uso:
            try
            {
                Console.WriteLine("=== Exemplo4ConnectWithTimeoutAsync ===");
                var client = await ConnectWithTimeoutAsync4("192.168.1.100", 9100, 2000);
                Console.WriteLine("✅ ConnectWithTimeoutAsync4 - Conectado");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"⏱️ TIMEOUT: {ex.Message}");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused)
            {
                // ❌ Porta fechada/recusada
                Console.WriteLine("❌ Conexão recusada");
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.HostNotFound)
            {
                // ❌ Host não encontrado (DNS falhou)
                Console.WriteLine("❌ Host não encontrado");
            }
            catch (SocketException ex)
            {
                // ❌ Outros erros de socket
                Console.WriteLine($"❌ Erro Code: {ex.SocketErrorCode} Message {ex.Message}");
            }
        }

        public static async Task<bool> ConnectWithTimeoutAsync1(
            TcpClient client,
            string host,
            int port,
            int timeoutMs)
        {
            var connectTask = client.ConnectAsync(host, port);
            var timeoutTask = Task.Delay(timeoutMs);

            var completedTask = await Task.WhenAny(connectTask, timeoutTask)
                                          .ConfigureAwait(false);

            if (completedTask == timeoutTask)
            {
                // Timeout - fecha o cliente
                client.Close();
                throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {host}:{port}");
            }

            // Verifica se houve erro na conexão
            await connectTask; // Re-lança exceção se houver

            return client.Connected;
        }

        public static async Task<bool> ConnectWithTimeoutAsync2(
            TcpClient client,
            string host,
            int port,
            int timeoutMs)
        {
            using (var cts = new CancellationTokenSource(timeoutMs))
            {
                try
                {
                    // Infelizmente ConnectAsync não aceita CancellationToken no .NET Framework 4.7.2
                    // Então usamos Task.WhenAny mesmo
                    var connectTask = client.ConnectAsync(host, port);
                    var delayTask = Task.Delay(timeoutMs, cts.Token);

                    var completedTask = await Task.WhenAny(connectTask, delayTask)
                                                  .ConfigureAwait(false);

                    if (completedTask == delayTask)
                    {
                        client.Close();
                        throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {host}:{port}");
                    }

                    await connectTask;
                    return client.Connected;
                }
                catch (OperationCanceledException)
                {
                    client.Close();
                    throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {host}:{port}");
                }
            }
        }

        public static async Task<bool> ConnectWithTimeoutAsync3(
            TcpClient client,
            string host,
            int port,
            int timeoutMs)
        {
            var tcs = new TaskCompletionSource<bool>();

            // ✅ Usa o Socket do TcpClient, não o TcpClient.BeginConnect
            var socket = client.Client;

            // Resolve DNS primeiro
            var addresses = await Task.Run(() => System.Net.Dns.GetHostAddresses(host))
                                      .ConfigureAwait(false);

            if (addresses.Length == 0)
            {
                throw new SocketException((int)SocketError.HostNotFound);
            }

            var endPoint = new System.Net.IPEndPoint(addresses[0], port);

            IAsyncResult ar = socket.BeginConnect(endPoint, asyncResult =>
            {
                try
                {
                    socket.EndConnect(asyncResult);
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }, null);

            // Aguarda conexão ou timeout
            var timeoutTask = Task.Delay(timeoutMs);
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask)
                                          .ConfigureAwait(false);

            if (completedTask == timeoutTask)
            {
                client.Close();
                throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {host}:{port}");
            }

            return await tcs.Task;
        }

        public static async Task<TcpClient> ConnectWithTimeoutAsync4(
            string host,
            int port,
            int timeoutMs)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Define timeout de conexão
            socket.SendTimeout = timeoutMs;
            socket.ReceiveTimeout = timeoutMs;

            var tcs = new TaskCompletionSource<bool>();

            var ar = socket.BeginConnect(host, port, asyncResult =>
            {
                try
                {
                    socket.EndConnect(asyncResult);
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }, null);

            // Aguarda conexão ou timeout
            var timeoutTask = Task.Delay(timeoutMs);
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask)
                                          .ConfigureAwait(false);

            if (completedTask == timeoutTask)
            {
                socket.Close();
                throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {host}:{port}");
            }

            await tcs.Task;

            // Cria TcpClient a partir do socket conectado
            return new TcpClient { Client = socket };
        }

    }
}
