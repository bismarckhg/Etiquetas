using Etiquetas.Bibliotecas.Comum.Arrays;
using Etiquetas.Bibliotecas.Comum.Evento;
using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    public class StreamBaseTCPCliente : StreamBase
    {

        public StreamBaseTCPCliente()
        {
            // Constructor logic here
        }
        public override async Task ConectarAsync(params object[] parametros)
        {
            ThrowIfDisposed();

            if (parametros == null)
            {
                throw new ArgumentNullException("Parâmetros inválidos!");
            }

            var posicao = 0;
            TcpClient client = default;
            string serverIpAdress = string.Empty;
            int serverPort = 0;
            int timeout = 0;
            var cancellationBruto = CancellationToken.None;

            foreach (var item in parametros)
            {
                switch (item)
                {
                    case string texto:
                        serverIpAdress = texto;
                        break;
                    case int inteiro:
                        switch (posicao)
                        {
                            case 0:
                                serverPort = inteiro > 0
                                ? inteiro
                                : throw new ArgumentOutOfRangeException("Porta inválida!");
                                posicao++;
                                break;
                            case 1:
                                timeout = inteiro;
                                posicao++;
                                break;
                            default:
                                break;
                        }
                        break;
                    case TcpClient tcpClient:
                        client = tcpClient;
                        break;
                    case CancellationToken token:
                        cancellationBruto = token;
                        break;

                    default:
                        break;
                }
            }

            var hasServerIpAdress = !StringEhNuloVazioComEspacosBranco.Execute(serverIpAdress);
            var hasServerPort = serverPort > 0;

            if (hasServerIpAdress && hasServerPort)
            {
                await ConnectAsync(serverIpAdress, serverPort, timeout, cancellationBruto).ConfigureAwait(false);
                return;
            }

            if (client == default)
            {
                throw new ArgumentNullException("Ip Adress e TcpClient não informado!");
            }
            await ConnectAsync(client, cancellationBruto).ConfigureAwait(false);
            return;
        }

        public override async Task ConectarAsync(ITaskParametros parametros)
        {
            ThrowIfDisposed();

            if (parametros == null)
            {
                throw new ArgumentNullException("Parâmetros inválidos!");
            }
            var cancellationToken = parametros.RetornaSeExistir<CancellationToken>("CancellationBruto");
            var timeout = parametros.RetornaSeExistir<int>("Timeout");
            TcpClient client = default;

            client = parametros.RetornaSeExistir<TcpClient>("TcpClient");
            if (client == default)
            {
                var serverIpAdress = parametros.RetornaSeExistir<string>("ServerIpAdress");
                var serverPort = parametros.RetornaSeExistir<int>("ServerPort");
                var hasServerIpAdress = !StringEhNuloVazioComEspacosBranco.Execute(serverIpAdress);
                var hasServerPort = serverPort > 0;

                if (hasServerIpAdress && hasServerPort)
                {
                    await ConnectAsync(serverIpAdress, serverPort, timeout, cancellationToken).ConfigureAwait(false);
                    return;
                }

                throw new ArgumentNullException("Ip Adress e TcpClient não informado!");
            }

            await ConnectAsync(client, cancellationToken).ConfigureAwait(false);
            return;
        }

        public override Task ConectarReaderOnlyUnshareAsync()
        {
            throw new NotImplementedException();
        }

        public override Task ConectarWriterAndReaderUnshareAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="serverIpAdress">IP Servidor de conexão</param>
        /// <param name="serverPort">Porta Servidor de conexão</param>
        /// <param name="timeout">Time Out de conexao com o Servidor</param>
        /// <param name="cancellationBruto">Token de cancelamento</param>
        private async Task ConnectAsync(
            string serverIpAdress,
            int serverPort,
            int timeoutMs = Timeout.Infinite,
            CancellationToken cancellationBruto = default)
        {
            var client = new TcpClient();

            using (var timeoutCts = new CancellationTokenSource(timeoutMs))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationBruto, timeoutCts.Token))
            {
                try
                {
                    // Infelizmente ConnectAsync não aceita CancellationToken no .NET Framework 4.7.2
                    // Então usamos Task.WhenAny mesmo
                    
                    var connectTask = client.ConnectAsync(serverIpAdress, serverPort);
                    var delayTask = Task.Delay(timeoutMs, linkedCts.Token);

                    var completedTask = await Task.WhenAny(connectTask, delayTask)
                                                  .ConfigureAwait(false);

                    if (completedTask == delayTask)
                    {
                        client.Close();
                        throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {host}:{port}");
                    }

                    await connectTask;

                    // Cada cliente é tratado em sua própria tarefa (não bloqueia o processo)
                    _ = Task.Run(() => HandleClientAsync(client, cancellationBruto), cancellationBruto);

                }
                catch (OperationCanceledException)
                {
                    client.Close();
                    throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {host}:{port}");
                }
            }
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="cancellationBruto">Token de cancelamento</param>
        private async Task ConnectAsync(
            TcpClient client,
            CancellationToken cancellationBruto = default)
        {
            try
            {
                // .NET 4.5 não tem ConnectAsync nativo, usamos Task.Run com timeout
                var connectTask = Task.Run(() =>
                {
                    client = client ?? throw new ArgumentNullException(nameof(client));
                }, cancellationBruto);

                var conectado = EstaAberto();

                if (conectado)
                {
                    var tcpClientEndPoint = client.Client.RemoteEndPoint as System.Net.IPEndPoint;
                    var tcpClientIPAddress = tcpClientEndPoint?.Address.ToString() ?? "Desconhecido";
                    var tcpClientPort = tcpClientEndPoint?.Port ?? 0;
                }

                await connectTask; // Aguarda a conexão completar ou propagar exceção
            }
            catch (OperationCanceledException)
            {
                throw; // Re-lança OperationCanceledException sem encapsular
            }
            catch (Exception ex)
            {
                var tcpClientEndPoint = client.Client.RemoteEndPoint as System.Net.IPEndPoint;
                var tcpClientIPAddress = tcpClientEndPoint?.Address.ToString() ?? "Desconhecido";
                var tcpClientPort = tcpClientEndPoint?.Port ?? 0;
                throw new InvalidOperationException($"Erro ao conectar com {tcpClientIPAddress}:{tcpClientPort}: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public override bool EstaAberto(TcpClient client)
        {
            var retorno = (client?.Connected ?? false)
                && (client?.Client?.Connected ?? false);

            return retorno;
        }

        /// <inheritdoc/>
        public override Task FecharAsync()
        {
            TCPClient?.Close();
            TCPClient?.Dispose();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override bool PossuiDados()
        {
            if (TCPClient == null)
                return false;

            try
            {
                //var conectado = TCPClient.Connected;
                //var socket = TCPClient.Client;
                //conectado = conectado && socket != null && socket.Connected;

                var conectado = EstaAberto();
                var tem = (TCPClient?.Available > 0);
                // Método mais confiável: Poll + Available
                // Poll com SelectMode.SelectRead retorna true se:
                // - Há dados para ler
                // - A conexão foi fechada
                // - A conexão foi resetada
                var socket = TCPClient.Client;
                bool pollResult = socket.Poll(1000, SelectMode.SelectRead);
                // Se Poll retorna true MAS não há dados, a conexão foi fechada
                bool hasData = socket.Available > 0;
                var temDados = conectado && pollResult && hasData;

                return temDados;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (stDisposed) return;
            if (disposing)
            {
                // Libera recursos gerenciados aqui
                TCPClient?.Close();
                TCPClient?.Dispose();
                TCPClient = null;
            }
            // Libera recursos não gerenciados aqui
            stDisposed = true;
        }
    }
}
