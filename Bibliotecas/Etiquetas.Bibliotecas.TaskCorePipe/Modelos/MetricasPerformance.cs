using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Modelos
{
    /// <summary>
    /// Coleta métricas de desempenho para um componente ou sistema.
    /// </summary>
    public class MetricasPerformance
    {
        /// <summary>
        /// Utilização média de CPU (em porcentagem).
        /// </summary>
        public double CpuUsoMedio { get; set; }

        /// <summary>
        /// Memória de trabalho atual (em bytes).
        /// </summary>
        public long MemoriaDeTrabalho { get; set; }

        /// <summary>
        /// Número de threads ativas.
        /// </summary>
        public int ThreadsAtivas { get; set; }

        /// <summary>
        /// Número de handles abertos pelo processo.
        /// </summary>
        public int HandlesAbertos { get; set; }

        /// <summary>
        /// Total de bytes lidos do pipe.
        /// </summary>
        public long BytesLidosPipe { get; set; }

        /// <summary>
        /// Total de bytes escritos no pipe.
        /// </summary>
        public long BytesEscritosPipe { get; set; }
    }
}
