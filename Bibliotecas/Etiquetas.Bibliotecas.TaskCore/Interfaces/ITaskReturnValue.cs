using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TaskCore.Interfaces
{
    /// <summary>
    /// Define o contrato para armazenamento e recuperação de parâmetros e valores de retorno de uma tarefa.
    /// </summary>
    public abstract class ITaskReturnValue
    {

        /// <summary>Nome completo da classe e método que criou a instância.</summary>
        public abstract string NomeClasseChamou { get; set; }

        /// <summary>Identificador numérico da task.</summary>
        public abstract int IdTask { get; set; }

        /// <summary>Nome descritivo da task.</summary>
        public abstract string NomeTask { get; set; }

        /// <summary>Estado atual da task.</summary>
        public abstract TaskState StatusTask { get; set; }

        /// <summary>Controlador de cancelamento (Token ou Source).</summary>
        public abstract object CancellationController { get; set; }

        /// <summary>Definição de Encoding para Textos.</summary>
        public abstract Encoding EncodingTexto { get; set; }

        /// <summary>Opções de criação da <see cref="Task"/>.</summary>
        public abstract TaskCreationOptions TaskCreationOptions { get; set; }
        public abstract TasksGrupos GrupoTasks { get; set; }

        /// <summary>
        /// Obtém o número total de parâmetros/valores esperados.
        /// </summary>
        public abstract int TotalParametros { get; }

        /// <summary>
        /// Dicionário que mapeia o índice do parâmetro para o seu tipo.
        /// </summary>
        public abstract ConcurrentDictionary<int, Type> Tipo { get; }

        /// <summary>
        /// Dicionário que armazena o valor bruto de cada parâmetro pelo seu índice.
        /// </summary>
        public abstract ConcurrentDictionary<int, object> Valor { get; }

        /// <summary>
        /// Dicionário que mapeia nomes simbólicos de parâmetros para seus índices.
        /// </summary>
        public abstract ConcurrentDictionary<string, int> Nomes { get; set; }

        /// <summary>
        /// Armazena um valor do tipo void no conjunto de parâmetros.
        /// </summary>
        /// <remarks>
        /// Este método é utilizado para registrar explicitamente que o retorno de uma tarefa não possui valor (void).
        /// </remarks>
        public abstract void ArmazenaVoid();

        /// <summary>
        /// Define o tempo de espera máximo antes de ocorrer timeout, em <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="timeout">Intervalo de tempo antes do timeout.</param>
        public abstract void ArmazenaTimeOutTimeSpan(TimeSpan timeout);

        /// <summary>
        /// Define o tempo de espera máximo antes de ocorrer timeout, em milissegundos.
        /// </summary>
        /// <param name="timeout">Tempo de timeout em milissegundos.</param>
        public abstract void ArmazenaTimeOutMilliseconds(int timeout);

        /// <summary>
        /// Armazena as opções de criação de <see cref="Task"/>.
        /// </summary>
        /// <param name="options">Opções de criação de tarefa.</param>
        public abstract void ArmazenaTaskCreationOptions(TaskCreationOptions options);

        /// <summary>
        /// Armazena um token de cancelamento para a tarefa.
        /// </summary>
        /// <param name="token">Token de cancelamento.</param>
        public abstract void ArmazenaCancellationToken(CancellationToken token);

        /// <summary>
        /// Armazena uma fonte de token de cancelamento para a tarefa.
        /// </summary>
        /// <param name="cancelToken">Fonte de token de cancelamento.</param>
        public abstract void ArmazenaCancellationToken(CancellationTokenSource cancelToken);

        /// <summary>
        /// Define a fonte de token de cancelamento.
        /// </summary>
        /// <param name="encoding">Fonte de token de cancelamento.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public abstract void ArmazenaEncoding(Encoding encoding);

        /// <summary>
        /// Atribui um nome simbólico à tarefa.
        /// </summary>
        /// <param name="nomeTask">Nome textual da tarefa.</param>
        public abstract void ArmazenaNomeTask(string nomeTask);

        /// <summary>
        /// Define o estado atual da task.
        /// </summary>
        /// <param name="status">Estado da task.</param>
        /// <returns>Instância corrente para encadeamento.</returns>
        public abstract void ArmazenaStatusTask(Etiquetas.Bibliotecas.TaskCore.TaskState status);

        /// <summary>
        /// Armazena o nome da classe que chamou a instância atual.
        /// </summary>
        /// <param name="nomeClasseChamou">O nome completo da classe chamadora.</param>
        public abstract void ArmazenaNomeClasseChamou(string nomeClasseChamou);

        /// <summary>  
        /// Armazena o grupo de tarefas associado à instância atual.  
        /// </summary>  
        /// <param name="grupoTasks">O grupo de tarefas a ser atribuído.</param>  
        public abstract void ArmazenaTasksGrupo(TasksGrupos grupoTasks);

        /// <summary>
        /// Atribui um identificador numérico à tarefa.
        /// </summary>
        /// <param name="id">Índice numérico da tarefa.</param>
        public abstract void ArmazenaIdTask(int id);

        /// <summary>
        /// Armazena um valor genérico em sequência automática, com nome opcional.
        /// </summary>
        /// <typeparam name="T">Tipo do valor.</typeparam>
        /// <param name="value">Valor a armazenar.</param>
        /// <param name="nome">Nome opcional para indexação do valor.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se a sequência ultrapassar a quantidade de parâmetros definida.
        /// </exception>
        public abstract void Armazena<T>(T value, string nome = null);

        /// <summary>
        /// Armazena um valor genérico em posição específica, com nome opcional.
        /// </summary>
        /// <typeparam name="T">Tipo do valor.</typeparam>
        /// <param name="value">Valor a armazenar.</param>
        /// <param name="parametro">Índice (1-based) do parâmetro.</param>
        /// <param name="nome">Nome opcional para indexação do valor.</param>
        public abstract void Armazena<T>(T value, int parametro, string nome = null);

        /// <summary>
        /// Recupera o valor associado ao identificador da tarefa.
        /// </summary>
        /// <returns>Valor armazenado em "Id".</returns>
        public abstract int RetornoIdTask();

        /// <summary>
        /// Recupera o valor associado ao nome da tarefa.
        /// </summary>
        /// <returns>Valor armazenado em "NomeTask".</returns>
        public abstract string RetornoNomeTask();

        /// <summary>
        /// retorna o estado atual da tarefa.
        /// </summary> 
        public abstract TaskState RetornoStatusTask();

        /// <summary>
        /// Recupera o token de cancelamento armazenado.
        /// </summary>
        /// <returns>Token de cancelamento.</returns>
        public abstract CancellationToken RetornoCancellationToken { get; }

        /// <summary>
        /// Recupera o Encoding do Texto.
        /// </summary>
        /// <returns>Encoding.</returns>
        public abstract Encoding RetornoEncoding { get; }

        /// <summary>
        /// Recupera o token de cancelamento armazenado.
        /// </summary>
        /// <returns>Token de cancelamento.</returns>
        public abstract CancellationTokenSource RetornoCancellationTokenSource();

        /// <summary>
        /// Recupera as opções de criação de tarefa armazenadas.
        /// </summary>
        /// <returns>Opções de criação de tarefa.</returns>
        public abstract TaskCreationOptions RetornoTaskCreationOptions();

        /// <summary>
        /// Recupera o timeout em milissegundos armazenado.
        /// </summary>
        /// <returns>Timeout em milissegundos.</returns>
        public abstract int RetornoTimeOutMilliseconds();

        /// <summary>
        /// Recupera o nome completo da classe e método que criou a instância.
        /// </summary>
        /// <returns>O nome completo da classe e método que criou a instância.</returns>
        public abstract string RetornoNomeClasseChamou();

        /// <summary>
        /// Recupera o grupo de tarefas associado.
        /// </summary>
        /// <returns>O grupo de tarefas armazenado.</returns>
        public abstract TasksGrupos RetornoTasksGrupo();

        /// <summary>
        /// Recupera o único valor armazenado, assumindo que haja apenas um parâmetro.
        /// </summary>
        /// <returns>Valor armazenado.</returns>
        public abstract object Retorno();

        /// <summary>
        /// Recupera o valor armazenado no índice especificado.
        /// </summary>
        /// <param name="parametro">Índice do parâmetro (1-based).</param>
        /// <returns>Valor armazenado.</returns>
        public abstract object Retorno(int parametro);

        /// <summary>
        /// Recupera o valor armazenado por nome se existir.
        /// </summary>
        /// <param name="nome">Nome simbólico do parâmetro.</param>
        /// <returns>Valor armazenado.</returns>
        public abstract T RetornaSeExistir<T>(string nome);

        /// <summary>
        /// Indexador para recuperar valor por índice.
        /// </summary>
        /// <param name="parametro">Índice do parâmetro.</param>
        /// <returns>Valor armazenado.</returns>
        public abstract object this[int parametro] { get; }

        /// <summary>
        /// Indexador para recuperar valor por nome.
        /// </summary>
        /// <param name="nome">Nome simbólico do parâmetro.</param>
        /// <returns>Valor armazenado.</returns>
        public abstract object this[string nome] { get; }

        /// <summary>
        /// Ponto de extensão para conversores adicionais caso o tipo não seja suportado.
        /// </summary>
        /// <param name="type">Tipo a ser convertido.</param>
        /// <param name="valor">Valor bruto a converter.</param>
        /// <returns>Função de conversão.</returns>
        public virtual Func<object, IFormatProvider, object> CreateConverterContinue(Type type)
        {
            throw new ArgumentException($"Tipo não suportado: {type.Name}.");
        }
    }
}
