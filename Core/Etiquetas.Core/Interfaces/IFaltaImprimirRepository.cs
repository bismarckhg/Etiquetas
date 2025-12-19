using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Core
{
    /// <summary>
    /// Repositório para operações CRUD de Falta Imprimir.
    /// </summary>
    public interface IFaltaImprimirRepository : IDisposable
    {
        /// <summary>
        /// Obtém uma etiqueta de impressão pelo seu Id.
        /// </summary>
        /// <param name="id">id de uma etiqueta de impressão.</param>
        /// <returns>Retorna a entidade do id da etiqueta de impressão.</returns>
        Task<IFaltaImprimir> GetByIdAsync(long id);

        /// <summary>
        /// Obtém todas a colleção de Falta Imprimir vinculado ao Id de uma etiqueta de impressão.
        /// </summary>
        /// <param name="idEtiqueta">Id de uma etiqueta de impressão.</param>
        /// <returns>retorna coleção de Falta Imprimir de uma etiqueta de impressão.</returns>
        Task<IEnumerable<IFaltaImprimir>> GetByEtiquetaImpressaoIdAsync(string idEtiqueta);

        /// <summary>
        /// Insere um nova entidade Falta Imprimir.
        /// </summary>
        /// <param name="faltaImprimir">entidade falta imprimir.</param>
        /// <returns>return retorna uma tarefa TASK.</returns>
        Task InsertAsync(IFaltaImprimir faltaImprimir);
    }
}
