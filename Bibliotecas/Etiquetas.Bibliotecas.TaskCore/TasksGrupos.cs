using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Etiquetas.Bibliotecas.TaskCore.Interfaces.ITasksGrupos;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Reflection;

namespace Etiquetas.Bibliotecas.TaskCore
{
    /// <summary>
    /// Gerencia e executa grupos de tarefas assíncronas, implementando a interface <see cref="ITasksGrupos"/>.
    /// Utiliza Task-based Asynchrony Pattern (TAP) em conjunto com Reactive Extensions (Rx) para
    /// orquestrar fluxos de execução, controlar paralelismo, cancelamento e tratamento de erros.
    ///
    /// Esta classe oferece flexibilidade na execução de múltiplas operações, mas o uso de Reactive Extensions
    /// implica uma curva de aprendizado. Recomenda-se familiaridade com os conceitos do Rx para pleno entendimento
    /// dos fluxos internos (<see cref="MontarFluxoProcessos"/>, <see cref="MontarFluxoResultados"/>).
    /// </summary>

    public class TasksGrupos : ITasksGrupos, IDisposable
    {
        #region "Constantes Internas"
        /// <summary>
        /// Número máximo de tentativas para registrar CancellationToken no dicionário.
        /// </summary>
        private const int MaxTokenRegistrationAttempts = 10;

        /// <summary>
        /// Delay em milissegundos entre tentativas de registro de cancellationNovo.
        /// </summary>
        private const int TokenRegistrationDelayMs = 10;

        /// <summary>
        /// Padrão para nome de grupo, caso não seja fornecido.
        /// </summary>
        private static readonly string DefaultGroupNamePattern = "GrupoTasks_{0:yyyyMMddHHmmssfff}";
        #endregion

        #region "Propriedades Públicas"

        /// <summary>  
        /// Define se o grupo de tasks deve ser executado em um único thread.  
        /// Quando definido como true, as tasks serão agendadas em um scheduler dedicado  
        /// para execução sequencial em um único thread.  
        /// Caso contrário, o agendamento será feito no scheduler padrão.  
        /// </summary> 
        public override bool UseSingleThread { get; }

        /// <summary>
        /// Scheduler utilizado para agendamento das tasks.
        /// Quando <see cref="UseSingleThread"/> é true, utiliza um scheduler dedicado para execução sequencial.
        /// Caso contrário, utiliza o scheduler padrão.
        /// </summary>
        public override TaskScheduler SchedulerTask { get; }

        /// <summary>
        /// Lista somente leitura contendo as últimas exceções capturadas durante a execução das tasks do grupo.
        /// 
        /// Esta propriedade é atualizada após a execução de todas as tasks, armazenando as exceções
        /// que ocorreram durante o processamento. Caso nenhuma exceção tenha ocorrido, a lista estará vazia.
        /// 
        /// Uso:
        /// - Útil para análise de falhas e depuração.
        /// - Não relança as exceções automaticamente, permitindo que o chamador as trate conforme necessário.
        /// </summary>
        public IReadOnlyList<Exception> UltimasExceptions { get; private set; } = Enumerable.Empty<Exception>().ToList();

        /// <summary>
        /// Exceções capturadas por ID na última chamada de AguardaTaskPorNomeAsync.
        /// </summary>
        public IReadOnlyDictionary<int, Exception> UltimasExceptionsPorId { get; private set; } = new Dictionary<int, Exception>();

        /// <summary>
        /// Informa o maior id inserido para uma Task.
        /// </summary>
        public int MaiorIdTasks { get; private set; }

        #endregion

        #region "Propriedades Protegidas"

        /// <summary>
        /// Source que indica o término do registro de todas as tasks.
        /// </summary>
        protected override TaskCompletionSource<bool> TasksPoolGroupCompleted { get; set; }

        /// <summary>
        /// Source que indica o término do registro de Array de algumas tasks.
        /// </summary>
        protected override TaskCompletionSource<bool> TasksPoolArrayCompleted { get; set; }

        /// <summary>
        /// Nome descritivo para o grupo de tasks.
        /// <see cref="GrupoTasks.NomeGrupo"/> implementa a leitura interna deste valor.
        /// </summary>
        protected override string NomeGrupo { get; }

        /// <summary>
        /// Máximo de tasks que podem rodar em paralelo.
        /// <see cref="GrupoTasks.MaxDegreeOfParallelism"/> define o comportamento do Merge no fluxo de processos.
        /// </summary>
        protected override int MaxDegreeOfParallelism { get; }

        #endregion

        #region "Campos Privados"

        private int TasksCriadas;
        private int TasksRegistradas;

        // Campos internos
        /// <summary>Token de cancelamento do grupo.</summary>
        protected readonly CancellationTokenSource CtsGrupo;

        // Campos internos
        /// <summary>Token de cancelamento do grupo por Throw.</summary>
        protected readonly CancellationTokenSource CtsGrupoThrow;

        /// <summary>
        /// 
        /// </summary>
        private readonly object LockRegistroTask = new object();

        /// <summary>  
        /// Indica se o grupo de tasks está em execução.  
        /// Quando verdadeiro, significa que as tasks foram iniciadas e estão sendo processadas.  
        /// </summary>  
        private bool Executando;

        #endregion

        #region "Dicionarios"

        /// <summary>Funcões(Com metodos com parametros e retonos, para criação das Tasks.</summary>
        private readonly ConcurrentDictionary<int, Func<ITaskParametros, Task<ITaskReturnValue>>> Funcoes = new ConcurrentDictionary<int, Func<ITaskParametros, Task<ITaskReturnValue>>>();
        /// <summary>Controle de mutex Unico por servicos./// </summary>
        private readonly ConcurrentDictionary<int, Mutex> ServicoMutex = new ConcurrentDictionary<int, Mutex>();
        /// <summary>Parâmetros de cada task.</summary>
        private readonly ConcurrentDictionary<int, ITaskParametros> ParametrosDict = new ConcurrentDictionary<int, ITaskParametros>();
        /// <summary>Nome da task para os ID.</summary>
        private readonly ConcurrentDictionary<string, int> NomeParaId = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        /// <summary>ID para o Nome da task.</summary>
        private readonly ConcurrentDictionary<int, string> IdParaNome = new ConcurrentDictionary<int, string>();
        /// <summary>Tasks em execução.</summary>
        private readonly ConcurrentDictionary<int, Task<ITaskReturnValue>> ExecutandoTasks = new ConcurrentDictionary<int, Task<ITaskReturnValue>>();
        /// <summary>Resultados das tasks processadas.</summary>
        private readonly ConcurrentDictionary<int, ITaskReturnValue> TaskResults = new ConcurrentDictionary<int, ITaskReturnValue>();
        /// <summary>Estados das tasks.</summary>
        private readonly ConcurrentDictionary<int, TaskState> Situacoes = new ConcurrentDictionary<int, TaskState>();

        private readonly ConcurrentDictionary<int, CancellationTokenSource> TaskIdComCancellationTokenSource = new ConcurrentDictionary<int, CancellationTokenSource>();

        private readonly ConcurrentDictionary<int, bool> _erroDisparado = new ConcurrentDictionary<int, bool>();

        /// <summary>Dicionário que armazena exceções encontradas ao tentar acessar o resultado de tasks falhadas.</summary>
        private readonly ConcurrentDictionary<int, Exception> _errosAoAcessarResultado = new ConcurrentDictionary<int, Exception>();

        // Dicionário para armazenar TaskCompletionSource para conjuntos de nomes de tasks aguardados
        private readonly Dictionary<string, TaskCompletionSource<bool>> _registroTasksSources = new Dictionary<string, TaskCompletionSource<bool>>();

