using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    using Etiquetas.Bibliotecas.Rede;
    using System;
    using System.Diagnostics.Metrics;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Net.Sockets;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.Intrinsics.X86;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class TcpConnector
    {
        protected EnderecoRede EnderecoDeRede { get; }

        protected int TamanhoBuffer { get; set; }
        protected int TimeoutMs { get; set; }

        public TcpConnector(EnderecoRede enderecoRede)
        {
            this.EnderecoDeRede = enderecoRede ?? throw new ArgumentNullException(nameof(enderecoRede));
        }

        public async Task<TcpClient> ConnectWithTimeoutAsync(CancellationToken cancellationTokenBruto, int tamanhoBuffer = 8192, int timeoutMs = Timeout.Infinite)
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

                // Se sua aplicação envia muitos pequenos pacotes e a latência é crítica(ex: jogos, telnet, sistemas de controle em tempo real):
                // defina NoDelay = true(desabilita Nagle).
                // Se sua aplicação envia dados em blocos e o throughput é mais importante que a latência mínima para cada byte(ex: transferência de arquivos grandes, streaming de vídeo):
                // defina NoDelay = false(habilita Nagle).
                tcpClient.NoDelay = true; // Desabilita o Nagle Algorithm

                this.TimeoutMs = timeoutMs;

                tcpClient.ReceiveTimeout = timeoutMs;
                tcpClient.SendTimeout = timeoutMs;

                tcpClient.ReceiveBufferSize = tamanhoBuffer;
                tcpClient.SendBufferSize = tamanhoBuffer;

                // Quando KeepAlive = true ? Quando você define KeepAlive como true, você está habilitando o envio de pacotes Keep-Alive.
                // Vantagens:
                // Detecção de Conexões Quebradas: Ajuda a detectar se o peer remoto ainda está ativo e acessível, mesmo que não haja tráfego de dados. Isso é crucial para conexões de longa duração que podem ficar ociosas por períodos prolongados.
                // Prevenção de Timeouts de NAT / Firewall: Muitos dispositivos de rede(como NATs e firewalls) têm timeouts para conexões ociosas.O Keep-Alive envia tráfego suficiente para manter a conexão "viva" e evitar que esses dispositivos a encerrem prematuramente.
                // Evita Conexões "Zumbis": Sem Keep-Alive, uma conexão pode parecer ativa para uma das partes, enquanto a outra parte(ou um dispositivo intermediário) já a encerrou.Isso pode levar a tentativas de envio de dados para uma conexão morta, resultando em erros e timeouts na aplicação.
                // Desvantagens:
                // Tráfego de Rede Adicional: Embora os pacotes Keep-Alive sejam pequenos, eles adicionam um pequeno volume de tráfego à rede, o que pode ser uma preocupação em redes com largura de banda extremamente limitada ou em cenários de pagamento por volume de dados.
                // Consumo de Recursos: Manter muitas conexões com Keep - Alive habilitado pode consumir um pouco mais de recursos no sistema operacional, embora geralmente seja insignificante para a maioria das aplicações.
                // A configuração new LingerOption(true, 0) deve ser usada com extrema cautela e apenas quando você tem certeza de que a perda de dados no fechamento é aceitável ou desejada(ex: forçar o fechamento de uma conexão travada).
                // Para a maioria das aplicações que exigem confiabilidade, o padrão(LingerOption(false, 0)) ou LingerOption(true, X) com um tempo de espera razoável(X > 0) é preferível para permitir que os dados pendentes sejam enviados.
                tcpClient.LingerState = new LingerOption(true, timeoutMs);

                // Quando Usar KeepAlive = true? É altamente recomendado para conexões TCP de longa duração que podem ter períodos de inatividade. Exemplos incluem:
                // Serviços de Chat / Mensageria: Para manter as conexões dos clientes ativas e detectar rapidamente se um cliente se desconectou.
                // Aplicações Cliente-Servidor de Longa Duração: Onde o cliente mantém uma conexão persistente com o servidor para receber atualizações ou enviar comandos esporadicamente.
                // Conexões de Banco de Dados Persistentes: Para garantir que a conexão com o banco de dados não seja encerrada por um firewall ou timeout de rede.
                // As configurações de tempo e número de tentativas para os pacotes Keep-Alive são geralmente controladas pelo sistema operacional e podem ser ajustadas em nível de sistema, ou em algumas plataformas, através de opções de Socket mais específicas(como SocketOptionName.TcpKeepAliveTime, TcpKeepAliveInterval, TcpKeepAliveRetryCount no.NET Core 3.1 + e.NET 5 +).No.NET Framework 4.7.2, você geralmente depende das configurações padrão do sistema operacional ou precisa usar P / Invoke para ajustar essas opções mais finamente.
                // tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true)
                // Altamente recomendado para conexões de longa duração que podem ficar ociosas, para detectar desconexões e evitar timeouts de rede.
                // Para conexões de curta duração que são abertas, usadas e fechadas rapidamente, pode não ser estritamente necessário, mas geralmente não causa problemas.
                // tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                // Task.Run é usado aqui para envolver a chamada assíncrona com o CancellationToken,
                // garantindo que a Task de conexão possa ser cancelada.
                // No entanto, ConnectAsync já aceita um CancellationToken diretamente no .NET 6+.
                // Se você estiver no .NET Framework 4.7.2, ConnectAsync não aceita CancellationToken.
                // Para .NET Framework 4.7.2, a abordagem original com Task.Run para a operação síncrona
                // é mais comum, mas ainda precisamos do cancelamento.

                // Vamos considerar a abordagem para .NET 6+ primeiro, que é mais limpa.
                // Se você estiver no .NET Framework 4.7.2, veja a alternativa abaixo.

                #region Abordagem para .NET 6+ (ConnectAsync com CancellationToken)
                // var connectTask = tcpClient.ConnectAsync(IpAddress, ServerPort, cts.Token);
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

                        var ip = EnderecoDeRede.ObtemEnderecoIP();
                        var porta = EnderecoDeRede.ObtemPorta();

                        // Para .NET Framework 4.7.2, TcpClient.Connect() é síncrono????????
                        // Para .NET 6+, você pode usar tcpClient.ConnectAsync().
                        // Vamos usar a versão síncrona para compatibilidade com o seu código original e .NET 4.7.2????????
                        await tcpClient.ConnectAsync(ip, porta).ConfigureAwait(false);

                        if (tcpClient.Client.ReceiveBufferSize != tamanhoBuffer)
                        {
                            tcpClient.Client.ReceiveBufferSize = tamanhoBuffer;
                        }

                        if (tcpClient.Client.SendBufferSize != tamanhoBuffer)
                        {
                            tcpClient.Client.SendBufferSize = tamanhoBuffer;
                        }

                        if (tcpClient.Client.ReceiveTimeout != timeoutMs)
                        {
                            tcpClient.Client.ReceiveTimeout = timeoutMs;
                        }

                        if (tcpClient.Client.SendTimeout != timeoutMs)
                        {
                            tcpClient.Client.ReceiveTimeout = timeoutMs;
                        }
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

                    throw new TimeoutException($"Timeout de {timeoutMs}ms ao conectar em {ServerIpAdressString}:{ServerPort}");
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

        public string ObtemEnderecoIPOuNomeHost()
        {
            return this.EnderecoDeRede.ObtemEnderecoIPOuNomeHost();
        }

        public int ObtemPorta()
        {
            return this.EnderecoDeRede.ObtemPorta();
        }

        public IPEndPoint ObtemIPEndPoint()
        {
            return this.EnderecoDeRede.ObtemEnderecoRedeIpEndPoint();
        }

        public string ObtemEnderecoRedeIpEndPoint()
        {
            return this.EnderecoDeRede.ObtemEnderecoIPOuNomeHost();
        }

        public IPAddress ObtemEnderecoIP()
        {
            return this.EnderecoDeRede.ObtemEnderecoIP();
        }

    }
}
