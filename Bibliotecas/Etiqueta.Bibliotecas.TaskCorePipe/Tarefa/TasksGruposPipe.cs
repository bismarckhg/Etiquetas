using Etiquetas.Bibliotecas.TaskCore;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using Etiqueta.Bibliotecas.TaskCorePipe.Enums;
using Etiqueta.Bibliotecas.TaskCorePipe.Interfaces;
using Etiqueta.Bibliotecas.TaskCorePipe.Modelos;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Tarefa
{
    /// <summary>
    /// Estende TasksGrupos com a capacidade de comunicação via pipe nomeado.
    /// Cada instância desta classe representa uma tarefa que pode ser controlada externamente.
    /// </summary>
    public class TasksGruposPipe : TasksGrupos
    {
        private readonly GerenciadorPipeTarefa _gerenciadorPipe;
        private readonly string _nomeUnicoPipe;
        private readonly Guid _idGrupo;
        private readonly Stopwatch _cronometro;
        private readonly CancellationTokenSource _ctsBruto; // Token para parada forçada (BREAK)

        /// <summary>
        /// Construtor que inicializa a tarefa com pipe.
        /// </summary>
        public TasksGruposPipe(string nomeUnicoPipe,
                               string nomeGrupo = null,
                               CancellationTokenManager tokenExterno = null,
                               bool useSingleThread = false,
                               int maxDegreeOfParallelism = 0)
            : base(nomeGrupo, tokenExterno, useSingleThread, maxDegreeOfParallelism)
        {
            _idGrupo = Guid.NewGuid();
            _cronometro = new Stopwatch();
            _ctsBruto = new CancellationTokenSource();
            _nomeUnicoPipe = nomeUnicoPipe;
            _gerenciadorPipe = new GerenciadorPipeTarefa(_nomeUnicoPipe);
            _gerenciadorPipe.ComandoRecebido += ProcessarComandoRecebidoAsync;
        }

        /// <summary>
        /// Inicia a execução das tarefas e também o listener do pipe e o cronômetro.
        /// </summary>
        public new void IniciarExecucao()
        {
            _cronometro.Start();
            _gerenciadorPipe.IniciarAsync();
            base.IniciarExecucao();
        }

        /// <summary>
        /// Sobrescreve o registro de token para incluir o cancelamento bruto (BREAK).
        /// </summary>
        protected override async Task<bool> RegistraCancellationTokenSourceTaskNoGroupCancellationTokenSource(CancellationTokenManager cancelToken)
        {
            // Mantém o comportamento original (ligação com o STOP suave)
            var registroSuaveOk = await base.RegistraCancellationTokenSourceTaskNoGroupCancellationTokenSource(cancelToken);

            // Adiciona a ligação com o token de parada forçada (BREAK)
            var registroBruto = _ctsBruto.Token.Register(() => {
                // Aqui poderíamos adicionar lógica para indicar que foi um cancelamento forçado.
                // Por exemplo, setando um status específico antes de cancelar.
                cancelToken.Cancel();
            });

            return registroSuaveOk;
        }

        private StatusTarefa MapearEstadoParaStatusTarefa(Etiquetas.Bibliotecas.TaskCore.TaskState estado)
        {
            switch (estado)
            {
                case Etiquetas.Bibliotecas.TaskCore.TaskState.AguardandoInicio:
                    return StatusTarefa.Inicializando;
                case Etiquetas.Bibliotecas.TaskCore.TaskState.EmProcessamento:
                    return StatusTarefa.Executando;
                case Etiquetas.Bibliotecas.TaskCore.TaskState.Finalizada:
                    return StatusTarefa.Finalizada;
                case Etiquetas.Bibliotecas.TaskCore.TaskState.Cancelada:
                    return StatusTarefa.Cancelada;
                case Etiquetas.Bibliotecas.TaskCore.TaskState.ComErro:
                case Etiquetas.Bibliotecas.TaskCore.TaskState.Timeout:
                    return StatusTarefa.Erro;
                default:
                    return StatusTarefa.Erro;
            }
        }

        /// <summary>
        /// Processa os comandos recebidos do pipe.
        /// </summary>
        private Task<object> ProcessarComandoRecebidoAsync(IComandoPipe comando)
        {
            var comandoPipe = comando as ComandoPipe;
            if (comandoPipe == null)
            {
                return Task.FromResult<object>(new RespostaPipe { CodigoResposta = CodigoResposta.BadRequest, MensagemErro = "Payload do comando inválido." });
            }

            switch (comandoPipe.Comando)
            {
                case TipoComando.PING:
                    return Task.FromResult<object>(new RespostaPipe { CodigoResposta = CodigoResposta.Success, Dados = "PONG" });

                case TipoComando.STATUS:
                    var status = new StatusTarefaPipe
                    {
                        IdTarefa = _idGrupo,
                        NomeTarefa = this.NomeGrupo,
                        Status = MapearEstadoParaStatusTarefa(this.ObterEstadoTask(0)),
                        TempoDeExecucao = _cronometro.Elapsed
                    };
                    return Task.FromResult<object>(new RespostaPipe { CodigoResposta = CodigoResposta.Success, Dados = status });

                case TipoComando.STOP:
                    this.CancelarGrupo(); // Dispara o cancelamento suave
                    return Task.FromResult<object>(new RespostaPipe { CodigoResposta = CodigoResposta.Accepted, Dados = "Comando STOP recebido. Iniciando parada suave." });

                case TipoComando.BREAK:
                    _ctsBruto.Cancel(); // Dispara o cancelamento bruto/forçado
                    return Task.FromResult<object>(new RespostaPipe { CodigoResposta = CodigoResposta.Accepted, Dados = "Comando BREAK recebido. Iniciando parada forçada." });

                default:
                    return Task.FromResult<object>(new RespostaPipe { CodigoResposta = CodigoResposta.BadRequest, MensagemErro = "Comando desconhecido." });
            }
        }

        /// <summary>
        /// Sobrescreve o método Dispose para garantir que os recursos do pipe e do cronômetro sejam liberados.
        /// </summary>
        public override void Dispose()
        {
            _cronometro.Stop();
            _ctsBruto?.Dispose();
            _gerenciadorPipe?.Dispose();
            base.Dispose();
        }
    }
}