        /// <summary>
        /// Expõe um dicionário somente leitura contendo os IDs das tasks que falharam
        /// e cuja tentativa de acesso ao resultado foi feita através dos métodos ObterResultado*,
        /// mapeando para a exceção original que causou a falha.
        /// Nota: Retorna uma cópia para compatibilidade com .NET 4.5.
        /// </summary>
        public IReadOnlyDictionary<int, Exception> ErrosAoAcessarResultado
        {
            get
            {
                // Cria uma cópia ReadOnlyDictionary para compatibilidade com .NET 4.5
                // ConcurrentDictionary não implementa IReadOnlyDictionary diretamente nessa versão.
                return new System.Collections.ObjectModel.ReadOnlyDictionary<int, Exception>(_errosAoAcessarResultado);
            }
        }

        #endregion

        #region "Fluxos Processos e Resultados"

        private Subject<KeyValuePair<int, ITaskReturnValue>> ResultadosSubject;

        /// <summary>
        /// Fluxo de processos (OnNext) para execução paralela.
        /// </summary>
        private IObservable<KeyValuePair<int, Task<ITaskReturnValue>>> ProcessosStream;

        /// <summary>
        /// 
        /// </summary>
        private IObservable<KeyValuePair<int, ITaskReturnValue>> ResultadosStream;

        /// <summary>
        /// 
        /// </summary>
        private IDisposable ProcessosSubscription;

        /// <summary>
        /// 
        /// </summary>
        private IDisposable ResultadosSubscription;

        #endregion

        /// <summary>
        /// Construtor que inicializa o grupo de tasks.
        /// </summary>
        /// <param name="nomeGrupo">Nome descritivo do grupo (opcional).</param>
        /// <param name="tokenExterno">Token externo para cancelamento (opcional).</param>
        public TasksGrupos(string nomeGrupo = null,
                   CancellationToken tokenExterno = default,
                   CancellationToken tokenBreak = default,
                   bool useSingleThread = false,
                   int maxDegreeOfParallelism = 0)
        {
            this.ResultadosSubject = new Subject<KeyValuePair<int, ITaskReturnValue>>();
            this.TasksCriadas = 0;
            this.TasksRegistradas = 0;
            this.TasksPoolGroupCompleted = new TaskCompletionSource<bool>();

            this.NomeGrupo = !EhStringNuloVazioComEspacosBranco.Execute(nomeGrupo)
                ? nomeGrupo
                : $"GrupoTasks_{DateTime.UtcNow:yyyyMMddHHmmssfff}";

            this.CtsGrupo = new CancellationTokenSource();
            if (tokenExterno != default)
            {
                // Linka o cancellationNovo externo ao cancellationNovo do grupo
                this.CtsGrupo = CancellationTokenSource.CreateLinkedTokenSource(tokenExterno);
            }

            this.CtsGrupoThrow = new CancellationTokenSource();
            if (tokenBreak != default)
            {
                // Linka o cancellationNovo externo ao cancellationNovo do grupo
                this.CtsGrupoThrow = CancellationTokenSource.CreateLinkedTokenSource(tokenBreak);
            }


            this.UseSingleThread = useSingleThread;
            this.SchedulerTask = UseSingleThread
                       ? new SingleThreadTaskScheduler()
                       : TaskScheduler.Default;

            MaxDegreeOfParallelism = maxDegreeOfParallelism > 0 ? maxDegreeOfParallelism : HardwareInfo.ProcessorCount;
        }

        #region "eventos de Tasks"

        /// <summary>
        /// Evento disparado quando uma task é cancelada.
        /// <see cref="GrupoTasks.HandleException(int,string,Exception)"/> dispara este evento em OperationCanceledException.
        /// </summary>
        public override event Func<int, string, string, Exception, Task> TratamentoCancelamentoGrupo;
        //public override event EventosTasksAsync TratamentoCancelamentoGrupo;

        /// <summary>
        /// Evento disparado quando uma task atinge timeout.
        /// <see cref="GrupoTasks.HandleException(int,string,Exception)"/> dispara este evento em TimeoutException.
        /// </summary>
        public override event Func<int, string, string, Exception, Task> TratamentoTimeOutGrupo;
        //public override event EventosTasksAsync TratamentoTimeOutGrupo;

        /// <summary>
        /// Evento disparado quando ocorre erro em uma task.
        /// <see cref="GrupoTasks.HandleException(int,string,Exception)"/> dispara este evento para exceções genéricas.
        /// </summary>
        //public override event Action<int, string, string, Exception> TratamentoErroGrupo;
        //public override event EventosTasksAsync TratamentoErroGrupo;
        public override event Func<int, string, string, Exception, Task> TratamentoErroGrupo;

        /// <summary>
        /// Evento disparado ao término do fluxo de execução de todas as tasks.
        /// Disparado em <see cref="IGrupoTasks.SubscribeProcessos()"/> no OnCompleted.
        /// </summary>
        public override event Action Finalizacao;
        //public override event EventosTasksAsync Finalizacao;

        #endregion

        #region "Metodos Publicos"

        /// <summary>
        /// Adiciona uma nova task ao grupo, e garante que a tarefa seja registrada corretamente, com validações rigorosas para evitar conflitos de:
        /// IDs ou função, parâmetros e nome. Para assegurar que os dados necessários estejam presentes no Start da Task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="funcao"></param>
        /// <param name="parametros"></param>
        /// <param name="nomeTask"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public override async Task AdicionarTask(int id,
                                    Func<ITaskParametros, Task<ITaskReturnValue>> funcao,
                                    ITaskParametros parametros,
                                    string nomeTask = null)
        {
            if (Executando)
                throw new InvalidOperationException("Não é possível adicionar tasks após iniciar execução.");
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (funcao == null) throw new ArgumentNullException(nameof(funcao));
            if (parametros == null) throw new ArgumentNullException(nameof(parametros));

            if (!Funcoes.TryAdd(id, funcao))
            {
                throw new ArgumentException($"Função já existe para ID {id}.", nameof(id));
            }

            var tarefaNome = !EhStringNuloVazioComEspacosBranco.Execute(nomeTask)
                ? nomeTask : $"TaskId{id}";

            if (!NomeParaId.TryAdd(tarefaNome, id))
            {
                throw new ArgumentException($"Já existe uma task com o nome '{tarefaNome}'.", nameof(nomeTask));
            }

            if (!IdParaNome.TryAdd(id, tarefaNome))
            {
                throw new ArgumentException($"Já existe uma task com o ID '{id}'.");
            }

            Etiquetas.Bibliotecas.TaskCore.TaskState statusTask = Situacoes.TryGetValue(id, out TaskState status) ? status : TaskState.AguardandoInicio;

            parametros.ArmazenaIdTask(id);
            parametros.ArmazenaTasksGrupo(this);
            parametros.ArmazenaNomeTask(tarefaNome);
            parametros.ArmazenaStatusTask(statusTask);
            var cancelToken = parametros.RetornoCancellationToken;
            var cancelTokenBreak = parametros.RetornoCancellationTokenBreak;

            if (!await AdicionaCancellationTokenComTaskId(id, cancelToken).ConfigureAwait(false))
            {
                throw new InvalidOperationException($"Não foi possível adicionar CancellationTokenSource para a Task com ID {id}.");
            }

            //if (!await RegistraCancellationTokenTaskNoGroupCancellationToken(cancelToken).ConfigureAwait(false))
            //{
            //    throw new InvalidOperationException($"Não foi possível adicionar CancellationTokenManager para o TasksGrupos {NomeGrupo} da Task com ID {id}.");
            //}

            if (!ParametrosDict.TryAdd(id, parametros))
            {
                throw new ArgumentException($"Não foi possivel armazenar Parametros para ID {id}.", nameof(id));
            }

            // Situacoes[id] = TaskState.AguardandoInicio;
            Situacoes.AddOrUpdate(id, TaskState.AguardandoInicio, (key, oldValue) => TaskState.AguardandoInicio);

            // Incrementa o contador de tasks criadas
            Interlocked.Increment(ref TasksCriadas);
        }

