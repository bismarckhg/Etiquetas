using System;

namespace Etiquetas.Application.Pipeline.Messages
{
    /// <summary>
    /// Mensagem para fechar uma etiqueta de impressão.
    /// </summary>
    public class FecharEtiquetaMensagem : EtiquetaMensagemBase
    {
        /// <summary>
        /// Gets or sets - ID da etiqueta a ser fechada.
        /// </summary>
        public long IdEtiqueta { get; set; }

        /// <summary>
        /// Gets or sets - Nome do JOB.
        /// </summary>
        public string JobName { get; set; }
    }
}
