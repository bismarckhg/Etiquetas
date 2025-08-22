using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Modelos
{
    /// <summary>
    /// Contém metadados sobre o desempenho e a execução de uma tarefa.
    /// </summary>
    public class MetadadosTarefa
    {
        /// <summary>
        /// Identificador único da tarefa (GUID).
        /// </summary>
        public Guid IdTarefa { get; set; }

        /// <summary>
        /// Timestamp de criação e início de execução.
        /// </summary>
        public DateTime TimestampInicio { get; set; }

        /// <summary>
        /// Tempo total de CPU utilizado pela tarefa.
        /// </summary>
        public TimeSpan TempoDeCpu { get; set; }

        /// <summary>
        /// Pico de utilização de memória (em bytes).
        /// </summary>
        public long PicoMemoriaUsada { get; set; }

        /// <summary>
        /// Número de comandos processados pela tarefa.
        /// </summary>
        public long ComandosProcessados { get; set; }

        /// <summary>
        /// Última resposta a um comando PING.
        /// </summary>
        public DateTime UltimaRespostaPing { get; set; }
    }
}
