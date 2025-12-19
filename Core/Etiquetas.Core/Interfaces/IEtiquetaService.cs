using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Core
{
    /// <summary>
    /// Interface para o serviço de etiquetas.
    /// </summary>
    public interface IEtiquetaService : IDisposable
    {
        /// <summary>
        /// Obtém uma etiqueta de impressão pelo seu ID.
        /// </summary>
        /// <param name="id">Id da etiqueta a ser selecionada.</param>
        /// <returns>Retorna a etqueta selecionada pelo Id.</returns>
        Task<IEtiquetaImpressao> ObterEtiquetaImpressaoPorIdAsync(long id);

        /// <summary>
        /// Obtém todas as etiquetas de impressão.
        /// </summary>
        /// <returns>Retorna coleção das etiquetas de impressao da tabela.</returns>
        Task<IEnumerable<IEtiquetaImpressao>> ObterTodasEtiquetaImpressaoAsync();

        /// <summary>
        /// Obtém etiquetas de impressão dentro de um período específico.
        /// </summary>
        /// <param name="inicio">Periodo Inicial das Etiquetas.</param>
        /// <param name="fim">Periodo Final das Etiquetas.</param>
        /// <returns>Return Coleção de etqiuetas de impressão.</returns>
        Task<IEnumerable<IEtiquetaImpressao>> ObterEtiquetaImpressaoPorPeriodoAsync(DateTime inicio, DateTime fim);

        /// <summary>
        /// Obtém todas as faltas de impressão associadas a uma etiqueta de impressão pelo ID da etiqueta.
        /// </summary>
        /// <param name="id">Id etiqueta impressão para obter todas as Falta Imprimir.</param>
        /// <returns>Retorna coleção Falta Imprimir.</returns>
        Task<IEnumerable<IFaltaImprimir>> ObterTodasFaltaImprimirDeEtiquetaImpressaoPorIdEtiquetaImpressaoAsync(string id);
    }
}
