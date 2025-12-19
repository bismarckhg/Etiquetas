using System;

namespace Etiquetas.Application.Pipeline.Messages
{
    /// <summary>
    /// Mensagem contendo pacote de bytes recebido da impressora Sato.
    /// </summary>
    public class PacoteSatoMensagem : EtiquetaMensagemBase
    {
        /// <summary>
        /// Gets or sets - Array de bytes recebido da impressora.
        /// </summary>
        public byte[] PacoteBytes { get; set; }

        /// <summary>
        /// Gets or sets - Número de bytes recebidos.
        /// </summary>
        public int TamanhoRecebido { get; set; }

        /// <summary>
        /// Gets or sets - Origem do pacote (porta TCP, etc).
        /// </summary>
        public string Origem { get; set; }
    }
}
