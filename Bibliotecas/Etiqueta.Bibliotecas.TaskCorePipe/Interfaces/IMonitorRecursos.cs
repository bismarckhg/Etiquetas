using Etiqueta.Bibliotecas.TaskCorePipe.Modelos;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Interfaces
{
    /// <summary>
    /// Define o contrato para um monitor de recursos do sistema ou de uma tarefa.
    /// </summary>
    public interface IMonitorRecursos
    {
        /// <summary>
        /// Obtém as métricas de performance atuais.
        /// </summary>
        /// <returns>Um objeto contendo as métricas de performance.</returns>
        Task<MetricasPerformance> ObterMetricasAsync();
    }
}
