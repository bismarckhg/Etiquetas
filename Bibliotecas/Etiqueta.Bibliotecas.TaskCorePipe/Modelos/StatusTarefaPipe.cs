using Etiqueta.Bibliotecas.TaskCorePipe.Enums;
using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Modelos
{
    /// <summary>
    /// Contém informações detalhadas sobre o estado atual de uma tarefa.
    /// </summary>
    public class StatusTarefaPipe
    {
        /// <summary>
        /// Identificador único da tarefa (GUID).
        /// </summary>
        public Guid IdTarefa { get; set; }

        /// <summary>
        /// Nome descritivo da tarefa.
        /// </summary>
        public string NomeTarefa { get; set; }

        /// <summary>
        /// O estado atual da tarefa no ciclo de vida.
        /// </summary>
        public StatusTarefa Status { get; set; }

        /// <summary>
        /// Tempo de execução total da tarefa até o momento.
        /// </summary>
        public TimeSpan TempoDeExecucao { get; set; }

        /// <summary>
        /// Timestamp da última atividade registrada para a tarefa.
        /// </summary>
        public DateTime UltimaAtividade { get; set; }

        /// <summary>
        /// Informações adicionais ou metadados sobre a tarefa.
        /// </summary>
        public object Metadados { get; set; }
    }
}
