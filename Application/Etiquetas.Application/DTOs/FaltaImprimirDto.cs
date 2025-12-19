using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Application.DTOs
{
    /// <inheritdoc/>
    public class FaltaImprimirDto : IFaltaImprimirDto
    {
        /// <inheritdoc/>
        public string Id { get; set; }

        /// <inheritdoc/>
        public string IdEtiquetaImpressao { get; set; }

        /// <inheritdoc/>
        public string NomeDoJOB { get; set; }

        /// <inheritdoc/>
        public string DataImpressao { get; set; }

        /// <inheritdoc/>
        public byte StatusImpressora { get; set; }

        /// <inheritdoc/>
        public string DescricaoStatusImpressora { get; set; }

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
