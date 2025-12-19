using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Core
{
    /// <summary>
    /// Repositório para operações CRUD de Etiqueta de Impressão.
    /// </summary>
    public interface IEtiquetaImpressaoRepository : IDisposable
    {
        /// <summary>
        /// Obtém uma etiqueta de impressão pelo seu Id.
        /// </summary>
        /// <param name="id">id de uma etiqueta de impressão.</param>
        /// <returns>Retorna a entidade do id da etiqueta de impressão.</returns>
        Task<IEtiquetaImpressao> GetByIdAsync(long id);

        /// <summary>
        /// Obtém etiquetas de impressão dentro de um período específico.
        /// </summary>
        /// <param name="start">Periodo inicial da etiqueta de impressão.</param>
        /// <param name="end">Periodo final da etiqueta de impressão.</param>
        /// <returns>Retorna a Colleção de etiquetas de impressão do periodo informado.</returns>
        Task<IEnumerable<IEtiquetaImpressao>> GetByPeriodAsync(DateTime start, DateTime end);

        /// <summary>
        /// Obtém todas as etiquetas de impressão.
        /// </summary>
        /// <returns>Retorna uma coleção com todas as Etiquetas de impressão da base.</returns>
        Task<IEnumerable<IEtiquetaImpressao>> GetAllAsync();

        /// <summary>
        /// Insere uma nova etiqueta de impressão.
        /// </summary>
        /// <param name="etiqueta">entidade a ser inserida na tabela etiqueta de impressão.</param>
        /// <returns>return retorna uma tarefa TASK e depois o resultado.</returns>
        Task InsertAsync(IEtiquetaImpressao etiqueta);

        /// <summary>
        /// Atualiza uma etiqueta de impressão existente.
        /// </summary>
        /// <param name="etiqueta">entidade a ser atualizada na tabela de etiqueta de impressão.</param>
        /// <returns>return retorna uma tarefa TASK e depois o resultado.</returns>
        Task UpdateAsync(IEtiquetaImpressao etiqueta);

        /// <summary>
        /// Exclui uma etiqueta de impressão pelo seu Id.
        /// </summary>
        /// <param name="id">id da etiqueta de impressão a ser excluida.</param>
        /// <returns>return retorna uma tarefa TASK e depois o resultado.</returns>
        Task DeleteAsync(long id);

        /// <summary>
        /// Obtém uma etiqueta de impressão pelo nome do job.
        /// </summary>
        /// <param name="jobName">informa o job name para busca da etiqueta de impressão.</param>
        /// <returns>retorna uma tarefa TASK e depois o resultado da entidade da Etiqueta de Impressão.</returns>
        Task<IEtiquetaImpressao> FindByJobNameAsync(string jobName);

        /// <summary>
        /// Obtém uma etiqueta de impressão pelo código de barras.
        /// </summary>
        /// <param name="codigoBarras">codigo de barras para procuta das etiquetas de Impressão.</param>
        /// <returns>Retorna etiqueta pelo codigo de barras.</returns>
        Task<IEtiquetaImpressao> FindByCodigoBarrasAsync(string codigoBarras);
    }
}
