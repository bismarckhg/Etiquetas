using System;
using Etiquetas.Core.Interfaces;
using LiteDB;

namespace Etiquetas.Domain.Entities
{
    /// <summary>
    /// Entidade que representa uma Etiqueta de Impressão.
    /// </summary>
    public class EtiquetaImpressao : IEtiquetaImpressao
    {
        /// <inheritdoc/>
        [BsonId]
        public long Id { get; set; }

        /// <inheritdoc/>
        public string DescricaoMedicamento { get; set; }

        /// <inheritdoc/>
        public string PrincipioAtivo1 { get; set; }

        /// <inheritdoc/>
        public string PrincipioAtivo2 { get; set; }

        /// <inheritdoc/>
        public long CodigoMaterial { get; set; }

        /// <inheritdoc/>
        public DateTime Validade { get; set; }

        /// <inheritdoc/>
        public string Lote { get; set; }

        /// <inheritdoc/>
        public string MatriculaFuncionario { get; set; }

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
