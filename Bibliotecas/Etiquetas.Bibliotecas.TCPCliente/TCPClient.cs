using Etiquetas.Bibliotecas.Rede;
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
    public class TCPClient : IDisposable
    {
        protected TcpClient TcpCliente { get; set; }
        protected CancellationToken CancellationTokenStop { get; set; }
        protected CancellationToken CancellationTokenBreak { get; set; }

        public TCPClient(CancellationToken cancellationTokenStop, CancellationToken cancellationTokenBreak)
        {
            // Constructor logic here
            CancellationTokenBreak = cancellationTokenBreak;
            CancellationTokenStop = cancellationTokenStop;
        }

        public TcpClient ObtemTcpClient()
        {
            return this.TcpCliente;
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="cancellationBruto">Token de cancelamento</param>
        public async Task ConnectAsync(
            TcpClient tcpCliente
            )
        {
            try
            {
                // .NET 4.5 não tem ConnectAsync nativo, usamos Task.Run com timeout
                var connectTask = Task.Run(() =>
                {
                    this.TcpCliente = tcpCliente ?? throw new ArgumentNullException(nameof(tcpCliente));
                }, CancellationTokenBreak);

                var conectado = EstaAberto();

                if (conectado)
                {
                    var tcpClientEndPoint = this.TcpCliente.Client.RemoteEndPoint as System.Net.IPEndPoint;
                    var enderecoRede = new EnderecoRede(tcpClientEndPoint);
                    var tcpClientIPAddress = enderecoRede.ObtemEnderecoIP();
                    var tcpClientPort = enderecoRede.ObtemPorta();
                }

                await connectTask; // Aguarda a conexão completar ou propagar exceção
            }
            catch (OperationCanceledException)
            {
                throw; // Re-lança OperationCanceledException sem encapsular
            }
            catch (ArgumentNullException)
            {
                throw; // Re-lança ArgumentNullException sem encapsular
            }
            catch (Exception ex)
            {
                var tcpClientEndPoint = this.TcpCliente.Client.RemoteEndPoint as System.Net.IPEndPoint;
                var enderecoRede = new EnderecoRede(tcpClientEndPoint);
                var tcpClientIPAddress = enderecoRede.ObtemEnderecoIP();
                var tcpClientPort = enderecoRede.ObtemPorta();
                throw new InvalidOperationException($"Erro ao conectar com {tcpClientIPAddress}:{tcpClientPort}: {ex.Message}", ex.InnerException);
            }
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="serverIpAdress">IP Servidor de conexão</param>
        /// <param name="serverPort">Porta Servidor de conexão</param>
        /// <param name="timeout">Time Out de conexao com o Servidor</param>
        /// <param name="cancellationBruto">Token de cancelamento</param>
        public async Task ConnectAsync(
            string serverIpAdress,
            int serverPort,
            int timeoutMs = Timeout.Infinite)
        {

            try
            {
                var enderecoRede = new EnderecoRede(serverIpAdress, serverPort);
                var tcpConnector = new TcpConnector(enderecoRede);
                await tcpConnector.ConnectWithTimeoutAsync(CancellationTokenBreak, timeoutMs).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public bool EstaAberto()
        {
            var retorno = (this.TcpCliente?.Connected ?? false)
                && (this.TcpCliente?.Client?.Connected ?? false);

            return retorno;
        }

        /// <inheritdoc/>
        public Task FecharAsync()
        {
            this.TcpCliente?.Close();
            this.TcpCliente?.Dispose();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public bool PossuiDados()
        {
            if (this.TcpCliente == null)
            {
                return false;
            }

            try
            {
                //var conectado = TCPClient.Connected;
                //var socket = TCPClient.Client;
                //conectado = conectado && socket != null && socket.Connected;

                var conectado = EstaAberto();
                var temDados = conectado && (this.TcpCliente?.Available > 0);

                // Verificar Depois
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

        /// <summary>
        /// Faz a leitura do servidor TCP e retorna os dados recebidos em um array de bytes(buffer).
        /// </summary>

        public byte[] ReaderBufferTCP(
            int tamanhoBuffer,
            int timeoutMs = Timeout.Infinite)
        {
            if (!EstaAberto())
            {
                throw new InvalidOperationException("Conexão TCP não está aberta.");
            }

            CancellationTokenBreak.ThrowIfCancellationRequested();
            var netstream = TcpCliente.GetStream();
            var tamanhoLeitura = TcpCliente.Available;
            if (tamanhoLeitura == 0)
            {
                
            }
        }

        protected async Task<int> LerComTimeoutAsync(
                byte[] buffer,
                int tamanho,
                int timeoutMs,
                bool throwOnTimeout = false,
                CancellationToken cancellationBreak = default)
        {

        }

        #region IDisposable Support

        protected bool stDisposed = false;

        /// <summary>
        /// Realiza a liberação de recursos.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método protegido para que as classes derivadas possam implementar a lógica de descarte.
        /// </summary>
        /// <param name="disposing">Indica se a chamada vem do método Dispose().</param>
        protected virtual void Dispose(bool disposing)
        {
            if (stDisposed) return;

            if (disposing)
            {
                // Libera recursos gerenciados aqui
                FecharAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }

            // Libera recursos não gerenciados aqui

            stDisposed = true;
        }

        #endregion
    }
}
