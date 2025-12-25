using System;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Application.DTOs
{
    /// <inheritdoc/>
    public class EtiquetaImpressaoDto : IEtiquetaImpressaoDto
    {
        /// <inheritdoc/>
        public long Id { get; set; }

        /// <inheritdoc/>
        public string DescricaoMedicamento { get; set; }

        /// <inheritdoc/>
        public string DescricaoMedicamento2 { get; set; }

        /// <inheritdoc/>
        public string PrincipioAtivo { get; set; }

        /// <inheritdoc/>
        public string PrincipioAtivo2 { get; set; }

        /// <inheritdoc/>
        public string CodigoMaterial { get; set; }

        /// <inheritdoc/>
        public string Validade { get; set; }

        /// <inheritdoc/>
        public string Lote { get; set; }

        /// <inheritdoc/>
        public string Embalagem { get; set; }

        /// <inheritdoc/>
        public string CodigoUsuario { get; set; }

        /// <inheritdoc/>
        public string CodigoBarras { get; set; }

        /// <inheritdoc/>
        public string DataHoraInicio { get; set; }

        /// <inheritdoc/>
        public string DataHoraFim { get; set; }

        /// <inheritdoc/>
        public char StatusEtiqueta { get; set; }

        /// <inheritdoc/>
        public string DescricaoStatus { get; set; }

        /// <inheritdoc/>
        public string QuantidadeSolicitada { get; set; }

        /// <inheritdoc/>
        public string FaltaImpressao { get; set; }

        /// <inheritdoc/>
        public string JobName { get; set; }
    }
}
