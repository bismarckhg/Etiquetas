using System;
using Etiquetas.Core.Interfaces;
using LiteDB;

namespace Etiquetas.Domain.Entities
{
    /// <summary>
    /// Entidade que representa uma Falta de Impressão.
    /// </summary>
    public class FaltaImprimir : IFaltaImprimir
    {
        /// <inheritdoc/>
        [BsonId]
        public string Id { get; set; }

        /// <inheritdoc/>
        public string IdEtiquetaImpressao { get; set; }

        /// <inheritdoc/>
        public string NomeDoJOB { get; set; }

        /// <inheritdoc/>
        public DateTime DataImpressao { get; set; } = DateTime.Now;

        /// <inheritdoc/>
        public byte StatusImpressora { get; set; }

        /// <inheritdoc/>
        public long FaltaImpressao { get; set; }

        /// <inheritdoc/>
        public bool IsOnline { get; set; }

        /// <inheritdoc/>
        public bool IsError { get; set; }

        /// <inheritdoc/>
        public string State { get; set; }
    }
}
