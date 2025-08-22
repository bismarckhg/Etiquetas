using Etiqueta.Bibliotecas.TaskCorePipe.Comunicacao;
using Etiqueta.Bibliotecas.TaskCorePipe.Interfaces;
using Etiqueta.Bibliotecas.TaskCorePipe.Modelos;
using System;
using System.Collections.Concurrent;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Servidor
{
    /// <summary>
    /// Servidor central que gerencia tarefas assíncronas e a comunicação com elas via pipe.
    /// </summary>
    public class ServidorPipeControlador : IPipeControlador
    {
        private readonly GerenciadorMutexGlobal _gerenciadorMutex;
        private readonly ConcurrentDictionary<string, object> _tarefasAtivas;
        private readonly ProtocoloMensagem _protocolo;

        private const string NOME_MUTEX_SERVIDOR = "TaskCorePipe_ServidorPipeControlador_Mutex";

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ServidorPipeControlador"/>.
        /// </summary>
        public ServidorPipeControlador()
        {
            _gerenciadorMutex = new GerenciadorMutexGlobal(NOME_MUTEX_SERVIDOR);
            _tarefasAtivas = new ConcurrentDictionary<string, object>();
            _protocolo = new ProtocoloMensagem(new SerializadorJson());
        }

        /// <summary>
        /// Inicia o servidor, garantindo que apenas uma instância esteja ativa, e prepara para operar.
        /// </summary>
        public Task IniciarAsync()
        {
            if (!_gerenciadorMutex.Adquirir(TimeSpan.FromSeconds(1)))
            {
                throw new InvalidOperationException("Uma instância do ServidorPipeControlador já está em execução.");
            }
            Console.WriteLine("Servidor Pipe Controlador iniciado.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Para o servidor, liberando o mutex global.
        /// </summary>
        public Task PararAsync()
        {
            _gerenciadorMutex.Liberar();
            Console.WriteLine("Servidor Pipe Controlador parado.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Envia um comando para uma tarefa específica através de seu pipe.
        /// </summary>
        /// <param name="nomePipe">O nome do pipe da tarefa de destino.</param>
        /// <param name="comando">O comando a ser enviado.</param>
        /// <returns>Um objeto de resposta da tarefa.</returns>
        public async Task<object> EnviarComandoAsync(string nomePipe, IComandoPipe comando)
        {
            using (var pipeClient = new NamedPipeClientStream(".", nomePipe, PipeDirection.InOut, PipeOptions.Asynchronous))
            {
                try
                {
                    await pipeClient.ConnectAsync(5000);

                    var mensagemEnvio = new MensagemPipe
                    {
                        IdOrigem = "ServidorControlador",
                        IdDestino = nomePipe,
                        Payload = comando,
                        TipoMensagem = Enums.TipoMensagem.Command
                    };

                    await _protocolo.EscreverMensagemAsync(pipeClient, mensagemEnvio);
                    var mensagemResposta = await _protocolo.LerMensagemAsync(pipeClient);

                    if (mensagemResposta?.Payload is RespostaPipe resposta)
                    {
                        return resposta;
                    }
                    return new RespostaPipe { CodigoResposta = Enums.CodigoResposta.InternalServerError, MensagemErro = "Resposta inválida ou nula recebida da tarefa." };
                }
                catch (TimeoutException)
                {
                    return new RespostaPipe { CodigoResposta = Enums.CodigoResposta.GatewayTimeout, MensagemErro = $"Não foi possível conectar ao pipe '{nomePipe}'. A operação atingiu o tempo limite." };
                }
                catch (Exception ex)
                {
                    return new RespostaPipe { CodigoResposta = Enums.CodigoResposta.InternalServerError, MensagemErro = $"Erro ao comunicar com o pipe '{nomePipe}': {ex.Message}" };
                }
            }
        }

        /// <summary>
        /// Libera os recursos utilizados pelo servidor, incluindo o mutex.
        /// </summary>
        public void Dispose()
        {
            PararAsync().Wait();
            _gerenciadorMutex?.Dispose();
        }
    }
}
