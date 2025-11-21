using Etiquetas.Bibliotecas.Comum.Geral;
using Etiquetas.Bibliotecas.Rede;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    public class TCPClient : IDisposable
    {
        protected TcpClient TcpCliente { get; set; }
        protected CancellationToken CancellationTokenStop { get; }
        protected CancellationToken CancellationTokenBreak { get; }

        protected bool ThrowOnTimeout;

        protected int TimeoutMS { get; }

        protected Encoding TypeEncoding { get; set; }

        public TCPClient(
            CancellationToken cancellationTokenStop,
            CancellationToken cancellationTokenBreak,
            int timeoutMs = Timeout.Infinite,
            bool throwOnTimeout = false,
            Encoding encoding = null
            )
        {
            // Constructor logic here
            this.CancellationTokenBreak = cancellationTokenBreak;
            this.CancellationTokenStop = cancellationTokenStop;
            this.TimeoutMS = timeoutMs;

            // Melhor usar UTF8SemBom como padrão para ZPL, JSON etc.
            this.TypeEncoding = encoding ?? ConversaoEncoding.UTF8SemBom;
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
                if (TcpCliente.ReceiveTimeout != TimeoutMS)
                {
                    this.TcpCliente.ReceiveTimeout = TimeoutMS;
                }

                if (TcpCliente.Client.ReceiveTimeout != TimeoutMS)
                {
                    this.TcpCliente.Client.ReceiveTimeout = TimeoutMS;
                }

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
                TcpCliente?.Dispose();
                throw; // Re-lança OperationCanceledException sem encapsular
            }
            catch (ArgumentNullException)
            {
                TcpCliente?.Dispose();
                throw; // Re-lança ArgumentNullException sem encapsular
            }
            catch (Exception ex)
            {
                var tcpClientEndPoint = this.TcpCliente.Client.RemoteEndPoint as System.Net.IPEndPoint;
                var enderecoRede = new EnderecoRede(tcpClientEndPoint);
                var tcpClientIPAddress = enderecoRede.ObtemEnderecoIP();
                var tcpClientPort = enderecoRede.ObtemPorta();
                TcpCliente?.Dispose();
                throw new InvalidOperationException($"Erro: {ex.Message}\r\nAo conectar com {tcpClientIPAddress}:{tcpClientPort}: ", ex.InnerException);
            }
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="serverIpAdress">IP Servidor de conexão</param>
        /// <param name="serverPort">Porta Servidor de conexão</param>
        public async Task ConnectAsync(
            string serverIpAdress,
            int serverPort)
        {

            try
            {

                var enderecoRede = new EnderecoRede(serverIpAdress, serverPort);
                var tcpConnector = new TcpConnector(enderecoRede);
                await tcpConnector.ConnectWithTimeoutAsync(CancellationTokenBreak, this.TimeoutMS).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro: {ex.Message}\r\nAo conectar com {serverIpAdress}:{serverPort}: ", ex.InnerException);
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

        /// <summary>
        /// Faz a leitura do servidor TCP e retorna os dados recebidos em um array de bytes(buffer).
        /// </summary>

        public async Task<byte[]> ReaderBufferTCP()
        {
            if (!EstaAberto())
            {
                throw new InvalidOperationException("Conexão TCP não está aberta.");
            }

            CancellationTokenBreak.ThrowIfCancellationRequested();

            var netStream = TcpCliente.GetStream();

            if (netStream.ReadTimeout != TimeoutMS)
            {
                netStream.ReadTimeout = TimeoutMS;
            }

            var tamanhoLeitura = TcpCliente.Available;
            var byteTemp = new byte[tamanhoLeitura];

            var tentativas = 5;
            for (int i = 0; i < tentativas; i++)
            {
                if (tamanhoLeitura != 0)
                {
                    break;
                }
                byteTemp = await LerComTimeoutAsync(netStream, tamanhoLeitura).ConfigureAwait(false);

                await Task.Delay(100, CancellationTokenBreak).ConfigureAwait(false);
                tamanhoLeitura = TcpCliente.Available;
            }

            if (tamanhoLeitura != 0)
            {
                byteTemp = await LerComTimeoutAsync(netStream, tamanhoLeitura).ConfigureAwait(false);
            }
            return byteTemp;
        }

        protected async Task<byte[]> LerComTimeoutAsync(
                NetworkStream netStream,
                int tamanhoLeitura
            )
        {
            // Verifica cancelamento antes de começar
            CancellationTokenBreak.ThrowIfCancellationRequested();

            try
            {
                var dadosLidos = 0;
                var buffer = new byte[tamanhoLeitura];

                // Executa Read síncrono em thread separada
                return await Task.Run(async () =>
                {
                    try
                    {
                        dadosLidos = await netStream.ReadAsync(buffer, 0, tamanhoLeitura).ConfigureAwait(false);
                        return buffer;
                    }
                    catch (IOException ex) when (ex.InnerException is SocketException se &&
                                                  se.SocketErrorCode == SocketError.TimedOut)
                    {
                        if (!ThrowOnTimeout)
                        {
                            if (dadosLidos > 0)
                            {
                                return buffer; // Retorna bytes lidos antes do timeout
                            }
                            return new byte[0]; // Saida por Timeout sem exceção
                        }

                        throw new TimeoutException($"Timeout de {TimeoutMS}ms na leitura", ex);
                    }
                }, CancellationTokenBreak).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }
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
