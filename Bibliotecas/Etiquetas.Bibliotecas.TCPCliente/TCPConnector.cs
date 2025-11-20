using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    using Etiquetas.Bibliotecas.Rede;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class TcpConnector
    {
        // Supondo que ipAddress e serverPort já foram validados e são acessíveis
        private readonly IPAddress _ipAddress;
        private readonly int _serverPort;
        private readonly string _serverIpAdressString; // Para a mensagem de erro

        public TcpConnector()
        {
            _ipAddress = ipAddress;
            _serverPort = serverPort;
            _serverIpAdressString = $"{_ipAddress.ToString()}:{_serverPort.ToString()}";
        }

        public async Task<TcpClient> ConnectWithTimeoutAsync(CancellationToken cancellationTokenBruto, int timeoutMs)
        {
            // Usamos um CancellationTokenSource para coordenar o cancelamento
            // entre a conexão e o timeout, e para cancelar a conexão se o timeout ocorrer.
            using (var timeoutCts = new CancellationTokenSource(timeoutMs))
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenBruto, timeoutCts.Token))
            {
                // A tarefa de conexão usa ConnectAsync, que é não-bloqueante e assíncrono.
                // Passamos o token de cancelamento para que a operação possa ser abortada.
                var tcpClient = new TcpClient(); // O TcpClient é criado fora da Task para ser acessível
                                                 // para descarte em caso de timeout.

                // Task.Run é usado aqui para envolver a chamada assíncrona com o CancellationToken,
                // garantindo que a Task de conexão possa ser cancelada.
                // No entanto, ConnectAsync já aceita um CancellationToken diretamente no .NET 6+.
                // Se você estiver no .NET Framework 4.7.2, ConnectAsync não aceita CancellationToken.
                // Para .NET Framework 4.7.2, a abordagem original com Task.Run para a operação síncrona
                // é mais comum, mas ainda precisamos do cancelamento.

                // Vamos considerar a abordagem para .NET 6+ primeiro, que é mais limpa.
                // Se você estiver no .NET Framework 4.7.2, veja a alternativa abaixo.

                #region Abordagem para .NET 6+ (ConnectAsync com CancellationToken)
                // var connectTask = tcpClient.ConnectAsync(_ipAddress, _serverPort, cts.Token);
                #endregion

                #region Abordagem para .NET Framework 4.7.2 (e compatível com .NET 6+)
                // Para .NET Framework 4.7.2, ConnectAsync não tem sobrecarga com CancellationToken.
                // Precisamos de uma maneira de cancelar a operação de conexão se ela estiver em andamento.
                // Uma forma é usar um TaskCompletionSource e um Task.Run, mas é mais complexo.
                // A maneira mais simples de "cancelar" uma conexão TcpClient que não tem ConnectAsync(CancellationToken)
                // é fechar o socket subjacente ou o próprio TcpClient.
                // A abordagem original com Task.Run para a operação síncrona é válida, mas o cancelamento
                // da operação de rede em si é mais difícil sem a sobrecarga de CancellationToken.
                // No entanto, podemos cancelar a *espera* pela Task.

                // Vamos manter a estrutura original com Task.Run para compatibilidade e para demonstrar o cancelamento da Task.
                // A operação interna tcpClient.Connect() ainda é síncrona e não pode ser cancelada diretamente
                // por um CancellationToken enquanto está bloqueando. O CancellationToken aqui cancela a Task *antes* de iniciar,
                // ou a espera pela Task.
                var connectTask = Task.Run(async () => // Usamos async aqui para poder usar await dentro, se necessário
                {
                    try
                    {
                        // O CancellationToken pode ser verificado antes de iniciar a conexão
                        linkedCts.Token.ThrowIfCancellationRequested();

                        var ipEndPoint = NetworkAddress.TryCreateIpEndPoint(_serverIpAdressString, _serverPort);
                        // Para .NET Framework 4.7.2, TcpClient.Connect() é síncrono.
                        // Para .NET 6+, você pode usar tcpClient.ConnectAsync().
                        // Vamos usar a versão síncrona para compatibilidade com o seu código original e .NET 4.7.2.
                        await tcpClient.ConnectAsync(_serverIpAdressString, _serverPort).ConfigureAwait(false);
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

                #endregion

                var delayTask = Task.Delay(timeoutMs, linkedCts.Token);

                var completedTask = await Task.WhenAny(connectTask, delayTask)
                                                .ConfigureAwait(false);

                // Se a tarefa de delay completou primeiro, significa timeout
                if (completedTask == delayTask)
                {
                    // O timeout ocorreu. Precisamos cancelar a tarefa de conexão.
                    // Isso fará com que 'connectTask' lance uma OperationCanceledException
                    // se ela ainda não tiver terminado.
                    linkedCts.Cancel();

                    // É importante não esperar por connectTask aqui, pois ela pode estar bloqueada
                    // ou levar muito tempo para reagir ao cancelamento.
                    // Apenas lançamos a exceção de timeout.
                    // O TcpClient criado dentro de connectTask (se a conexão não foi estabelecida)
                    // será descartado na cláusula catch dentro do Task.Run.
                    tcpClient.Dispose(); // Descartar o cliente que foi criado mas não conectado a tempo.

                    // Se FecharAsync() lida com recursos externos, ele pode ser chamado aqui.
                    // await FecharAsync().ConfigureAwait(false); // Se FecharAsync() não depende do tcpClient

                    throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {_serverIpAdressString}:{_serverPort}");
                }
                else // A tarefa de conexão completou primeiro (ou falhou antes do timeout)
                {
                    // A conexão foi bem-sucedida ou falhou antes do timeout.
                    // Cancelamos o delayTask para que ele não continue rodando desnecessariamente.
                    linkedCts.Cancel();

                    // Agora, esperamos pela connectTask para obter o TcpClient ou para propagar qualquer exceção
                    // que possa ter ocorrido durante a conexão (ex: SocketException).
                    // Esta linha é crucial para obter o resultado ou a exceção real da conexão.
                    await connectTask.ConfigureAwait(false);
                    return tcpClient; // Retorna o TcpClient conectado
                }
            } // O CancellationTokenSource é descartado aqui
        }

        // Exemplo de uso
        public static async Task Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1"); // Ou IPAddress.Loopback
            int port = 12345; // Uma porta que provavelmente não terá um servidor escutando
            //string ipString = "127.0.0.1";

            var connector = new TcpConnector(ip, port);

            Console.WriteLine($"Tentando conectar a {ip.ToString()}:{port} com timeout de 2000ms...");
            try
            {
                // Para testar o timeout, use uma porta onde não há servidor.
                // Para testar sucesso, inicie um servidor simples na porta 12345.
                using (TcpClient client = await connector.ConnectWithTimeoutAsync(new CancellationTokenSource().Token, 2000))
                {
                    Console.WriteLine($"Conectado com sucesso a {client.Client.RemoteEndPoint}");
                    // Faça algo com o cliente
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Erro de Timeout: {ex.Message}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Erro de Socket: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }

            // Exemplo de conexão rápida (para não dar timeout)
            Console.WriteLine($"Tentando conectar a {ip.ToString()}:80 (HTTP) com timeout de 5000ms...");
            try
            {
                // Tentar conectar a uma porta comum que pode estar aberta (ex: HTTP)
                // Isso pode falhar se não houver um servidor HTTP na máquina local
                // ou se o firewall bloquear.
                var connector2 = new TcpConnector(ip, 80);
                using (TcpClient client = await connector2.ConnectWithTimeoutAsync(new CancellationTokenSource().Token, 5000))
                {
                    Console.WriteLine($"Conectado com sucesso a {client.Client.RemoteEndPoint}");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Erro de Timeout: {ex.Message}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Erro de Socket: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }
    }
}
