using Etiqueta.Bibliotecas.TaskCorePipe.Enums;
using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Interfaces
{
    /// <summary>
    /// Define o contrato para informações de status de tarefas.
    /// </summary>
    public interface IStatusTarefa
    {
        /// <summary>
        /// Identificador único da tarefa.
        /// </summary>
        Guid IdTarefa { get; }

        /// <summary>
        /// Nome descritivo da tarefa.
        /// </summary>
        string NomeTarefa { get; }

        /// <summary>
        /// O estado atual da tarefa.
        /// </summary>
        StatusTarefa Status { get; }

        /// <summary>
        /// Tempo total de execução da tarefa.
        /// </summary>
        TimeSpan TempoDeExecucao { get; }
    }
}
