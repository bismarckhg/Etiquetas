using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etiqueta.Application.DTOs;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Resolve o Id da etiqueta baseado na informação da Impressora SATO.
    /// </summary>
    public interface IEtiquetaIdResolver
    {
        /// <summary>
        /// Resuelve el Id de la etiqueta basado en la información del Sato.
        /// </summary>
        /// <param name="sato">Sato DTO dos dados recebidos da impressora SATO</param>
        /// <returns>Retorna o Id da Etiqueta</returns>
        Task<long?> ResolveEtiquetaIdAsync(ISatoDto sato);
    }
}
