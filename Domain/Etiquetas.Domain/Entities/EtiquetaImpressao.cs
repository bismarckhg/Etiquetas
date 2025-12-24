using System;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Domain.Entities
{
    /// <summary>
    /// Entidade que representa uma Etiqueta de Impressão.
    /// </summary>
    public class EtiquetaImpressao : IEtiquetaImpressao
    {
        // BsonIdAttribute está correto, pois o atributo é referenciado como [BsonId]
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
        public long CodigoMaterial { get; set; }

        /// <inheritdoc/>
        public DateTime Validade { get; set; }

        /// <inheritdoc/>
        public string Lote { get; set; }

        /// <inheritdoc/>
        public string CodigoUsuario { get; set; }

        /// <inheritdoc/>
        public string CodigoBarras { get; set; }

        /// <inheritdoc/>
        public DateTime DataHoraInicio { get; set; } = DateTime.Now;

        /// <inheritdoc/>
        public DateTime DataHoraFim { get; set; } = DateTime.Now;

        /// <inheritdoc/>
        public char StatusEtiqueta { get; set; }

        /// <inheritdoc/>
        public long QuantidadeSolicitada { get; set; }

        /// <inheritdoc/>
        public long FaltaImpressao { get; set; }

        /// <inheritdoc/>
        public string JobName { get; set; }
    }
}
