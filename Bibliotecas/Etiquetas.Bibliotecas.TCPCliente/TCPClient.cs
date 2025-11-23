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
        protected TcpClient Client { get; set; }
        protected CancellationToken CancellationTokenStop { get; }
        protected CancellationToken CancellationTokenBreak { get; }

        protected bool ThrowOnTimeout { get; }

        protected int TimeoutMS { get; }

        protected int TamanhoBuffer { get; }

        protected Encoding TypeEncoding { get; set; }
        /// <summary>
        ///     ctor TCPClient
        /// </summary>
        /// <param name="cancellationTokenStop">Interrupção na finalizacao de um processo completo.</param>
        /// <param name="cancellationTokenBreak">Interrupção Bruta de um processo sem finalização.</param>
        /// <param name="tamanhoBuffer"> tamanho dos Buffers de leitura e envio do TcpClient</param>
        /// <param name="timeoutMs">Timeout para conexao, leitura e envio.</param>
        /// <param name="throwOnTimeout">Timeout interrupção por erro, ou apenas interrupção.</param>
        /// <param name="encoding">Encoding para texto a ser recebido ou enviado.</param>
        public TCPClient(
            TcpClient tcpClient,
            CancellationToken cancellationTokenStop,
            CancellationToken cancellationTokenBreak,
            int tamanhoBuffer = 8192,
            int timeoutMs = Timeout.Infinite,
            bool throwOnTimeout = false,
            Encoding encoding = null
        ) : this(
                cancellationTokenStop,
                cancellationTokenBreak,
                tamanhoBuffer,
                timeoutMs,
                throwOnTimeout,
                encoding
            )
        {
            this.Client = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
        }

        /// <summary>
        ///     ctor TCPClient
        /// </summary>
        /// <param name="cancellationTokenStop">Interrupção na finalizacao de um processo completo.</param>
        /// <param name="cancellationTokenBreak">Interrupção Bruta de um processo sem finalização.</param>
        /// <param name="tamanhoBuffer"> tamanho dos Buffers de leitura e envio do TcpClient</param>
        /// <param name="timeoutMs">Timeout para conexao, leitura e envio.</param>
        /// <param name="throwOnTimeout">Timeout interrupção por erro, ou apenas interrupção.</param>
        /// <param name="encoding">Encoding para texto a ser recebido ou enviado.</param>
        public TCPClient(
            CancellationToken cancellationTokenStop,
            CancellationToken cancellationTokenBreak,
            int tamanhoBuffer = 8192,
            int timeoutMs = Timeout.Infinite,
            bool throwOnTimeout = false,
            Encoding encoding = null
        )
        {
            this.Client = default;
            this.ThrowOnTimeout = throwOnTimeout;
            // Constructor logic here
            this.CancellationTokenBreak = cancellationTokenBreak;
            this.CancellationTokenStop = cancellationTokenStop;
            this.TamanhoBuffer = tamanhoBuffer;
            this.TimeoutMS = timeoutMs;

            // Melhor usar UTF8SemBom como padrão para ZPL, JSON etc.
            this.TypeEncoding = encoding ?? ConversaoEncoding.UTF8SemBom;
        }

        public TcpClient ObtemTcpClient()
        {
            return this.Client;
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="cancellationBruto">Token de cancelamento</param>
        public async Task ConnectAsync()
        {
            try
            {
                this.CancellationTokenBreak.ThrowIfCancellationRequested();

                Client.ReceiveBufferSize = TamanhoBuffer;
                Client.SendBufferSize = TamanhoBuffer;
                Client.Client.ReceiveBufferSize = TamanhoBuffer;
                Client.Client.SendBufferSize = TamanhoBuffer;

                if (Client.ReceiveTimeout != TimeoutMS)
                {
                    this.Client.ReceiveTimeout = TimeoutMS;
                }

                if (Client.Client.ReceiveTimeout != TimeoutMS)
                {
                    this.Client.Client.ReceiveTimeout = TimeoutMS;
                }

                if (Client.SendTimeout != TimeoutMS)
                {
                    this.Client.ReceiveTimeout = TimeoutMS;
                }

                if (Client.Client.SendTimeout != TimeoutMS)
                {
                    this.Client.Client.ReceiveTimeout = TimeoutMS;
                }

                //// .NET 4.5 não tem ConnectAsync nativo, usamos Task.Run com timeout
                //var connectTask = Task.Run(() =>
                //{
                //    this.Client.Connect();

                //}, CancellationTokenBreak);

                var conectado = EstaAberto();

                if (conectado)
                {
                    var tcpClientEndPoint = this.Client.Client.RemoteEndPoint as System.Net.IPEndPoint;
                    var enderecoRede = new EnderecoRede(tcpClientEndPoint);
                    var tcpClientIPAddress = enderecoRede.ObtemEnderecoIP();
                    var tcpClientPort = enderecoRede.ObtemPorta();
                }

                //await connectTask; // Aguarda a conexão completar ou propagar exceção
            }
            catch (OperationCanceledException)
            {
                Client?.Dispose();
                throw; // Re-lança OperationCanceledException sem encapsular
            }
            catch (ArgumentNullException)
            {
                Client?.Dispose();
                throw; // Re-lança ArgumentNullException sem encapsular
            }
            catch (Exception ex)
            {
                var tcpClientEndPoint = this.Client.Client.RemoteEndPoint as System.Net.IPEndPoint;
                var enderecoRede = new EnderecoRede(tcpClientEndPoint);
                var tcpClientIPAddress = enderecoRede.ObtemEnderecoIP();
                var tcpClientPort = enderecoRede.ObtemPorta();
                Client?.Dispose();
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
            int serverPort
        )
        {
            try
            {
                this.CancellationTokenBreak.ThrowIfCancellationRequested();

                var enderecoRede = new EnderecoRede(serverIpAdress, serverPort);
                var tcpConnector = new TcpConnector(enderecoRede);
                await tcpConnector.ConnectWithTimeoutAsync(CancellationTokenBreak, this.TimeoutMS).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro: {ex.Message}\r\nAo conectar com {serverIpAdress}:{serverPort}: ", ex.InnerException);
            }
        }

        /// <summary>
        /// Determina se a conexão TCP está aberta.
        /// </summary>
        /// <returns>Retorna verdadeiro se a conexao TCPClient esta aberta.</returns>
        public bool EstaAberto()
        {
            var retorno = (this.Client?.Connected ?? false)
                && (this.Client?.Client?.Connected ?? false);

            return retorno;
        }

        /// <inheritdoc/>
        public Task FecharAsync()
        {
            this.Client?.Close();
            this.Client?.Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Determina se há dados disponíveis para leitura na conexão TCP.
        /// </summary>
        /// <returns>Retorna verdadeiro se houver dados disponíveis para leitura.</returns>
        public bool PossuiDados()
        {
            if (this.Client == null)
            {
                return false;
            }

            //var conectado = TCPClient.Connected;
            //var socket = TCPClient.Client;
            //conectado = conectado && socket != null && socket.Connected;

            var conectado = EstaAberto();
            var temDados = conectado && (this.Client?.Available > 0);

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
        /// <returns>Array de bytes com os dados recebidos</returns>
        public async Task<byte[]> LerBufferAsync()
        {
            try
            {
                CancellationTokenBreak.ThrowIfCancellationRequested();

                if (!EstaAberto())
                {
                    throw new InvalidOperationException("Conexão TCP não está aberta.");
                }

                var netStream = Client.GetStream();

                if (netStream.ReadTimeout != TimeoutMS)
                {
                    netStream.ReadTimeout = TimeoutMS;
                }

                var tamanhoLeitura = Client.Available;
                var byteTemp = new byte[tamanhoLeitura];

                var tentativas = 5;
                for (int i = 0; i < tentativas; i++)
                {
                    CancellationTokenBreak.ThrowIfCancellationRequested();

                    if (tamanhoLeitura != 0)
                    {
                        break;
                    }
                    byteTemp = await LerComOuSemTimeoutAsync(netStream, tamanhoLeitura).ConfigureAwait(false);

                    await Task.Delay(100, CancellationTokenBreak).ConfigureAwait(false);
                    tamanhoLeitura = Client.Available;
                }

                if (tamanhoLeitura != 0)
                {
                    CancellationTokenBreak.ThrowIfCancellationRequested();
                    byteTemp = await LerComOuSemTimeoutAsync(netStream, tamanhoLeitura).ConfigureAwait(false);
                }
                return byteTemp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Leitura do TcpClient com ou sem timeout
        /// </summary>
        /// <param name="netStream">NetworkStream da conexao TcpClient</param>
        /// <param name="tamanhoLeitura">Tamanho da leitura em bytes</param>
        protected async Task<byte[]> LerComOuSemTimeoutAsync(
            NetworkStream netStream,
            int tamanhoLeitura
        )
        {
            try
            {
                CancellationTokenBreak.ThrowIfCancellationRequested();

                using (var timeoutCts = new CancellationTokenSource(this.TimeoutMS))
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(this.CancellationTokenBreak, timeoutCts.Token))
                {
                    var buffer = new byte[tamanhoLeitura];
                    // Declara dadosLidos aqui para ser acessível após a tarefa
                    int dadosLidos = 0;

                    // A readTask agora retornará um int (o número de bytes lidos)
                    // e não usaremos ConfigureAwait(false) diretamente na atribuição da Task,
                    // mas sim dentro da lambda se necessário.
                    var readTask = Task.Run(async () =>
                    {
                        try
                        {
                            // netStream.ReadAsync no .NET Framework 4.7.2 não aceita CancellationToken
                            // A lógica de timeout será tratada pelo Task.WhenAny
                            dadosLidos = await netStream.ReadAsync(buffer, 0, tamanhoLeitura).ConfigureAwait(false);
                            return dadosLidos; // Retorna o número de bytes lidos
                        }
                        catch (IOException ex) when (ex.InnerException is SocketException se &&
                                                     se.SocketErrorCode == SocketError.TimedOut)
                        {
                            if (!ThrowOnTimeout)
                            {
                                // Se houve timeout do socket, mas já lemos algo, retornamos o que foi lido.
                                // Caso contrário, retornamos 0 para indicar que nada foi lido.
                                return dadosLidos;
                            }
                            throw new TimeoutException($"Timeout de {TimeoutMS}ms na leitura", ex);
                        }
                        catch (OperationCanceledException)
                        {
                            // Se a tarefa foi cancelada (por linkedCts.Token ou CancellationTokenBreak),
                            // tratamos aqui. Podemos retornar 0 ou relançar, dependendo da política.
                            // Para este cenário, vamos retornar 0 e deixar o Task.WhenAny lidar com o cancelamento do delayTask.
                            return 0;
                        }
                    }, linkedCts.Token); // Passamos o linkedCts.Token para Task.Run

                    // A delayTask também usa o linkedCts.Token para que ela possa ser cancelada
                    // se a readTask completar primeiro.
                    var delayTask = Task.Delay(this.TimeoutMS, linkedCts.Token);

                    // Task.WhenAny agora recebe duas Tasks do mesmo tipo (ou Task e Task<T>)
                    // readTask é Task<int>, delayTask é Task.
                    // Task.WhenAny<T> retorna Task<Task<T>> se todas forem Task<T>.
                    // Se houver Task e Task<T>, retorna Task<Task>.
                    // Para simplificar a comparação, podemos usar Task.WhenAny(Task[], CancellationToken)
                    // ou apenas comparar com as instâncias originais.
                    var completedTask = await Task.WhenAny(readTask, delayTask).ConfigureAwait(false);

                    if (completedTask == readTask)
                    {
                        // A leitura completou antes do timeout.
                        // Aguardamos a readTask para obter o resultado (o número de bytes lidos)
                        // e para propagar quaisquer exceções que possam ter ocorrido nela.
                        int bytesRead = await readTask.ConfigureAwait(false);

                        // Se a leitura foi bem-sucedida e leu bytes, retornamos o buffer com os dados.
                        // Caso contrário, retornamos um array vazio ou null, dependendo da sua convenção.
                        if (bytesRead > 0)
                        {
                            // Retorna uma cópia dos bytes lidos para evitar problemas se o buffer for reutilizado.
                            byte[] result = new byte[bytesRead];
                            //Array.Copy(buffer, result, bytesRead);
                            return result;
                        }
                        else
                        {
                            return new byte[0]; // Nenhum byte lido ou timeout do socket sem dados
                        }
                    }
                    else // completedTask == delayTask
                    {
                        // O timeout ocorreu (delayTask completou primeiro).
                        // Neste ponto, a readTask ainda está em execução ou foi cancelada pelo linkedCts.Token.
                        // É importante cancelar o linkedCts para que a readTask pare de tentar ler.
                        linkedCts.Cancel();

                        // Opcional: Aguardar a readTask para garantir que ela finalize e liberar recursos.
                        // Isso também propagaria OperationCanceledException se ela foi cancelada.
                        try
                        {
                            await readTask.ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            // Esperado quando linkedCts.Cancel() é chamado e readTask estava aguardando.
                            throw;
                        }
                        catch (Exception ex)
                        {
                            // Outras exceções da readTask que podem ter ocorrido antes do cancelamento.
                            //Console.WriteLine($"Exceção na readTask após timeout: {ex.Message}");
                            throw;
                        }

                        if (!ThrowOnTimeout)
                        {
                            // Se não devemos lançar exceção em timeout, retornamos um array vazio.
                            return new byte[0];
                        }
                        throw new TimeoutException($"Timeout de {TimeoutMS}ms na leitura");
                    }
                }
            }
            catch (Exception ex)
            {
                // Re-lança a exceção original, preservando o stack trace.
                throw;
            }
        }

        /// <summary>
        /// Gravação de buffer no servidor TCP de forma assíncrona.
        /// </summary>
        /// <param name="buffer">Buffer de dados a serem enviados</param>
        /// <param name="timeoutMs">Timeout de envio em milissegundos</param>
        /// <param name="addLineBreak">Indica se deve adicionar quebra de linha ao final (não usado aqui)</param>
        public async Task GravarBufferAsync(
            byte[] buffer,
            int timeoutMs = Timeout.Infinite,
            bool addLineBreak = true
        )
        {
            try
            {
                CancellationTokenBreak.ThrowIfCancellationRequested();

                if (!EstaAberto())
                {
                    throw new InvalidOperationException("Conexão TCP não está aberta.");
                }

                var netStream = Client.GetStream();

                if (netStream.WriteTimeout != timeoutMs)
                {
                    netStream.WriteTimeout = timeoutMs;
                }

                // Envia com timeout
                if (timeoutMs > 0 && timeoutMs != Timeout.Infinite)
                {
                    using (var timeoutCts = new CancellationTokenSource(timeoutMs))
                    using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(CancellationTokenBreak, timeoutCts.Token))
                    {
                        await WriteWithTimeoutAsync(netStream, buffer, linkedCts.Token).ConfigureAwait(false);
                    }
                }
                else
                {
                    await WriteWithTimeoutAsync(netStream, buffer, CancellationTokenBreak).ConfigureAwait(false);
                }

                // Garante que os dados foram enviados
                await FlushStreamAsync(netStream).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Helper para escrever no stream com cancellation
        /// </summary>
        /// <param name="netStream">NetworkStream da conexao TcpClient</param>
        /// <param name="buffer">Buffer de dados a serem enviados</param>
        /// <param name="cancellationToken">|Cancellation Token com timeout ou apenas Break</param>
        protected async Task WriteWithTimeoutAsync(
            NetworkStream netStream,
            byte[] buffer,
            CancellationToken cancellationToken = default
        )
        {
            await Task.Run(async () =>
            {
                try
                {
                    await netStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
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
        /// <param name="cancellationToken">|Cancellation Token com timeout ou apenas Break</param>
        /// <param name="netStream">NetworkStream da conexao TcpClient</param>
        protected async Task FlushStreamAsync(
            NetworkStream netStream
        )
        {
            await Task.Run(() =>
            {
                try
                {
                    netStream.Flush();
                }
                catch (Exception)
                {
                    throw;
                }
            }, CancellationTokenBreak).ConfigureAwait(false);
        }

        #region IDisposable Support

        /// <summary>
        /// Indica se o objeto já foi descartado.
        /// </summary>
        /// <returns>Retorna verdadeiro se o objeto ja foi descartado.</returns>
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