        /// <summary>  
        /// Inicia a execução de todas as tasks adicionadas ao grupo.  
        ///  
        /// Este método monta os fluxos de processos e resultados utilizando Reactive Extensions (Rx).  
        /// Ele chama internamente os métodos:
        /// <see cref="MontarFluxoProcessos"/>
        /// e
        /// <see cref="MontarFluxoResultados(IObservable{KeyValuePair{int, Task{ITaskReturnValue}}})"/>  
        /// para configurar os fluxos de execução paralela e de resultados.  
        ///  
        /// Após a configuração, os fluxos são assinados para iniciar a execução das tasks e tratar os resultados.  
        ///  
        /// Exceções:  
        /// <exception cref="InvalidOperationException">Lançada se a execução já foi iniciada anteriormente.</exception>  
        /// </summary>  
        /// <exception cref="InvalidOperationException"></exception>  
        public override void IniciarExecucao()
        {
            if (Executando)
                throw new InvalidOperationException("Execução do grupo já iniciada.");

            if (!Funcoes.Any())
                throw new InvalidOperationException("Nenhuma tarefa registrada para execução.");


            var pendentes = Situacoes.Where(kvp => kvp.Value != TaskState.AguardandoInicio)
                                     .Select(kvp => kvp.Key).ToList();
            if (pendentes.Any())
                throw new InvalidOperationException($"Existem tasks em estado inconsistente antes de iniciar: {string.Join(", ", pendentes)}");

            Executando = true;
            ProcessosStream = MontarFluxoProcessos();
            ResultadosStream = MontarFluxoResultados(ProcessosStream);
            ProcessosSubscription = SubscribeProcessos();
            ResultadosSubscription = SubscribeResultados();
        }

        #region "Estado das Tasks"

        /// <summary>  
        /// Obtém o estado(de enum TaskState) atual de uma task com base no ID fornecido.  
        /// </summary>  
        /// <param name="id">O identificador único da task.</param>  
        /// <returns>O estado da task, como <see cref="TaskState"/>.</returns>  
        /// <remarks>  
        /// Retorna o estado da task se o ID for encontrado no dicionário de situações.  
        /// Caso contrário, retorna o estado padrão <see cref="TaskState.AguardandoInicio"/>.  
        /// </remarks>  
        public override TaskState ObterEstadoTask(int id)
           => Situacoes.TryGetValue(id, out var s) ? s : TaskState.AguardandoInicio;

        #endregion

        #region "Aguarda Processamento Tasks"

