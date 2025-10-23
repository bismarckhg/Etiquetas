using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TaskCore.Interfaces
{
    public abstract class ITasksGrupos : IDisposable
    {
        public delegate Task EventosTasksAsync(ITaskParametros parametros);

        /// <summary>
        /// Indica se o grupo de tasks deve ser executado em uma única thread.
        /// </summary>
        public abstract bool UseSingleThread { get; }

        /// <summary>
        /// Parametro define o agendador de tarefas(TaskScheduler) associado ao grupo de tasks.
        /// Este agendador é utilizado para manipula o trabalho de nível baixo de enfileirar tarefas
        /// em threads.
        /// </summary>
        public abstract TaskScheduler SchedulerTask { get; }

        /// <summary>
        /// Evento disparado quando uma task é cancelada.
        /// <see cref="GrupoTasks.HandleException(int,string,Exception)"/> dispara este evento em OperationCanceledException.
        /// </summary>
        public abstract event Func<int, string, string, Exception, Task> TratamentoCancelamentoGrupo;
        //public abstract event Action<int, string, string, Exception> TratamentoCancelamentoGrupo;
        //public abstract event EventosTasksAsync TratamentoCancelamentoGrupo;

        /// <summary>
        /// Evento disparado quando uma task atinge timeout.
        /// <see cref="GrupoTasks.HandleException(int,string,Exception)"/> dispara este evento em TimeoutException.
        /// </summary>
        public abstract event Func<int, string, string, Exception, Task> TratamentoTimeOutGrupo;
        //public abstract event Action<int, string, string, Exception> TratamentoTimeOutGrupo;
        //public abstract event EventosTasksAsync TratamentoTimeOutGrupo;

        /// <summary>
        /// Evento disparado quando ocorre erro em uma task.
        /// <see cref="GrupoTasks.HandleException(int,string,Exception)"/> dispara este evento para exceções genéricas.
        /// </summary>
        //public abstract event Action<int, string, string, Exception> TratamentoErroGrupo;
        //public abstract event EventosTasksAsync TratamentoErroGrupo;
        public abstract event Func<int, string, string, Exception, Task> TratamentoErroGrupo;

        /// <summary>
        /// Evento disparado ao término do fluxo de execução de todas as tasks.
        /// Disparado em <see cref="IGrupoTasks.SubscribeProcessos()"/> no OnCompleted.
        /// </summary>
        public abstract event Action Finalizacao;
        /// <summary>
        /// Adiciona uma nova task ao grupo, validando ID, função, parâmetros e nome.
        /// A implementação está em <c>GrupoTasks.AdicionarTask</c>.
        /// </summary>
        public abstract Task AdicionarTask(int id,
                                           Func<ITaskParametros, Task<ITaskReturnValue>> funcao,
                                           ITaskParametros parametros,
                                           string nomeTask = null);

        /// <summary>
        /// Inicia a execução de todas as tasks adicionadas, montando os fluxos Rx.
        /// Chama internamente <see cref="MontarFluxoProcessos()"/> e <see cref="MontarFluxoResultados(System.IObservable{System.Collections.Generic.KeyValuePair{int, System.Threading.Tasks.Task{ITaskReturnValue}}})"/>.
        /// </summary>
        public abstract void IniciarExecucao();

        /// <summary>
        /// Cancela todas as tasks do grupo.
        /// Dispara <see cref="TratamentoCancelamentoGrupo"/> para cada task se implementado.
        /// </summary>
        public abstract void CancelamentoGrupo();

        /// <summary>
        /// Cancela todas as tasks do grupo.
        /// Dispara <see cref="TratamentoCancelamentoGrupo"/> para cada task se implementado.
        /// </summary>
        public abstract void CancelamentoBruscoGrupo();

        /// <summary>
        /// Cancela uma task específica pelo nome, sinalizando seu token individual.
        /// Referenciado em <see cref="ITasksGrupos.CancelamentoTaskPorNome(string)"/>.
        /// </summary>
        public abstract void CancelamentoTaskPorNome(string nomeTask);

        /// <summary>
        /// Cancela uma task específica pelo nome, sinalizando seu token individual.
        /// Referenciado em <see cref="ITasksGrupos.CancelamentoBruscoTaskPorNome(string)"/>.
        /// </summary>
        public abstract void CancelamentoBrutoTaskPorNome(string nomeTask);

        /// <summary>
        /// Cancela uma task específica pelo nome, sinalizando seu token individual.
        /// Referenciado em <see cref="ITasksGrupos.CancelamentoTaskPorNome(string)"/>.
        /// </summary>
        public abstract void CancelamentoTaskPorId(int id);

        /// <summary>
        /// Cancela uma task específica pelo nome, sinalizando seu token individual.
        /// Referenciado em <see cref="ITasksGrupos.CancelamentoBrutoTaskPorNome(string)"/>.
        /// </summary>
        public abstract void CancelamentoBrutoTaskPorId(int id);

        /// Obtém o estado corrente de uma task (AguardandoInicio, EmProcessamento, etc.).
        /// Usa <see cref="UpdateTaskState(int, TaskState)"/> internamente.
        public abstract TaskState ObterEstadoTask(int id);

        /// <summary>
        /// Aguarda a conclusão de todas as tasks em execução.
        /// Implementado via Task.WhenAll em <see cref="GrupoTasks.AguardarTodasTasksAsync()"/>.
        /// </summary>
        public abstract Task AguardaTodasTasksAsync(bool throwOnAnyError = false);

        /// <summary>
        /// Aguarda e retorna o resultado de uma task específica pelo nome.
        /// Baseado em <see cref="GrupoTasks.AguardarTaskPorNomeAsync(string)"/>.
        /// </summary>
        public abstract Task AguardaTaskPorNomeAsync(string nomeTask);

        /// <summary>
        /// <summary>
        /// Aguarda e retorna o resultado de uma task específica pelo nome.
        /// Baseado em <see cref="GrupoTasks.AguardarTaskPorNomeAsync(string[])"/>.
        /// </summary>
        public abstract Task AguardaTaskPorNomeAsync(string[] nomesTask);

        /// <summary>
        /// Aguarda a conclusão de todas as tasks no grupo e retorna um dicionário
        /// contendo o ID de cada task como chave e o respectivo resultado como valor.
        /// 
        /// Este método garante que todas as tasks sejam concluídas antes de retornar,
        /// capturando internamente quaisquer exceções que possam ocorrer durante a execução.
        /// 
        /// Exceções:
        /// <exception cref="InvalidOperationException">Lançada se a execução ainda não foi iniciada.</exception>
        /// <exception cref="AggregateException">Lançada se ocorrerem falhas durante a execução das tasks.</exception>
        /// 
        /// Uso:
        /// Este método é útil para obter os resultados finais das tasks após a execução completa do grupo,
        /// permitindo que os resultados sejam acessados por ID.
        /// </summary>
        public abstract Task<IReadOnlyDictionary<int, ITaskReturnValue>> ObterTodosResultadosPorIdAsync();

        /// <summary>
        /// Obtém os resultados das tasks em execução no grupo, mapeados pelo nome das tasks.
        /// 
        /// Este método aguarda a conclusão de todas as tasks no grupo antes de retornar os resultados.
        /// Ele utiliza o dicionário interno de tasks em execução para criar um mapeamento entre
        /// os nomes das tasks e seus respectivos valores de retorno.
        /// 
        /// Exceções:
        /// <exception cref="InvalidOperationException">Lançada se a execução ainda não foi iniciada.</exception>
        /// <exception cref="AggregateException">Lançada se ocorrerem falhas durante a execução das tasks.</exception>
        /// 
        /// Uso:
        /// Este método é útil para obter os resultados finais das tasks após a execução completa do grupo,
        /// permitindo que os resultados sejam acessados por nome.
        /// </summary>
        /// <returns>Um dicionário somente leitura contendo os nomes das tasks como chave e os valores de retorno como valor.</returns>
        public abstract Task<IReadOnlyDictionary<string, ITaskReturnValue>> ObterResultadosPorNomeAsync();

        /// <summary>
        /// Aguarda e retorna o resultado(ITaskReturnValue) da task cujo nome foi informado.
        /// 
        /// Este método utiliza o nome da task para localizar e aguardar sua conclusão.
        /// Ele chama internamente o método <see cref="ObterResultadosPelosNomesAsync(string[])"/>
        /// para obter o resultado da task especificada.
        /// 
        /// Exceções:
        /// <exception cref="KeyNotFoundException">Lançada se a task com o nome especificado não for encontrada nos resultados.</exception>
        /// 
        /// Uso:
        /// Este método é útil para obter o resultado de uma task específica após sua execução,
        /// garantindo que a task tenha sido concluída antes de acessar seu valor de retorno.
        /// </summary>
        /// <param name="nomeTask">O nome da task cujo resultado se deseja obter.</param>
        /// <returns>O valor de retorno da task, como <see cref="ITaskReturnValue"/>.</returns>
        public abstract Task<ITaskReturnValue> ObterResultadoPeloNomeAsync(string nomeTask);

        /// <summary>
        /// Aguarda apenas as tasks cujos nomes estão em <paramref name="nomesTasks"/>,
        /// valida que estão registradas e em execução, e devolve um dicionário
        /// com o nome da task apontando para o seu ITaskReturnValue.
        /// </summary>
        /// <param name="nomesTasks">Array de nomes das Tasks a serem aguardadas.</param>
        /// <returns>
        /// Dicionário (nome → ITaskReturnValue) contendo o resultado de cada subtask.  
        /// Se alguma falhar, a exceção virá na Task.Result (lançada ao acessar).
        /// </returns>
        public abstract Task<IReadOnlyDictionary<string, ITaskReturnValue>> ObterResultadosPelosNomesAsync(string[] nomesTasks);

        /// Libera recursos internos, disposições de subscriptions e tokens.
        /// Implementado em <see cref="GrupoTasks.Dispose()"/>.
        /// </summary>
        public abstract void Dispose();

        // Pontos de extensão protegidos

        /// <summary>  
        /// Obtém a fonte de conclusão de tarefa para o pool de tasks.  
        ///  
        /// Esta propriedade é usada para sinalizar a conclusão de todas as tasks no grupo.  
        /// Quando todas as tasks são registradas e concluídas, a TaskCompletionSource é concluída,  
        /// permitindo que os assinantes saibam que o grupo de tasks foi finalizado.  
        ///  
        /// Exceções:  
        /// <exception cref="InvalidOperationException">Lançada se a execução já foi iniciada anteriormente.</exception>  
        /// </summary>  
        protected abstract TaskCompletionSource<bool> TasksPoolGroupCompleted { get; set; }

        protected abstract TaskCompletionSource<bool> TasksPoolArrayCompleted { get; set; }

        /// <summary>
        /// Nome descritivo para o grupo de tasks.
        /// <see cref="GrupoTasks.NomeGrupo"/> implementa a leitura interna deste valor.
        /// </summary>
        protected abstract string NomeGrupo { get; }

        /// <summary>
        /// Máximo de tasks que podem rodar em paralelo.
        /// <see cref="GrupoTasks.MaxDegreeOfParallelism"/> define o comportamento do Merge no fluxo de processos.
        /// </summary>
        protected abstract int MaxDegreeOfParallelism { get; }

        /// <summary>
        /// Define como montar o fluxo de processos (ID, Task) sem aguardar conclusão.
        /// Ligação: chamado por <see cref="IGrupoTasks.IniciarExecucao()"/> para criar o Observable de processos.
        /// </summary>
        protected abstract IObservable<KeyValuePair<int, Task<ITaskReturnValue>>> MontarFluxoProcessos();

        /// <summary>
        /// Define como transformar cada Task em ITaskReturnValue e encapsular eventos.
        /// Fluxo de resultados materializados (OnNext/OnError).
        /// Ligação: usado após <see cref="IGrupoTasks.MontarFluxoProcessos()"/> em <see cref="IGrupoTasks.IniciarExecucao()"/>.
        /// </summary>
        protected abstract IObservable<KeyValuePair<int, ITaskReturnValue>> MontarFluxoResultados(
            IObservable<KeyValuePair<int, Task<ITaskReturnValue>>> processos);

        /// <summary>
        /// Inscreve no fluxo de processos para gerenciar OnNext, OnError e OnCompleted.
        /// Ligação: retornado em <see cref="IGrupoTasks.IniciarExecucao()"/> e armazena a subscription.
        /// </summary>
        protected abstract IDisposable SubscribeProcessos();

        /// <summary>
        /// Inscreve no fluxo de resultados para tratar dados e erros de cada task.
        /// Ligação: retornado em <see cref="IGrupoTasks.IniciarExecucao()"/> e armazena a subscription.
        /// </summary>
        protected abstract IDisposable SubscribeResultados();

        /// <summary>
        /// Trata exceções de uma task, atualiza estado e dispara eventos.
        /// Ligação: invocado em <see cref="IGrupoTasks.SubscribeResultados()"/> no NotificationKind.OnError.
        /// </summary>
        protected abstract Task HandleExceptionAsync(int id, string nomeTask, Exception exception);

        /// <summary>
        /// Atualiza internamente o estado de uma task.
        /// Ligação: usado em início de execução, OnNext e OnError de <see cref="IGrupoTasks.SubscribeResultados()"/>.
        /// </summary>
        protected abstract void UpdateTaskState(int id, TaskState state);

        /// <summary>
        /// Cria a entry de processo para o ID informado, incluindo token e Task.
        /// Ligação: chamado dentro de <see cref="IGrupoTasks.MontarFluxoProcessos()"/> para inicializar cada Task.
        /// </summary>
        protected abstract KeyValuePair<int, Task<ITaskReturnValue>> CriarProcessEntry(int id);

        /// <summary>
        /// Cria a entry de forma async de processo para o ID informado, incluindo token e Task.
        /// Ligação: chamado dentro de <see cref="IGrupoTasks.MontarFluxoProcessos()"/> para inicializar cada Task.
        /// </summary>
        protected abstract Task<KeyValuePair<int, Task<ITaskReturnValue>>> CriarProcessEntryAsync(int id);


        #region "Adiministra Cancel Token"

        protected abstract Task<bool> AdicionaCancellationTokenComTaskId(int id, CancellationToken individualToken);
        protected abstract Task<bool> ArmazenaCancellationTokenPeloTaskId(int id, CancellationToken cancelToken);

        protected abstract Task<bool> RelacionaTaskComCancellationTokenAsync(int id, CancellationToken cancelToken);

        protected abstract Task<bool> AdicionaCancellationTokenBreakComTaskId(int id, CancellationToken individualToken);
        protected abstract Task<bool> ArmazenaCancellationTokenBreakPeloTaskId(int id, CancellationToken cancelToken);

        protected abstract Task<bool> RelacionaTaskComCancellationTokenBreakAsync(int id, CancellationToken cancelToken);

        // protected abstract Task<bool> RegistraCancellationTokenTaskNoGroupCancellationToken(CancellationToken cancelToken);

        #endregion

    }
}

