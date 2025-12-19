using System;

namespace Etiquetas.Application.Pipeline.Messages
{
    /// <summary>
    /// Resultado do processamento de um pacote Sato individual.
    /// </summary>
    public class PacoteProcessadoMensagem : EtiquetaMensagemBase
    {
        /// <summary>
        /// Gets or sets - DTO Sato processado.
        /// </summary>
        public Etiqueta.Application.DTOs.ISatoDto SatoDto { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o pacote é válido.
        /// </summary>
        public bool EhValido { get; set; }
    }
}
