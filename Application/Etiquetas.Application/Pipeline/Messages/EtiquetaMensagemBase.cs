using System;

namespace Etiquetas.Application.Pipeline.Messages
{
    /// <summary>
    /// Mensagem base para operações do pipeline de etiquetas.
    /// </summary>
    public abstract class EtiquetaMensagemBase
    {
        /// <summary>
        /// Gets or sets - Identificador único da mensagem.
        /// </summary>
        public string MessageId { get; set; } = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Gets or sets - Timestamp da mensagem.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