        /// <summary>
        /// Aguarda a conclusão de todas as tasks em execução no grupo.
        /// Este método captura internamente todas as exceções ocorridas durante a execução das tasks
        /// e as armazena na propriedade <see cref="UltimasExceptions"/>.
        /// Nenhuma exceção é relançada diretamente por este método.
        /// </summary>
        /// <remarks>
        /// É crucial que você verifique o conteúdo da propriedade <see cref="UltimasExceptions"/>
        /// após a conclusão deste método para identificar e tratar quaisquer falhas que possam ter ocorrido.
        /// Se <see cref="UltimasExceptions"/> não estiver vazia, indica que uma ou mais tasks falharam.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Lançada se a execução do grupo ainda não foi iniciada.</exception>
        //public override async Task AguardaTodasTasksAsync()
        public override async Task AguardaTodasTasksAsync(bool throwOnAnyError = false)
        {
            // Limpa as exceções capturadas durante a execução das tasks do grupo de tarefas anteriores.
            this.UltimasExceptions = Enumerable.Empty<Exception>().ToList();

            // Garante que foi iniciada a Execução de Tasks
            if (!Executando)
                throw new InvalidOperationException("Execução ainda não iniciada.");

            // Garanta que todas as Tasks foram registradas
            await this.TasksPoolGroupCompleted.Task.ConfigureAwait(false);

            // Cria snapshot de <id, Task> antes de WhenAll
            var lista = ExecutandoTasks.ToList();
            var whenAll = Task.WhenAll(lista.Select(kv => kv.Value));

            // Acumula exceções sem relançar
            var excecoesCapturadas = new List<Exception>();

            try
            {
                // await só “lança” a primeira inner-exception,
                // mas mantemos whenAll.Exception para capturar todas
                await whenAll.ConfigureAwait(false);
            }
            catch
            {
                // empacota todas as inner-exceptions, mesmo se aninhadas
                var agg = whenAll.Exception?.Flatten();
                if (agg != null)
                {
                    foreach (var ex in agg.InnerExceptions)
                    {
                        excecoesCapturadas.Add(ex);

                        // opcional: identificar o id e nome da Task que falhou
                        var pair = lista
                            .FirstOrDefault(kv => kv.Value.Exception?.InnerExceptions.Contains(ex) == true);

                        var id = pair.Key;
                        var nomeTask = ParametrosDict.TryGetValue(id, out var prm)
                                           ? prm.RetornoNomeTask().ToString()
                                           : "<desconhecido>";

                        await HandleExceptionAsync(id, nomeTask, ex).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                // Expõe todas as exceções para o chamador
                UltimasExceptions = excecoesCapturadas.AsReadOnly();

                // >>> INÍCIO DA NOVA LÓGICA <<<
                if (throwOnAnyError && UltimasExceptions.Any())
                {
                    throw new AggregateException("Uma ou mais tasks falharam durante a execução do grupo.", UltimasExceptions);
                }
                // >>> FIM DA NOVA LÓGICA <<<
            }
        }

        /// <summary>  
        /// Aguarda a conclusão de uma task específica pelo nome e retorna seu resultado.  
        ///  
        /// Este método verifica se a task com o nome fornecido existe, se foi iniciada e se está em execução.  
        /// Caso contrário, lança exceções apropriadas.  
        ///  
        /// Após validar as condições, ele aguarda a conclusão da task e retorna o valor de retorno associado.  
        ///  
        /// Exceções:  
        /// <exception cref="KeyNotFoundException">Lançada se a task com o nome especificado não for encontrada.</exception>  
        /// <exception cref="InvalidOperationException">Lançada se a task ainda não foi iniciada ou não está em execução.</exception>  
        /// </summary>  
        /// <param name="nomeTask">O nome da task a ser aguardada.</param>  
        /// <returns>O valor de retorno da task, como <see cref="ITaskReturnValue"/>.</returns>  
        public override async Task AguardaTaskPorNomeAsync(string nomeTask)
        {
            var arrayNomeTask = new[] { nomeTask };
            await AguardaTaskPorNomeAsync(arrayNomeTask).ConfigureAwait(false);

        }

        /// <summary>
        /// Aguarda apenas as Tasks cujos nomes foram passados em <paramref name="nomesTasks"/>.
        /// Verifica registro e início de execução, captura as exceções específicas por ID
        /// e armazena em <see cref="UltimasExceptionsPorId"/>, sem relançar nada.
        /// </summary>
        /// <param name="nomesTasks">Array de nomes das Tasks que se deseja aguardar.</param>
        public override async Task AguardaTaskPorNomeAsync(string[] nomesTasks)
        {
            // 1) Validação de parâmetros
            if (nomesTasks == null || nomesTasks.Length == 0)
                throw new ArgumentException(
                    "O array de nomes de tasks não pode ser nulo ou vazio.", nameof(nomesTasks));

            if (!Executando)
                throw new InvalidOperationException("Execução ainda não iniciada.");

            // 2) Aguarda registro de todas as tasks
            if (TasksRegistradas != TasksCriadas)
                await TasksPoolGroupCompleted.Task.ConfigureAwait(false);

            // 3) Valida nomes informados
            var invalid = nomesTasks.Where(n => !NomeParaId.ContainsKey(n)).ToList();
            if (invalid.Any())
                throw new KeyNotFoundException(
                    $"Tasks não registradas: {string.Join(", ", invalid)}");

            // 4) Obtém o dicionário nome→(id, Task)
            //nome => NomeParaId[nome],
            //nome => ExecutandoTasks[NomeParaId[nome]]
            var tasksById = nomesTasks.ToDictionary(
                nome => NomeParaId.TryGetValue(nome, out var id) ? id : throw new KeyNotFoundException($"Task '{nome}' não encontrada."),
                nome => ExecutandoTasks.TryGetValue(
                                                    NomeParaId.TryGetValue(nome, out var id)
                                                    ? id
                                                    : throw new KeyNotFoundException($"Task '{nome}' não encontrada.")
                                                    , out var task)
                        ? task
                        : throw new KeyNotFoundException($"Task '{nome}' não encontrada.")
            );

            // 5) Limpa exceções anteriores
            if (UltimasExceptionsPorId is Dictionary<int, Exception> dict)
                dict.Clear();

            // 6) Aguarda todas de forma “silenciosa”
            var continued = tasksById
                .Select(kv =>
                    kv.Value.ContinueWith(t => new { Id = kv.Key, Task = t },
                                           TaskScheduler.Default))
                .ToArray();

            var results = await Task.WhenAll(continued)
                                    .ConfigureAwait(false);

            // 7) Trata cada resultado separadamente
            foreach (var r in results)
            {
                var id = r.Id;
                var t = r.Task;
                var nome = ParametrosDict[id].RetornoNomeTask();

                if (t.IsFaulted && t.Exception != null)
                {
                    // pega a InnerException principal
                    var ex = t.Exception.InnerException ?? t.Exception;
                    await HandleExceptionAsync(id, nome, ex).ConfigureAwait(false); // Replica o Handle individual da Tasks.
                }
                else if (t.IsCanceled)
                {
                    await HandleExceptionAsync(id, nome, new OperationCanceledException()).ConfigureAwait(false);
                }
                // caso bem-sucedido, não há nada a fazer
            }
        }

        #endregion

        #region "Obtem Resultado Task"

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
        public override async Task<IReadOnlyDictionary<int, ITaskReturnValue>> ObterTodosResultadosPorIdAsync()
        {

            // primeiro garanta que terminou (até com captura interna de erros)
            await AguardaTodasTasksAsync().ConfigureAwait(false);

            // Project: ID → Resultado (caso falhe, .Result relança exception)
            return ExecutandoTasks
                            .ToDictionary(
                                kv => kv.Key,
                                kv => kv.Value.Result
                            );
        }


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
        public override async Task<IReadOnlyDictionary<string, ITaskReturnValue>> ObterResultadosPorNomeAsync()
        {
            await AguardaTodasTasksAsync().ConfigureAwait(false);

            return ExecutandoTasks
                .ToDictionary(
                    kv => ParametrosDict[kv.Key].RetornoNomeTask().ToString(),
                    kv => kv.Value.Result
                );
        }

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
        public override async Task<ITaskReturnValue> ObterResultadoPeloNomeAsync(string nomeTask)
        {
            // 1) chama o método que retorna um dicionário nome → ITaskReturnValue
            var resultados = await ObterResultadosPelosNomesAsync(new[] { nomeTask })
                                     .ConfigureAwait(false);

            // 2) tenta extrair do dicionário
            if (!resultados.TryGetValue(nomeTask, out var retorno))
                throw new KeyNotFoundException($"Task '{nomeTask}' não encontrada nos resultados.");

            // 3) retorna o resultado
            return retorno;
        }

        /// <summary>
        /// Aguarda a conclusão das tasks especificadas por seus nomes e retorna um dicionário com os resultados.
        /// </summary>
        /// <param name="nomesTasks">Um array de strings contendo os nomes das tasks a serem aguardadas.</param>
        /// <returns>
        /// Um dicionário somente leitura onde a chave é o nome da task e o valor é o <see cref="ITaskReturnValue"/>.
        /// Se uma task falhou ou foi cancelada, seu valor correspondente no dicionário será <c>null</c>.
        /// As exceções originais que causaram falhas ao tentar acessar o resultado de uma task são
        /// armazenadas na propriedade <see cref="ErrosAoAcessarResultado"/>.
        /// </returns>
        /// <remarks>
        /// Após chamar este método, verifique a propriedade <see cref="ErrosAoAcessarResultado"/>
        /// para identificar exceções de tasks que falharam. O acesso direto a <c>Task.Result</c>
        /// (que ocorre internamente para tasks bem-sucedidas) relançaria a exceção da task individualmente se ela tivesse falhado,
        /// mas este método lida com isso armazenando a exceção e retornando <c>null</null> no dicionário para a task específica.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Lançada se a execução do grupo ainda não foi iniciada.</exception>
        /// <exception cref="KeyNotFoundException">Lançada se algum dos nomes de task fornecidos não corresponder a uma task registrada.</exception>
        public override async Task<IReadOnlyDictionary<string, ITaskReturnValue>> ObterResultadosPelosNomesAsync(string[] nomesTasks)
        {
            if (!Executando)
                throw new InvalidOperationException("Execução do grupo ainda não iniciada.");

            // 1) garanta que todas as Tasks já foram registradas
            await TasksPoolGroupCompleted.Task.ConfigureAwait(false);

            // 2) constrói mapa nome → id
            var mapNomeParaId = ParametrosDict
                .ToDictionary(kv => kv.Value.RetornoNomeTask().ToString(), kv => kv.Key);

            // 3) valida nomes informados
            var faltando = nomesTasks.Where(n => !mapNomeParaId.ContainsKey(n)).ToList();
            if (faltando.Any())
                throw new KeyNotFoundException(
                    "Tasks não registradas: " + string.Join(", ", faltando)
                );

            // 4) valida que já estão em execução
            var ids = nomesTasks.Select(n => mapNomeParaId[n]).ToList();
            var naoIniciadas = ids.Where(id => !ExecutandoTasks.ContainsKey(id)).ToList();
            if (naoIniciadas.Any())
                throw new InvalidOperationException(
                    "Tasks ainda não iniciaram: " + string.Join(", ", naoIniciadas)
                );

            // 5) snapshot de pares (nome + Task)
            var pares = ids
                .Select(id => new
                {
                    Nome = ParametrosDict[id].RetornoNomeTask().ToString(),
                    Tarefa = ExecutandoTasks[id]
                })
                .ToList();

            // 6) aguarda só essas subtasks, ignorando AggregateException
            try
            {
                await Task.WhenAll(pares.Select(p => p.Tarefa)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Ignora a exceção do WhenAll, pois a lógica abaixo
                // tratará tarefas individuais que falharam.
            }

            // 7) monta o dicionário de resultados, tratando falhas e cancelamentos
            var resultados = pares.ToDictionary(
                p => p.Nome,
                p =>
                {
                    if (p.Tarefa.IsFaulted)
                    {
                        // Registra o erro ao tentar acessar o resultado
                        var id = mapNomeParaId[p.Nome]; // Obtém o ID correspondente ao nome
                        var innerEx = p.Tarefa.Exception?.InnerException ?? p.Tarefa.Exception;
                        if (innerEx != null)
                        {
                            _errosAoAcessarResultado.TryAdd(id, innerEx);
                        }
                        return null; // Retorna null para tasks falhadas
                    }
                    else if (p.Tarefa.IsCanceled)
                    {
                        return null; // Retorna null para tasks canceladas
                    }
                    else // IsCompletedSuccessfully
                    {
                        return p.Tarefa.Result;
                    }
                }
            );

            return resultados;
        }

        #endregion

        #region "Dispose e Cancelamento de Tasks"
        /// <summary>          
        /// Libera todos os recursos utilizados pela instância de:
        /// <see cref="TasksGrupos"/>.
        /// 
        /// Este método descarta as assinaturas de fluxos de processos e resultados,
        /// limpa os dicionários internos e libera o cancellationNovo de cancelamento associado ao grupo.
        /// Após a chamada deste método, a instância não deve mais ser utilizada.
        /// </summary>
        public override void Dispose()
        {
            if (this.Executando)
            {
                // Cancela todas as tasks em execução antes de liberar recursos
                this.CancelarGrupo();
            }

            // Libera assinaturas de fluxos
            this.ProcessosSubscription?.Dispose();
            this.ResultadosSubscription?.Dispose();

            // Libera o Subject de resultados
            this.ResultadosSubject?.OnCompleted();
            this.ResultadosSubject?.Dispose();

            // Limpa os dicionários internos
            this.Funcoes.Clear();
            this.ParametrosDict.Clear();
            this.NomeParaId.Clear();
            this.ExecutandoTasks.Clear();
            this.TaskResults.Clear();
            this.Situacoes.Clear();
            this.TaskIdComCancellationTokenSource.Clear();

            // Libera o cancellationNovo de cancelamento
            this.CtsGrupo.Dispose();

            // Marca a instância como não mais utilizável
            this.Executando = false;
        }

        #endregion

        #region "Administra Cancel Token"

        /// <summary>  
        /// Cancela todas as tasks do grupo.  
        ///  
        /// Este método sinaliza o cancellationNovo de cancelamento associado ao grupo,  
        /// interrompendo a execução de todas as tasks que ainda não foram concluídas.  
        ///  
        /// Exceções:  
        /// <exception cref="ObjectDisposedException">Lançada se o cancellationNovo de cancelamento já foi descartado.</exception>  
        /// </summary>  
        public override void CancelarGrupo() => CtsGrupo.Cancel();

        /// <summary>  
        /// Cancela todas as tasks do grupo.  
        ///  
        /// Este método sinaliza o cancellationNovo de cancelamento associado ao grupo,  
        /// interrompendo a execução de todas as tasks que ainda não foram concluídas.  
        ///  
        /// Exceções:  
        /// <exception cref="ObjectDisposedException">Lançada se o cancellationNovo de cancelamento já foi descartado.</exception>  
        /// </summary>  
        public override void CancelamentoBruscoGrupo() => CtsGrupo.Cancel();


        /// <summary>  
        /// Cancela uma task específica pelo nome dela.  
        /// </summary>  
        /// <param name="nomeTask">O nome da task a ser cancelada.</param>  
        /// <exception cref="KeyNotFoundException">Lançada se a task com o nome especificado não for encontrada.</exception>  
        /// <exception cref="InvalidOperationException">Lançada se o cancellationNovo de cancelamento da task não for encontrado.</exception>  
        public override void CancelarTaskPorNome(string nomeTask)
        {

            if (!NomeParaId.TryGetValue(nomeTask, out var id))
            {
                throw new KeyNotFoundException($"Task '{nomeTask}' não encontrada.");
            }

            CancelarTaskPorId(id);
        }

        /// <summary>
        /// Cancela a task pelo seu identificador.
        /// </summary>
        public void CancelarTaskPorId(int id)
        {
            if (!ParametrosDict.TryGetValue(id, out var parametros))
                throw new KeyNotFoundException($"Task ID {id} não encontrada.");

            if (TaskIdComCancellationTokenSource.TryGetValue(id, out var cts))
                cts.Cancel();
            else
                throw new InvalidOperationException($"CancellationTokenSource não encontrado para Task ID {id}.");

            //if (parametros.RetornoCancellationToken is CancellationToken src)
            //    src.Cancel();
            //else
            //    throw new InvalidOperationException($"Token não encontrado para Task ID {id}.");
        }
        #endregion

        #endregion

        #region "Metodos Estaticos Publicos"


        public static async Task TaskCallEvents(EventosTasksAsync eventos, ITaskParametros parametros)
        {
            foreach (var metodo in eventos.GetInvocationList())
            {
                if (metodo is Func<ITaskParametros, Task> funcTask)
                {
                    await TaskCallNoReturn(funcTask, parametros).ConfigureAwait(false);
                }
            }
        }

        #region "Chamada subTask"

        public static async Task TaskCallNoReturn(Func<ITaskParametros, Task> funcTask, ITaskParametros parametros)
        {
            var grupoTask = parametros.RetornoTasksGrupo();

            if (funcTask == null)
            {
                await CreateFaultedTask<Task>(
                    new InvalidOperationException($"Função fornecida para TaskCallNoReturn é nula.")
                ).ConfigureAwait(false);
                return; // Corrige o erro CS1997 removendo a expressão de retorno em métodos async void ou Task.  
            }

            try
            {
                var baseTask = funcTask(parametros);
                if (baseTask == null)
                {
                    // Se a própria função retornar null, cria uma task falhada
                    await CreateFaultedTask<ITaskReturnValue>(
                        new InvalidOperationException($"Função {funcTask.Method.Name} retornou uma Task nula.")
                    ).ConfigureAwait(false);
                    return;
                }

                var tokenManager = parametros.RetornoCancellationToken;
                var token = tokenManager == default ? CancellationToken.None : tokenManager;

                // Agenda a execução conforme a configuração do grupo (single-thread ou não)
                Task scheduledTask = grupoTask.UseSingleThread
                    ? Task.Factory.StartNew(() => baseTask,
                        token,
                        TaskCreationOptions.AttachedToParent, // Mantém AttachedToParent para single-thread
                        grupoTask.SchedulerTask).Unwrap()
                    : baseTask;
                await scheduledTask.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Se qualquer exceção ocorrer durante a execução ou await,
                // captura e retorna uma Task explicitamente falhada com essa exceção.
                await CreateFaultedTask<ITaskReturnValue>(ex).ConfigureAwait(false);
            }

        }

        /// <summary>
        /// Executa uma função assíncrona representada por <paramref name="funcTask"/> com os parâmetros fornecidos,
        /// utilizando a configuração atual do grupo de tasks, incluindo a política de agendamento (paralelo ou single-thread).
        ///
        /// Caso <see cref="UseSingleThread"/> esteja ativado, a execução será agendada no <see cref="SchedulerTask"/> do grupo.
        /// Quando em modo single-thread, a task é vinculada ao contexto da task chamadora usando <see cref="TaskCreationOptions.AttachedToParent"/>,
        /// permitindo que seja considerada parte da hierarquia da task principal.
        ///
        /// Este método é ideal para chamadas auxiliares internas dentro de uma task já gerenciada pelo grupo,
        /// mantendo coesão no modelo assíncrono e respeitando os tokens de cancelamento herdados de <paramref name="parametros"/>.
        ///
        /// </summary>
        /// <param name="funcTask">A função assíncrona a ser executada, que recebe um <see cref="ITaskParametros"/> e retorna um <see cref="ITaskReturnValue"/>.</param>
        /// <param name="parametros">Os parâmetros necessários para execução da função, incluindo cancellationNovo de cancelamento e configurações adicionais.</param>
        /// <returns>Uma <see cref="Task{ITaskReturnValue}"/> representando o resultado da função executada.</returns>
        /// <exception cref="InvalidOperationException">Lançada caso a função fornecida seja nula.</exception>
        public static async Task<ITaskReturnValue> TaskCallReturn(Func<ITaskParametros, Task<ITaskReturnValue>> funcTask, ITaskParametros parametros)
        {
            var grupoTask = parametros.RetornoTasksGrupo();

            if (funcTask == null)
                return await CreateFaultedTask<ITaskReturnValue>(
                    new InvalidOperationException($"Função fornecida para TaskCallReturn é nula.")
                ).ConfigureAwait(false); // Retorna task falhada se funcTask for nula

            try
            {
                var tokenManager = parametros.RetornoCancellationToken;
                var token = tokenManager == default ? CancellationToken.None : tokenManager;

                // Executa a função original para obter a Task base
                var baseTask = funcTask(parametros);
                if (baseTask == null)
                {
                    // Se a própria função retornar null, cria uma task falhada
                    return await CreateFaultedTask<ITaskReturnValue>(
                        new InvalidOperationException($"Função {funcTask.Method.Name} retornou uma Task nula.")
                    ).ConfigureAwait(false);
                }

                // Agenda a execução conforme a configuração do grupo (single-thread ou não)
                Task<ITaskReturnValue> scheduledTask = grupoTask.UseSingleThread
                    ? Task.Factory.StartNew(() => baseTask,
                        token,
                        TaskCreationOptions.AttachedToParent, // Mantém AttachedToParent para single-thread
                        grupoTask.SchedulerTask).Unwrap()
                    : baseTask;

                // Aguarda a conclusão da task agendada
                ITaskReturnValue taskReturnValue = await scheduledTask.ConfigureAwait(false);
                return taskReturnValue;
            }
            catch (Exception ex)
            {
                // Se qualquer exceção ocorrer durante a execução ou await,
                // captura e retorna uma Task explicitamente falhada com essa exceção.
                return await CreateFaultedTask<ITaskReturnValue>(ex).ConfigureAwait(false);
            }
        }

        #endregion

        #endregion

        #region "Metodos Protegidos"

        /// <summary>
        /// Monta o fluxo de processos(sequência de observables) para execução paralela das tasks no grupo sem aguardar conclusão.
        ///
        /// Este método utiliza Reactive Extensions (Rx) para criar um fluxo observável
        /// que gerencia a execução das tasks adicionadas ao grupo. Ele organiza as tasks
        /// com base em seus IDs, cria entradas de processo para cada uma e as executa
        /// respeitando o limite de paralelismo definido em <see cref="MaxDegreeOfParallelism"/>.
        ///
        /// O fluxo resultante é configurado para ser compartilhado entre múltiplos assinantes
        /// e é iniciado automaticamente quando uma assinatura é feita.
        ///
        /// Exceções:
        /// <exception cref="InvalidOperationException">Lançada se ocorrer um erro ao criar uma entrada de processo.</exception>
        /// </summary>
        ///
        /// Retorno:
        /// <returns>Um <see cref="IObservable{T}"/> que emite pares de ID e Task para execução.</returns>
        protected override IObservable<KeyValuePair<int, Task<ITaskReturnValue>>> MontarFluxoProcessos()
        {
            TasksCriadas = Funcoes.Count;
            TasksRegistradas = 0;

            // Para cada ID, cria um IObservable que emite 1 item via método async
            var inner = Funcoes.Keys
                .OrderBy(id => id)
                .Select(id =>
                    Observable.FromAsync(async () =>
                    {
                        try
                        {
                            LockMutexPorServico(id);
                            var retorno = await CriarProcessEntryAsync(id).ConfigureAwait(false);
                            var ok = ExecutandoTasks.TryGetValue(id, out var task);
                            Interlocked.Increment(ref TasksRegistradas);
                            if (TasksRegistradas == TasksCriadas)
                            {
                                TasksPoolGroupCompleted.TrySetResult(true);
                            }
                            return new KeyValuePair<int, Task<ITaskReturnValue>>(id, task);
                            //return new KeyValuePair<int, Task<ITaskReturnValue>>(id, CreateFaultedTask<ITaskReturnValue>(ex));
                        }
                        catch (Exception ex)
                        {
                            // LINHA ORIGINAL:
                            // throw new InvalidOperationException($"Não foi possivel criar a Task 'id'");

                            // SUGESTÃO DE ALTERAÇÃO:
                            throw new InvalidOperationException($"Não foi possível criar a entrada de processo para a Task ID: {id}.", ex);
                        }

                        //try
                        //{
                        //    return await CriarProcessEntryAsync(id).ConfigureAwait(false);
                        //}
                        //catch (Exception ex)
                        //{
                        //    var ok = ParametrosDict.TryGetValue(id, out var parametros);
                        //    var nome = parametros.RetornoNomeTask();
                        //    // HandleException(id, nome, ex);
                        //    Interlocked.Increment(ref TasksRegistradas);
                        //    if (TasksRegistradas == TasksCriadas)
                        //    {
                        //        TasksPoolGroupCompleted.TrySetResult(true);
                        //    }
                        //    return new KeyValuePair<int, Task<ITaskReturnValue>>(id, CreateFaultedTask<ITaskReturnValue>(ex));
                        //}
                    }));

            // Agora Merge(int) existe e limita paralelismo
            return inner
                .Merge(MaxDegreeOfParallelism)
                .Publish()
                .RefCount();
        }

        /// <summary>  
        /// Monta o fluxo de resultados para processar as tasks do grupo.  
        ///  
        /// Este método utiliza Reactive Extensions (Rx) para transformar cada task em um fluxo observável  
        /// que encapsula os eventos de conclusão (OnNext) e erro (OnError).  
        ///  
        /// Ele materializa os resultados das tasks, associando cada resultado ao ID correspondente,  
        /// e publica o fluxo para ser compartilhado entre múltiplos assinantes.  
        /// 
        /// Ligação: usado após <see cref="IGrupoTasks.MontarFluxoProcessos()"/> em <see cref="IGrupoTasks.IniciarExecucao()"/>.
        /// Exceções:  
        /// <exception cref="InvalidOperationException">Lançada se ocorrer um erro ao processar os resultados.</exception>  
        /// </summary>  
        /// <param name="processos">O fluxo de processos contendo pares de ID e Task.</param>  
        /// <returns>Um <see cref="IObservable{T}"/> que emite notificações de resultados das tasks em ITaskReturnValue.</returns>  
        protected override IObservable<KeyValuePair<int, ITaskReturnValue>> MontarFluxoResultados(IObservable<KeyValuePair<int, Task<ITaskReturnValue>>> processos)
        {
            // Limpa as exceções capturadas durante a execução das tasks do grupo de tarefas anteriores.
            this.UltimasExceptions = Enumerable.Empty<Exception>().ToList();

            // 1) subscribe no fluxo de tasks em execução
            processos.Subscribe(
                kvp => ProcessaResultado(kvp),

                // 2) OnError do fluxo: dispara apenas UMA vez
                ex =>
                {
                    // erro que quebrou o fluxo de emissões
                    TratamentoErroGrupo?.Invoke(
                        0,
                        NomeGrupo,
                        "MontarFluxoResultados em TasksGrupos",
                        ex
                    );
                    ResultadosSubject.OnError(ex);
                },

                // 3) OnCompleted do fluxo
                () => ResultadosSubject.OnCompleted()
            );

            // 4) devolve o Observable de resultados
            return ResultadosSubject.AsObservable();
        }

        /// <summary>
        /// Inscreve no fluxo de processos para gerenciar os eventos OnNext, OnError e OnCompleted.
        /// 
        /// Este método utiliza o fluxo de processos configurado em <see cref="MontarFluxoProcessos"/> para
        /// iniciar a execução das tasks no grupo. Ele trata os seguintes eventos:
        /// - OnNext: Ignorado, pois o processamento ocorre no fluxo de resultados.
        /// - OnError: Dispara o evento <see cref="TratamentoErroGrupo"/> para tratar erros globais.
        /// - OnCompleted: Dispara o evento <see cref="Finalizacao"/> para indicar que todas as tasks foram processadas.
        /// 
        /// Ligação: retornado em <see cref="IGrupoTasks.IniciarExecucao()"/> e armazena a subscription.
        /// Exceções:
        /// <exception cref="InvalidOperationException">Lançada se ocorrer um erro durante a assinatura do fluxo.</exception>
        /// </summary>
        protected override IDisposable SubscribeProcessos()
        {
            return ProcessosStream.Subscribe(
                kvp =>
                {
                    // Task foi criada e registrada em ExecutandoTasks dentro de CriarProcessEntryAsync
                    Interlocked.Increment(ref TasksRegistradas);
                    if (TasksRegistradas == TasksCriadas)
                    {
                        TasksPoolGroupCompleted.TrySetResult(true);
                    }
                },
                ex =>
                {
                    TratamentoErroGrupo?.Invoke(0, NomeGrupo, "SubscribeProcessos", ex);
                    TasksPoolGroupCompleted.TrySetException(ex);
                },
                () =>
                {

                });
        }

        /// <summary>  
        /// Inicia a execução de todas as tasks adicionadas ao grupo.  
        ///  
        /// Este método monta os fluxos de processos e resultados utilizando Reactive Extensions (Rx).  
        /// Ele chama internamente os métodos:
        /// <see cref="MontarFluxoProcessos"/>
        /// e
        /// <see cref="MontarFluxoResultados(IObservable{KeyValuePair{int, Task{ITaskReturnValue}}})"/>  
        /// para configurar os fluxos de execução paralela e de resultados.  
        ///  
        /// Após a configuração, os fluxos são assinados para iniciar a execução das tasks e tratar os resultados.  
        ///  
        /// Exceções:  
        /// <exception cref="InvalidOperationException">Lançada se a execução já foi iniciada anteriormente.</exception>  
        /// </summary>  
        /// <exception cref="InvalidOperationException"></exception>
        protected override IDisposable SubscribeResultados()
        {
            return ResultadosStream.Subscribe(_ => { },
                ex =>
                {
                    TratamentoErroGrupo?.Invoke(0, NomeGrupo, "SubscribeResultados", ex);
                    TasksPoolGroupCompleted.TrySetException(ex);
                },
                () =>
                {

                }
                );
        }

        protected void LockMutexPorServico(int id)
        {

            bool createdNew;
            var mutexName = $"Global\\Mutex_{this.NomeGrupo}_Id_{id}";
            var servicoMutex = new Mutex(true, mutexName, out createdNew);

            var tarefaNome = string.Empty;
            if (IdParaNome.TryGetValue(id, out var nome))
            {
                tarefaNome = nome;
            }

            if (!createdNew)
            {
                servicoMutex?.Dispose();
                servicoMutex = null;
                throw new InvalidOperationException(
                    $"Já existe uma instância do serviço '{tarefaNome}' em execução!\n" +
                    $"Mutex: {mutexName}");
            }

            if (!ServicoMutex.TryAdd(id, servicoMutex))
            {
                throw new ArgumentException($"Já existe uma task com o ID '{id}' registrada para Mutex de Serviço.", nameof(id));
            }

        }

        /// <summary>  
        /// Método async que monta toda a lógica e cria a entrada de processo para o ID informado, incluindo cancellationNovo de cancelamento e Task associada.  
        ///  
        /// Este método é responsável por inicializar a execução de uma task específica no grupo,  
        /// configurando os parâmetros necessários, como timeout e cancellationNovo de cancelamento.  
        /// Ele também atualiza o estado da task para "EmProcessamento" e armazena a task em execução no dicionário interno.  
        ///
        /// Exceções:  
        /// <exception cref="InvalidOperationException">Lançada se a função associada ao ID retornar uma task nula.</exception>  
        /// </summary>  
        /// <param name="id">O identificador único da task a ser criada.</param>  
        /// <returns>Um par contendo o ID da task e a Task associada.</returns>
        protected override async Task<KeyValuePair<int, Task<ITaskReturnValue>>> CriarProcessEntryAsync(int id)
        {
            // 1) Atualiza estado para EmProcessamento
            UpdateTaskState(id, TaskState.EmProcessamento);

            // 2) Prepara parâmetros e timeout
            var parametros = ParametrosDict[id];
            var timeoutMs = (int)parametros.RetornoTimeOutMilliseconds();

            // 3) Cria cancellationNovo individual com timeout
            var ctsIndividual = timeoutMs == Timeout.Infinite
                ? new CancellationTokenSource()
                : new CancellationTokenSource(timeoutMs);

            //  // 4) Linka cancellationNovo do grupo de forma assíncrona
            //  await RegistraCancellationTokenSourceTaskNoGroupCancellationTokenSource(ctsIndividual).ConfigureAwait(false);
            //  parametros.ArmazenaCancellationToken(ctsIndividual);

            // 5) Executa a função ou gera Task faulted
            var funcTask = Funcoes[id](parametros);
            var baseTask = funcTask ?? CreateFaultedTask<ITaskReturnValue>(
                new InvalidOperationException($"Função retornou task nula. id = 'id'")
            );

            // 6) Se UseSingleThread, programa a execução no scheduler dedicado
            Task<ITaskReturnValue> scheduledTask = UseSingleThread
                ? Task.Factory.StartNew(() => baseTask,
                    ctsIndividual.Token,
                    TaskCreationOptions.None,
                    SchedulerTask
                  ).Unwrap()
                : baseTask;

            // 7) Guarda no dicionário de tasks em execução
            if (!ExecutandoTasks.TryAdd(id, scheduledTask))
            {
                throw new InvalidOperationException($"Não foi possivel registrar a Task 'id'");
            }

            // 8) Retorna o par com ID e Task
            return new KeyValuePair<int, Task<ITaskReturnValue>>(id, scheduledTask);
        }

        /// <summary>  
        /// Método sincrono que monta toda a lógica e cria a entrada de processo para o ID informado, incluindo cancellationNovo de cancelamento e Task associada.  
        ///  
        /// Este método é responsável por inicializar a execução de uma task específica no grupo,  
        /// configurando os parâmetros necessários, como timeout e cancellationNovo de cancelamento.  
        /// Ele também atualiza o estado da task para "EmProcessamento" e armazena a task em execução no dicionário interno.  
        ///
        /// Exceções:  
        /// <exception cref="InvalidOperationException">Lançada se a função associada ao ID retornar uma task nula.</exception>  
        /// </summary>  
        /// <param name="id">O identificador único da task a ser criada.</param>  
        /// <returns>Um par contendo o ID da task e a Task associada.</returns>
        protected override KeyValuePair<int, Task<ITaskReturnValue>> CriarProcessEntry(int id)
        {
            // Bloqueia o thread de caller para manter assinatura síncrona
            return CriarProcessEntryAsync(id)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Adiciona uma nova task ao grupo, garantindo que a tarefa seja registrada corretamente.
        /// 
        /// Este método realiza validações rigorosas para evitar conflitos de IDs, funções, parâmetros e nomes.
        /// Ele assegura que os dados necessários estejam presentes antes do início da execução da task.
        /// 
        /// Exceções:
        /// <exception cref="InvalidOperationException">Lançada se a execução já foi iniciada.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lançada se o ID fornecido for menor ou igual a zero.</exception>
        /// <exception cref="ArgumentNullException">Lançada se a função ou os parâmetros forem nulos.</exception>
        /// <exception cref="ArgumentException">Lançada se já existir uma função, parâmetros ou nome duplicado para o ID fornecido.</exception>
        /// </summary>
        /// <param name="id">O identificador único da task.</param>
        /// <param name="funcao">A função que define a lógica da task.</param>
        /// <param name="parametros">Os parâmetros necessários para a execução da task.</param>
        /// <param name="nomeTask">O nome opcional da task. Se não fornecido, será gerado automaticamente.</param>
        protected override async Task HandleExceptionAsync(int id, string nomeTask, Exception ex)
        {
            // se já trataram esse id, sai sem disparar novamente
            if (!_erroDisparado.TryAdd(id, true))
                return;

            // Determina o tipo de exceção para tratamento específico
            if (ex is OperationCanceledException)
            {
                // Tratamento de cancelamento
                if (TratamentoCancelamentoGrupo != null)
                {
                    UpdateTaskState(id, TaskState.Cancelada);
                    await TratamentoCancelamentoGrupo(id, NomeGrupo, nomeTask, ex).ConfigureAwait(false); // tratamento Cancelamento no Task Individual
                }
            }
            else if (ex is TimeoutException)
            {
                // Tratamento de timeout
                if (TratamentoTimeOutGrupo != null)
                {
                    UpdateTaskState(id, TaskState.Timeout);
                    await TratamentoTimeOutGrupo(id, NomeGrupo, nomeTask, ex).ConfigureAwait(false); // tratamento TimeOut no Task Individual
                }
            }
            else
            {
                // Tratamento de erro genérico
                if (TratamentoErroGrupo != null)
                {
                    // Atualiza o estado da task para Falha
                    UpdateTaskState(id, TaskState.ComErro);
                    // Marca que o erro já foi disparado para este ID
                    _erroDisparado.TryAdd(id, true);
                    // Chama o delegate assíncrono e aguarda sua conclusão
                    await TratamentoErroGrupo(id, NomeGrupo, nomeTask, ex).ConfigureAwait(false); // tratamento erro no Task Individual
                }
            }
        }

        //protected override void HandleException(int id, string nomeTask, Exception ex)
        //{
        //    // se já trataram esse id, sai sem disparar novamente
        //    if (!_erroDisparado.TryAdd(id, true))
        //        return;

        //    if (ex is TimeoutException)
        //    {
        //        UpdateTaskState(id, TaskState.Timeout);
        //        TratamentoTimeOutGrupo?.Invoke(id, NomeGrupo, nomeTask, ex);
        //    }
        //    else if (ex is OperationCanceledException)
        //    {
        //        UpdateTaskState(id, TaskState.Cancelada);
        //        TratamentoCancelamentoGrupo?.Invoke(id, NomeGrupo, nomeTask, ex);
        //    }
        //    else
        //    {
        //        UpdateTaskState(id, TaskState.ComErro);
        //        TratamentoErroGrupo?.Invoke(id, NomeGrupo, nomeTask, ex); // tratamento erro no Task Individual
        //        // acrescentar em UltimasExceptionsPorId o exception
        //        // Replace the following line:  
        //        // UltimasExceptionsPorId.TryAdd(id, ex);  
        //        // With this corrected code:  
        //        if (UltimasExceptionsPorId is Dictionary<int, Exception> mutableExceptions)
        //        {
        //            mutableExceptions[id] = ex;
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("UltimasExceptionsPorId is not a mutable dictionary.");
        //        }
        //    }
        //    if (ParametrosDict.TryGetValue(id, out var prm)
        //        && prm.RetornoCancellationTokenManager() is CancellationTokenManager cts)
        //    {
        //        cts.Cancel();
        //    }
        //}

        /// <summary>
        /// Atualiza o estado de uma task no grupo.
        /// 
        /// Este método é utilizado para alterar o estado de uma task específica, identificada pelo seu ID,
        /// para um novo estado fornecido. Ele atualiza o dicionário interno de estados das tasks, garantindo
        /// que o estado atual seja refletido corretamente.
        /// 
        /// Exemplo de estados possíveis: <see cref="TaskState.AguardandoInicio"/>, <see cref="TaskState.EmProcessamento"/>,
        /// <see cref="TaskState.Finalizada"/>, entre outros.
        /// 
        /// Exceções:
        /// <exception cref="ArgumentException">Lançada se o ID fornecido for inválido ou não existir no grupo.</exception>
        /// 
        /// </summary>
        /// <param name="id">O identificador único da task cujo estado será atualizado.</param>
        /// <param name="state">O novo estado a ser atribuído à task.</param>
        protected override void UpdateTaskState(int id, TaskState state)
            => Situacoes.AddOrUpdate(id, state, (_, __) => state);

        #region "Administra Cancel Token"

        protected override async Task<bool> AdicionaCancellationTokenComTaskId(int id, CancellationToken individualToken)
        {
            var ok = await ArmazenaCancellationTokenPeloTaskId(id, individualToken).ConfigureAwait(false);

            return ok;
        }

        protected override async Task<bool> AdicionaCancellationTokenBreakComTaskId(int id, CancellationToken individualTokenBreak)
        {
            var ok = await ArmazenaCancellationTokenBreakPeloTaskId(id, individualTokenBreak).ConfigureAwait(false);

            return ok;
        }

        protected override async Task<bool> ArmazenaCancellationTokenPeloTaskId(int id, CancellationToken cancelToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            var task = Task.Run(async () =>
            {
                bool adicionado = await RelacionaTaskComCancellationTokenAsync(id, cancelToken).ConfigureAwait(false);
                tcs.SetResult(adicionado);
            });

            await task.ConfigureAwait(false);
            return tcs.Task.Result;
        }

        protected override async Task<bool> ArmazenaCancellationTokenBreakPeloTaskId(int id, CancellationToken cancelTokenBreak)
        {
            var tcs = new TaskCompletionSource<bool>();

            var task = Task.Run(async () =>
            {
                bool adicionado = await RelacionaTaskComCancellationTokenBreakAsync(id, cancelTokenBreak).ConfigureAwait(false);
                tcs.SetResult(adicionado);
            });

            await task.ConfigureAwait(false);
            return tcs.Task.Result;
        }

        protected override async Task<bool> RelacionaTaskComCancellationTokenAsync(int id, CancellationToken cancelToken)
        {
            bool adicionado = false;

            for (var tentativas = 0; tentativas < MaxTokenRegistrationAttempts; tentativas++)
            {
                var cancellationNovo = CancellationTokenSource.CreateLinkedTokenSource(
                    this.CtsGrupo.Token,
                    cancelToken);
                adicionado = this.TaskIdComCancellationTokenSource.TryAdd(id, cancellationNovo);
                if (adicionado)
                {
                    break;
                }
                await Task.Delay(TokenRegistrationDelayMs).ConfigureAwait(false);
            }

            return adicionado;
        }

        protected override async Task<bool> RelacionaTaskComCancellationTokenBreakAsync(int id, CancellationToken cancelTokenBreak)
        {
            bool adicionado = false;

            for (var tentativas = 0; tentativas < MaxTokenRegistrationAttempts; tentativas++)
            {
                var cancellationNovo = CancellationTokenSource.CreateLinkedTokenSource(
                    this.CtsGrupoThrow.Token,
                    cancelTokenBreak);
                adicionado = this.TaskIdComCancellationTokenSource.TryAdd(id, cancellationNovo);
                if (adicionado)
                {
                    break;
                }
                await Task.Delay(TokenRegistrationDelayMs).ConfigureAwait(false);
            }

            return adicionado;
        }

        //protected override async Task<bool> RegistraCancellationTokenSourceTaskNoGroupCancellationTokenSource(CancellationTokenSource cancelToken)
        //{
        //    var token = this.CtsGrupo.Token.Register(() =>
        //    {
        //        cancelToken.Cancel();
        //    });

        //    // cancellationNovo realmente cancelável  ❗
        //    var registrou = cancelToken.Token.CanBeCanceled && !token.Equals(default);  // handle não-vazio
        //    return await Task.FromResult(registrou);
        //}

        #endregion

        #endregion

        #region "Metodos Privados"

        private Task GetTaskByName(string nomeTask)
        {
            if (ExecutandoTasks.TryGetValue(NomeParaId[nomeTask], out var task))
            {
                return task;
            }
            return null;
        }

        /// <summary>
        /// Trata o resultado (ou exceção) de UMA task individual.
        /// </summary>
        private async void ProcessaResultado(KeyValuePair<int, Task<ITaskReturnValue>> kvp)
        {
            try
            {
                // aguarda só esta task
                var resultado = await kvp.Value.ConfigureAwait(false);

                // sucesso: atualiza estado e empurra no Subject
                TaskResults[kvp.Key] = resultado;
                UpdateTaskState(kvp.Key, TaskState.Finalizada);
                ResultadosSubject.OnNext(
                    new KeyValuePair<int, ITaskReturnValue>(kvp.Key, resultado)
                );
                Finalizacao?.Invoke();
            }
            catch (Exception ex)
            {
                // falha nesta task: dispara HandleException apenas UMA vez
                var nome = ParametrosDict[kvp.Key].RetornoNomeTask();
                await HandleExceptionAsync(kvp.Key, nome, ex).ConfigureAwait(false); // Handle individual de Tasks.
            }
        }

        /// <summary>  
        /// Inicia a execução de todas as tasks adicionadas ao grupo.  
        ///  
        /// Este método monta os fluxos de processos e resultados utilizando Reactive Extensions (Rx).  
        /// Ele chama internamente os métodos:
        /// <see cref="MontarFluxoProcessos"/>
        /// e
        /// <see cref="MontarFluxoResultados(IObservable{KeyValuePair{int, Task{ITaskReturnValue}}})"/>  
        /// para configurar os fluxos de execução paralela e de resultados.  
        ///  
        /// Após a configuração, os fluxos são assinados para iniciar a execução das tasks e tratar os resultados.  
        ///  
        /// Exceções:  
        /// <exception cref="InvalidOperationException">Lançada se a execução já foi iniciada anteriormente.</exception>  
        /// </summary>  
        /// <exception cref="InvalidOperationException"></exception>
        private static Task<T> CreateFaultedTask<T>(Exception ex)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(ex);
            return tcs.Task;
        }

        #endregion

    }
}
