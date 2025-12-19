using System;
using System.Collections.Generic;
using System.Linq;
using Etiqueta.Application.DTOs;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Entities;

namespace Etiqueta.Application.Mappers
{
    /// <summary>
    /// Mapeador de SatoDto para FaltaImprimir.
    /// </summary>
    public static class SatoToFaltaMapper
    {
        /// <summary>
        /// Mapeia uma coleção de ISatoDto para uma coleção de IFaltaImprimir.
        /// </summary>
        /// <param name="satoList">Coleção de SatoDto.</param>
        /// <returns>retorna Coleção de Entidade de Falta Imprimir.</returns>
        public static IEnumerable<IFaltaImprimir> Map(IEnumerable<ISatoDto> satoList)
        {
            if (satoList == null)
            {
                yield break;
            }

            foreach (var satoDto in satoList)
            {
                var f = new FaltaImprimir
                {
                    Id = Guid.NewGuid().ToString("N"),
                    IdEtiquetaImpressao = satoDto.JobId,
                    NomeDoJOB = satoDto.JobName,
                    DataImpressao = DateTime.UtcNow,
                    StatusImpressora = satoDto.Status,
                    FaltaImpressao = satoDto.NumeroFaltaImprimir,
                };
                yield return f;
            }
        }
    }
}
