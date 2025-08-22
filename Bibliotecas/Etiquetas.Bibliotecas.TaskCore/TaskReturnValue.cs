using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TaskCore
{
    /// <summary>
    /// Representa uma coleção de valores de retorno e parâmetros para uma task,
    /// permitindo armazenar, recuperar e converter múltiplos valores tipados.
    /// </summary>
    /// <remarks>
    /// Alterações aplicadas:
    /// 1. Uso de CallerInfoAttributes para obter classe/método chamador sem StackTrace.
    /// 2. Dicionário de conversores pré-inicializado para primitivos e Nullable&lt;T&gt;.
    /// 3. Metadados expostos como propriedades fortemente tipadas com Fluent API.
    /// </remarks>
    public class TaskReturnValue : ITaskParametros
    {
        // --- Metadados fortemente tipados ---
        /// <summary>Nome completo da classe e método que criou a instância.</summary>
        public override string NomeClasseChamou { get; set; }

        /// <summary>Identificador numérico da task.</summary>
        public override int IdTask { get; set; }

        /// <summary>Nome descritivo da task.</summary>
        public override string NomeTask { get; set; }

        /// <summary>Estado atual da task.</summary>
        public override TaskState StatusTask { get; set; }

        /// <summary>Controlador de cancelamento (Token, Source ou Manager).</summary>
        public override object CancellationController { get; set; }

        /// <summary>
        /// Grupo de tarefas associado à instância atual.
        /// </summary>
        public override TasksGrupos GrupoTasks { get; set; }

        /// <summary>Token de cancelamento efetivo, extraído do controlador.</summary>
        public override CancellationToken RetornoCancellationToken
        {
            get
            {
                if (CancellationController is CancellationToken ct)
                    return ct;
                if (CancellationController is CancellationTokenSource src)
                    return src.Token;
                if (CancellationController is CancellationTokenManager mgr)
                    return mgr.Token;
                return CancellationToken.None;
            }
        }

        /// <summary>Opções de criação da <see cref="Task"/>.</summary>
        public override TaskCreationOptions TaskCreationOptions { get; set; }

        /// <summary>Timeout em milissegundos antes do cancelamento.</summary>
        public int TimeoutMilliseconds { get; private set; }

        // --- Parâmetros do usuário ---
        protected int Sequencia;
        protected int QuantidadeParametros;

        /// <inheritdoc />
        public override int TotalParametros => QuantidadeParametros;

        /// <inheritdoc />
        public override ConcurrentDictionary<int, Type> Tipo { get; } = new ConcurrentDictionary<int, Type>();

        /// <inheritdoc />
        public override ConcurrentDictionary<int, object> Valor { get; } = new ConcurrentDictionary<int, object>();

        /// <inheritdoc />
        public override ConcurrentDictionary<string, int> Nomes { get; set; } = new ConcurrentDictionary<string, int>();

        // Fix for CS0236: Move the initialization of the `Converters` field to the constructor, as it cannot reference a non-static method during field initialization.

        private ConcurrentDictionary<Type, Func<object, IFormatProvider, object>> Converters;

        /// <summary>
        /// Configura os parâmetros internos da instância atual com base nos valores já fornecidos de outro parametros.
        /// </summary>
        /// <param name="parametros">Instância de <see cref="ITaskParametros"/> contendo os valores a serem configurados.</param>
        private void SetaParametros(ITaskParametros parametros)
        {
            // Inicializa metadados internos para compatibilidade
            ArmazenaIdTask(parametros.RetornoIdTask());
            ArmazenaNomeTask(parametros.RetornoNomeTask());
            ArmazenaStatusTask(parametros.RetornoStatusTask());
            ArmazenaCancellationToken(parametros.RetornoCancellationTokenManager());
            ArmazenaTaskCreationOptions(parametros.RetornoTaskCreationOptions());
            ArmazenaTimeOutMilliseconds(parametros.RetornoTimeOutMilliseconds());
            ArmazenaTasksGrupo(parametros.RetornoTasksGrupo());
            ArmazenaNomeClasseChamou(parametros.RetornoNomeClasseChamou());

            var cancel = parametros.RetornoCancellationTokenManager();
            var teste = cancel.IsCancellationRequested;
        }

        /// <summary>
        /// Configura a quantidade de parâmetros permitidos para a instância atual.
        /// </summary>
        /// <param name="quantidadeParametros">Número de parâmetros a ser configurado. Deve ser maior ou igual a 1.</param>
        /// <exception cref="ArgumentException">Lançada quando o número de parâmetros é menor que 1.</exception>
        private void SetaQuantidadeParametros(int quantidadeParametros)
        {
            if (quantidadeParametros < 1)
                throw new ArgumentException(
                    $"Número de parâmetros deve ser >= 1 (definido em {this.NomeClasseChamou}).");

            this.QuantidadeParametros = quantidadeParametros;

            // Initialize the Converters field here
            this.Converters = new ConcurrentDictionary<Type, Func<object, IFormatProvider, object>>(InitializeConverters());

            this.TimeoutMilliseconds = Timeout.Infinite;

        }

        /// <summary>
        /// Configura o nome da classe e método que chamou a instância atual.
        /// </summary>
        /// <param name="sourceFilePath">Caminho completo do arquivo fonte do chamador. Este parâmetro é preenchido automaticamente pelo compilador.</param>
        /// <param name="memberName">Nome do membro chamador. Este parâmetro é preenchido automaticamente pelo compilador.</param>
        private void SetaNomeClasseChamou(string sourceFilePath = "", string memberName = "")
        {
            var classe = Path.GetFileNameWithoutExtension(sourceFilePath);
            this.NomeClasseChamou = $"{classe}.{memberName}";
        }

        public TaskReturnValue(
            int quantidadeParametros,
            ITaskParametros parametros,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            SetaQuantidadeParametros(quantidadeParametros);
            SetaParametros(parametros);
            SetaNomeClasseChamou(sourceFilePath, memberName);
        }

        public TaskReturnValue(
            int quantidadeParametros,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            SetaQuantidadeParametros(quantidadeParametros);
            SetaNomeClasseChamou(sourceFilePath, memberName);
        }

        public TaskReturnValue(
            ITaskParametros parametros,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            SetaQuantidadeParametros(1);
            SetaParametros(parametros);
            SetaNomeClasseChamou(sourceFilePath, memberName);
        }

        public TaskReturnValue(
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
        {
            SetaQuantidadeParametros(1);
            SetaNomeClasseChamou(sourceFilePath, memberName);
        }

        // --- Fluent API para metadados ---
        /// <summary>
        /// Define o identificador da task.
        /// </summary>
        /// <param name="id">Identificador numérico.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaIdTask(int id) => this.IdTask = id;

        /// <summary>
        /// Define o nome da task.
        /// </summary>
        /// <param name="nome">Nome textual.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaNomeTask(string nome) => NomeTask = nome ?? string.Empty;

        /// <summary>
        /// Define o estado atual da task.
        /// </summary>
        /// <param name="status">Estado da task.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaStatusTask(TaskState status) => StatusTask = status;

        /// <summary>
        /// Define o token de cancelamento.
        /// </summary>
        /// <param name="token">Token de cancelamento.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaCancellationToken(CancellationToken token) => CancellationController = token;

        /// <summary>
        /// Define a fonte de token de cancelamento.
        /// </summary>
        /// <param name="cts">Fonte de token de cancelamento.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaCancellationToken(CancellationTokenSource cts) => this.CancellationController = cts;

        /// <summary>
        /// Define o gerenciador de token de cancelamento customizado.
        /// </summary>
        /// <param name="mgr">Gerenciador de token.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaCancellationToken(CancellationTokenManager mgr) => CancellationController = mgr;

        /// <summary>
        /// Define as opções de criação da task.
        /// </summary>
        /// <param name="options">Opções de <see cref="TaskCreationOptions"/>.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaTaskCreationOptions(TaskCreationOptions options) => TaskCreationOptions = options;

        /// <summary>
        /// Define o timeout em milissegundos.
        /// </summary>
        /// <param name="ms">Milissegundos de timeout.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaTimeOutMilliseconds(int ms) => TimeoutMilliseconds = ms;

        /// <summary>
        /// Define o timeout via <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="timeout">Intervalo de tempo.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public override void ArmazenaTimeOutTimeSpan(TimeSpan timeout) => TimeoutMilliseconds = (int)timeout.TotalMilliseconds;

        /// <summary>
        /// Armazena o nome da classe que chamou a instância atual.
        /// </summary>
        /// <param name="nomeClasseChamou">O nome completo da classe chamadora.</param>
        public override void ArmazenaNomeClasseChamou(string nomeClasseChamou) => this.NomeClasseChamou = nomeClasseChamou;

        /// <summary>  
        /// Armazena o grupo de tarefas associado à instância atual.  
        /// </summary>  
        /// <param name="grupoTasks">O grupo de tarefas a ser atribuído.</param>  
        public override void ArmazenaTasksGrupo(TasksGrupos grupoTasks) => this.GrupoTasks = grupoTasks;

        /// <summary>
        /// Armazena um valor do tipo void no conjunto de parâmetros.
        /// </summary>
        /// <remarks>
        /// Este método é utilizado para registrar explicitamente que o retorno de uma tarefa não possui valor (void).
        /// </remarks>
        public override void ArmazenaVoid()
        {
            Armazena(typeof(void), "void");
        }

        // --- Armazenamento genérico de parâmetros ---
        /// <inheritdoc />
        public override void Armazena<T>(T value, string nome = null)
        {
            int id = this.Nomes.TryGetValue(nome, out var existing)
                     ? existing
                     : Interlocked.Increment(ref Sequencia);

            if (id > this.QuantidadeParametros)
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    $"{id} está fora da quantidade de parâmetros ({this.QuantidadeParametros}) definida em {this.NomeClasseChamou}.");

            Armazena(value, id, nome);
        }

        /// <inheritdoc />
        public override void Armazena<T>(T value, int parametro, string nome = null)
        {
            if (parametro < 1 || parametro > QuantidadeParametros)
                throw new ArgumentOutOfRangeException(
                    nameof(parametro),
                    $"Número {parametro} está fora do intervalo (1–{QuantidadeParametros}) definido em {NomeClasseChamou}.");

            ArmazenaInterno(value, parametro, nome);
        }

        /// <summary>
        /// Tenta adicionar entrada nula para o parâmetro.
        /// </summary>
        /// <param name="parametro">Índice do parâmetro.</param>
        protected bool ArmazenaInternoNulo(int parametro)
        {
            var ok = TipoTryAddOrUpdate(parametro, typeof(object));
            ok &= Valor.TryAdd(parametro, null);
            return ok;
        }

        /// <summary>
        /// Lógica interna comum de armazenamento de valor e tipo.
        /// </summary>
        /// <typeparam name="T">Tipo do valor.</typeparam>
        /// <param name="value">Valor a armazenar (pode ser null).</param>
        /// <param name="parametro">Índice do parâmetro.</param>
        /// <param name="nome">Nome opcional para indexação.</param>
        protected void ArmazenaInterno<T>(T value, int parametro, string nome = null)
        {
            var isNull = value == null;
            var addedNull = isNull && ArmazenaInternoNulo(parametro);
            var tipo = typeof(T);
            var typeOk = !addedNull && TipoTryAddOrUpdate(parametro, tipo);
            var valueOk = typeOk && ValorTryAddOrUpdate(parametro, value);

            if (!string.IsNullOrEmpty(nome))
                Nomes.TryAdd(nome, parametro);
        }

        /// <summary>
        /// Tenta adicionar ou atualizar o valor armazenado.
        /// </summary>
        /// <param name="parametro">Índice do parâmetro.</param>
        /// <param name="objeto">Valor a armazenar.</param>
        protected bool ValorTryAddOrUpdate(int parametro, object objeto)
        {
            Valor.AddOrUpdate(parametro, objeto, (key, old) => objeto);
            return Valor.TryGetValue(parametro, out var atual) && Equals(atual, objeto);
        }

        /// <summary>
        /// Tenta adicionar ou atualizar o tipo associado.
        /// </summary>
        /// <param name="parametro">Índice do parâmetro.</param>
        /// <param name="tipo">Tipo do valor.</param>               
        protected bool TipoTryAddOrUpdate(int parametro, Type tipo)
        {
            Tipo.AddOrUpdate(parametro, tipo, (key, old) => tipo);
            return Tipo.TryGetValue(parametro, out var atual) && atual == tipo;
        }

        // --- Recuperação de valores ---

        public override int RetornoIdTask() => this.IdTask;

        /// <inheritdoc />
        public override string RetornoNomeTask() => this.NomeTask;

        /// <inheritdoc />
        public override Bibliotecas.TaskCore.TaskState RetornoStatusTask() => this.StatusTask;

        /// <inheritdoc />
        public override TaskCreationOptions RetornoTaskCreationOptions() => this.TaskCreationOptions;

        /// <inheritdoc />
        public override int RetornoTimeOutMilliseconds() => this.TimeoutMilliseconds;

        /// <inheritdoc />
        public override CancellationTokenManager RetornoCancellationTokenManager() => CancellationController as CancellationTokenManager;

        /// <inheritdoc />
        public override CancellationTokenSource RetornoCancellationTokenSource() => CancellationController as CancellationTokenSource;

        /// <inheritdoc />
        public override string RetornoNomeClasseChamou() => NomeClasseChamou as string;

        /// <inheritdoc />
        public override TasksGrupos RetornoTasksGrupo() => this.GrupoTasks;

        /// <inheritdoc />
        public override object Retorno()
        {
            if (QuantidadeParametros > 1)
                throw new ArgumentException(
                    "Quando há mais de um parâmetro é preciso informar qual índice retornar.");
            return Retorno(1);
        }

        /// <inheritdoc />
        public override object Retorno(int parametro)
        {
            if (!Valor.TryGetValue(parametro, out var raw))
                throw new KeyNotFoundException(
                    $"Parâmetro {parametro} não encontrado em {NomeClasseChamou}.");

            if (raw == null)
                return null;

            if (!Tipo.TryGetValue(parametro, out var tipo))
                throw new KeyNotFoundException(
                    $"Tipo do parâmetro {parametro} não encontrado em {NomeClasseChamou}.");

            return ConvertValue(raw, tipo);
        }

        /// <inheritdoc />
        public override object this[int parametro] => Retorno(parametro);

        /// <inheritdoc />
        public override object this[string nome]
        {
            get
            {
                if (Nomes.TryGetValue(nome, out var idx))
                    return Retorno(idx);
                throw new KeyNotFoundException(
                    $"Parâmetro com nome '{nome}' não encontrado em {NomeClasseChamou}.");
            }
        }

        // --- Conversão de valores com cache pré-inicializado ---
        /// <summary>
        /// Converte o valor bruto para o tipo alvo utilizando o cache de conversores.
        /// </summary>
        /// <param name="value">Valor bruto armazenado.</param>
        /// <param name="targetType">Tipo de destino para conversão.</param>
        /// <returns>Objeto convertido.</returns>
        private object ConvertValue(object value, Type targetType)
        {
            if (Converters.TryGetValue(targetType, out var conv))
                return conv(value, Thread.CurrentThread.CurrentCulture);

            var converter = CreateConverter(targetType);
            Converters.TryAdd(targetType, converter);
            return converter(value, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Inicializa mapeamento de conversores para tipos primitivos e Nullable&lt;T&gt;.
        /// </summary>
        /// <returns>Dicionário de conversores padrão.</returns>
        private IDictionary<Type, Func<object, IFormatProvider, object>> InitializeConverters()
        {
            var d = new Dictionary<Type, Func<object, IFormatProvider, object>>
            {
                { typeof(short),   (v,p) => Convert.ToInt16(v,p) },
                { typeof(int),     (v,p) => Convert.ToInt32(v,p) },
                { typeof(long),    (v,p) => Convert.ToInt64(v,p) },
                { typeof(ushort),  (v,p) => Convert.ToUInt16(v,p) },
                { typeof(uint),    (v,p) => Convert.ToUInt32(v,p) },
                { typeof(ulong),   (v,p) => Convert.ToUInt64(v,p) },
                { typeof(float),   (v,p) => Convert.ToSingle(v,p) },
                { typeof(double),  (v,p) => Convert.ToDouble(v,p) },
                { typeof(decimal), (v,p) => Convert.ToDecimal(v,p) },
                { typeof(string),  (v,p) => Convert.ToString(v,p) },
                { typeof(char),    (v,p) => Convert.ToChar(v,p) },
                { typeof(bool),    (v,p) => Convert.ToBoolean(v,p) },
                { typeof(byte),    (v,p) => Convert.ToByte(v,p) },
                { typeof(sbyte),   (v,p) => Convert.ToSByte(v,p) },
                { typeof(DateTime),(v,p) => Convert.ToDateTime(v,p) },
                { typeof(Guid),    (v,p) => v is Guid g ? g : Guid.Parse(v.ToString()) },
                // Nullable<T> equivalents
                { typeof(short?),   (v,p) => v==null? (short?)null : Convert.ToInt16(v,p) },
                { typeof(int?),     (v,p) => v==null? (int?)null   : Convert.ToInt32(v,p) },
                { typeof(long?),    (v,p) => v==null? (long?)null  : Convert.ToInt64(v,p) },
                { typeof(ushort?),  (v,p) => v==null? (ushort?)null: Convert.ToUInt16(v,p) },
                { typeof(uint?),    (v,p) => v==null? (uint?)null  : Convert.ToUInt32(v,p) },
                { typeof(ulong?),   (v,p) => v==null? (ulong?)null : Convert.ToUInt64(v,p) },
                { typeof(float?),   (v,p) => v==null? (float?)null : Convert.ToSingle(v,p) },
                { typeof(double?),  (v,p) => v==null? (double?)null: Convert.ToDouble(v,p) },
                { typeof(decimal?), (v,p) => v==null? (decimal?)null: Convert.ToDecimal(v,p) },
                { typeof(char?),    (v,p) => v==null? (char?)null  : Convert.ToChar(v,p) },
                { typeof(bool?),    (v,p) => v==null? (bool?)null : Convert.ToBoolean(v,p) },
                { typeof(byte?),    (v,p) => v==null? (byte?)null : Convert.ToByte(v,p) },
                { typeof(sbyte?),   (v,p) => v==null? (sbyte?)null: Convert.ToSByte(v,p) },
                { typeof(DateTime?),(v,p) => v==null? (DateTime?)null: Convert.ToDateTime(v,p) },
                { typeof(Guid?),    (v,p) => v==null? (Guid?)null  : (v is Guid gg? gg : Guid.Parse(v.ToString())) },
                { typeof(object),  (v,p) => v },
                { typeof(void),    (v,p) => null }
            };
            return d;
        }

        /// <summary>
        /// Fallback para conversão de tipos não primitivos e customizados.
        /// </summary>
        /// <param name="type">Tipo de destino.</param>
        /// <returns>Função de conversão para o tipo especificado.</returns>
        private Func<object, IFormatProvider, object> CreateConverter(Type type)
        {
            // Nullable<T>
            var underlying = Nullable.GetUnderlyingType(type);
            if (underlying != null)
            {
                var inner = CreateConverter(underlying);
                return (v, p) => v == null ? null : inner(v, p);
            }
            // IConvertible
            if (typeof(IConvertible).IsAssignableFrom(type))
            {
                return (v, p) => Convert.ChangeType(v, type, p);
            }
            // Enums
            if (type.IsEnum)
            {
                return (v, p) => Enum.Parse(type, v.ToString(), true);
            }
            // Guid
            if (type == typeof(Guid))
            {
                return (v, p) => v is Guid g ? g : Guid.Parse(v.ToString());
            }
            // Casos específicos: cancelamento e opções de task
            if (typeof(CancellationTokenManager).IsAssignableFrom(type) ||
                typeof(CancellationTokenSource).IsAssignableFrom(type) ||
                type == typeof(CancellationToken) ||
                type == typeof(TaskCreationOptions))
            {
                return (v, p) => v;
            }

            var returning = CreateConverterContinue(type);
            return (v, p) => returning(v, p);

            // Fallback: lança se não suportado
            //return (v, p) => throw new ArgumentException($"Tipo não suportado: {type.Name}");
        }
    }

}
