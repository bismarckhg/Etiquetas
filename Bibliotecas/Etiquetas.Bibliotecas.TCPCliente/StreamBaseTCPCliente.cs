using Etiquetas.Bibliotecas.Comum.Arrays;
using Etiquetas.Bibliotecas.Comum.Evento;
using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    public class StreamBaseTCPCliente : StreamBase
    {
        protected TcpClient TCPClient { get; set; }

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
                        this.TCPClient = tcpClient;
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

            if (TCPClient == default)
            {
                throw new ArgumentNullException("Ip Adress e TcpClient não informado!");
            }
            await ConnectAsync(cancellationBruto).ConfigureAwait(false);
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

            this.TCPClient = parametros.RetornaSeExistir<TcpClient>("TcpClient");
            if (this.TCPClient == default)
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

            await ConnectAsync(cancellationToken).ConfigureAwait(false);
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
            this.TCPClient = new TcpClient();

            using (var timeoutCts = new CancellationTokenSource(timeoutMs))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationBruto, timeoutCts.Token))
            {
                try
                {
                    // Infelizmente ConnectAsync não aceita CancellationToken no .NET Framework 4.7.2
                    // Então usamos Task.WhenAny mesmo
                    var byteAdress = Etiquetas.Bibliotecas.Comum.Caracteres.StringIP.EmByteArray(serverIpAdress);
                    var ipAddress = Etiquetas.Bibliotecas.Comum.Arrays.ByteArrayEmIPAddress.Execute(byteAdress);

                    this.TCPClient.Connect(ipAddress, serverPort);

                    var tcpClient = new TcpClient();

                    var connectTask = Task.Run(() => // Usamos async aqui para poder usar await dentro, se necessário
                    {
                        try
                        {
                            // O CancellationToken pode ser verificado antes de iniciar a conexão
                            linkedCts.Token.ThrowIfCancellationRequested();

                            // Para .NET Framework 4.7.2, TcpClient.Connect() é síncrono.
                            // Para .NET 6+, você pode usar tcpClient.ConnectAsync().
                            // Vamos usar a versão síncrona para compatibilidade com o seu código original e .NET 4.7.2.
                            tcpClient.Connect(ipAddress, serverPort);
                            return tcpClient;
                        }
                        catch (OperationCanceledException)
                        {
                            // Se a Task foi cancelada antes de iniciar a conexão, ou durante a espera.
                            // Descartar o cliente se ele foi criado mas não conectado.
                            tcpClient.Dispose();
                            throw; // Relançar a exceção de cancelamento
                        }
                        catch
                        {
                            // Em caso de qualquer outra exceção durante a conexão, descartar o cliente.
                            tcpClient.Dispose();
                            throw;
                        }
                    }, linkedCts.Token); // Passa o token para Task.Run para que a Task possa ser cancelada antes de iniciar

                    var delayTask = Task.Delay(timeoutMs, linkedCts.Token);

                    var completedTask = await Task.WhenAny(connectTask, delayTask)
                                                    .ConfigureAwait(false);

                    if (completedTask == delayTask)
                    {
                        await FecharAsync().ConfigureAwait(false); // O que FecharAsync faz?
                        throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {serverIpAdress}:{serverPort}");
                    }
                    // await connectTask; // Linha problemática

                    // Cada cliente é tratado em sua própria tarefa (não bloqueia o processo)
                }
                catch (OperationCanceledException)
                {
                    await FecharAsync().ConfigureAwait(false);
                    throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {serverIpAdress}:{serverPort}");
                }
            }
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="cancellationBruto">Token de cancelamento</param>
        private async Task ConnectAsync(
            CancellationToken cancellationBruto = default)
        {
            try
            {
                // .NET 4.5 não tem ConnectAsync nativo, usamos Task.Run com timeout
                var connectTask = Task.Run(() =>
                {
                    this.TCPClient = this.TCPClient ?? throw new ArgumentNullException(nameof(client));
                }, cancellationBruto);

                var conectado = EstaAberto();

                if (conectado)
                {
                    var tcpClientEndPoint = this.TCPClient.Client.RemoteEndPoint as System.Net.IPEndPoint;
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
                var tcpClientEndPoint = this.TCPClient.Client.RemoteEndPoint as System.Net.IPEndPoint;
                var tcpClientIPAddress = tcpClientEndPoint?.Address.ToString() ?? "Desconhecido";
                var tcpClientPort = tcpClientEndPoint?.Port ?? 0;
                throw new InvalidOperationException($"Erro ao conectar com {tcpClientIPAddress}:{tcpClientPort}: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public override bool EstaAberto()
        {
            var retorno = (this.TCPClient?.Connected ?? false)
                && (this.TCPClient?.Client?.Connected ?? false);

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
                var temDados = conectado && (TCPClient?.Available > 0);
                // Método mais confiável: Poll + Available
                // Poll com SelectMode.SelectRead retorna true se:
                // - Há dados para ler
                // - A conexão foi fechada
                // - A conexão foi resetada
                //var socket = TCPClient.Client;
                //bool pollResult = socket.Poll(1000, SelectMode.SelectRead);
                // Se Poll retorna true MAS não há dados, a conexão foi fechada
                //bool hasData = socket.Available > 0;
                //var temDados = conectado && pollResult && hasData;

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
