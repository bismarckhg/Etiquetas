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
        protected TcpClient TCPClient;

        public override event EventHandler<ErrorEventArgs> ErrorOccurred;

        public override void OnErrorOccurred(Exception exception)
        {
            ErrorOccurred?.Invoke(this, new ErrorEventArgs(exception));
        }

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
            string serverIpAdress = string.Empty;
            int serverPort = 0;
            int timeout = 0;
            var cancellationToken = CancellationToken.None;
            TcpClient tcpClient = null;

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

                    case TcpClient client:
                        tcpClient = client;
                        break;
                    case CancellationToken token:
                        cancellationToken = token;
                        break;

                    default:
                        break;
                }
            }

            var hasServerIpAdress = !StringEhNuloVazioComEspacosBranco.Execute(serverIpAdress);
            var hasServerPort = serverPort > 0;

            if (hasServerIpAdress && hasServerPort)
            {
                await ConnectAsync(serverIpAdress, serverPort, timeout, cancellationToken).ConfigureAwait(false);
                return;
            }

            if (tcpClient == default)
            {
                throw new ArgumentNullException("Ip Adress e TcpClient não informado!");
            }
            await ConnectAsync(tcpClient, cancellationToken).ConfigureAwait(false);
            return;
        }

        public override async Task ConectarAsync(ITaskParametros parametros)
        {
            ThrowIfDisposed();

            if (parametros == null)
            {
                throw new ArgumentNullException("Parâmetros inválidos!");
            }
            var cancellationToken = parametros.RetornoCancellationToken;
            var timeout = parametros.RetornaSeExistir<int>("Timeout");


            var tcpClient = parametros.RetornaSeExistir<TcpClient>("TcpClient");
            if (tcpClient == default)
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

            await ConnectAsync(tcpClient, cancellationToken).ConfigureAwait(false);
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
        /// <param name="tcpClient">Cliente TCP</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        private async Task ConnectAsync(string serverIpAdress, int serverPort, int timeout = 0, CancellationToken cancellationToken = default)
        {
            try
            {
                // .NET 4.5 não tem ConnectAsync nativo, usamos Task.Run com timeout
                var connectTask = Task.Run(() =>
                {
                    var ipAddress = System.Net.IPAddress.Parse(serverIpAdress);
                    TCPClient = new TcpClient();
                    TCPClient.Connect(ipAddress, serverPort);
                }, cancellationToken);

                // Aplica timeout de conexão
                var timeoutTask = Task.Delay(timeout, cancellationToken);
                var completedTask = await Task.WhenAny(connectTask, timeoutTask).ConfigureAwait(false);

                var conectado = EstaAberto();

                if (completedTask == timeoutTask && !conectado)
                {
                    throw new TimeoutException($"Timeout {timeout}ms ao tentar conectar com {serverIpAdress}:{serverPort}");
                }

                await connectTask; // Aguarda a conexão completar ou propagar exceção
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao conectar com {serverIpAdress}:{serverPort}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="tcpClient">Cliente TCP</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        private async Task ConnectAsync(TcpClient tcpClient, CancellationToken cancellationToken = default)
        {
            try
            {
                // .NET 4.5 não tem ConnectAsync nativo, usamos Task.Run com timeout
                var connectTask = Task.Run(() =>
                {
                    TCPClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
                }, cancellationToken);

                var conectado = EstaAberto();

                if (conectado)
                {
                    var tcpClientEndPoint = TCPClient.Client.RemoteEndPoint as System.Net.IPEndPoint;
                    var tcpClientIPAddress = tcpClientEndPoint?.Address.ToString() ?? "Desconhecido";
                    var tcpClientPort = tcpClientEndPoint?.Port ?? 0;

                    throw new TimeoutException($"Timeout ao conectar com {tcpClientIPAddress}:{tcpClientPort}");
                }

                await connectTask; // Aguarda a conexão completar ou propagar exceção
            }
            catch (Exception ex)
            {
                var tcpClientEndPoint = TCPClient.Client.RemoteEndPoint as System.Net.IPEndPoint;
                var tcpClientIPAddress = tcpClientEndPoint?.Address.ToString() ?? "Desconhecido";
                var tcpClientPort = tcpClientEndPoint?.Port ?? 0;
                throw new InvalidOperationException($"Erro ao conectar com {tcpClientIPAddress}:{tcpClientPort}: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public override bool EstaAberto()
        {
            // return TCPClient?.Connected ?? false;
            var conectado = TCPClient?.Connected ?? false;
            var socket = TCPClient?.Client;
            return conectado && socket != null && socket.Connected;
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

                // Método mais confiável: Poll + Available
                // Poll com SelectMode.SelectRead retorna true se:
                // - Há dados para ler
                // - A conexão foi fechada
                // - A conexão foi resetada
                var socket = TCPClient.Client;
                bool pollResult = socket.Poll(1000, SelectMode.SelectRead);
                // Se Poll retorna true MAS não há dados, a conexão foi fechada
                bool hasData = socket.Available > 0;
                conectado = conectado && pollResult && hasData;

                return conectado;
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
