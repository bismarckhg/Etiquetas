using Etiqueta.Bibliotecas.TaskCorePipe.Enums;
using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Modelos
{
    /// <summary>
    /// Representa a estrutura base de uma mensagem trocada via pipe.
    /// </summary>
    public class MensagemPipe
    {
        /// <summary>
        /// Identificador único da mensagem (GUID).
        /// </summary>
        public Guid IdMensagem { get; set; }

        /// <summary>
        /// Tipo da mensagem (Command, Response, etc.).
        /// </summary>
        public TipoMensagem TipoMensagem { get; set; }

        /// <summary>
        /// Timestamp de criação da mensagem.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Identificador do componente de origem da mensagem.
        /// </summary>
        public string IdOrigem { get; set; }

        /// <summary>
        /// Identificador do componente de destino da mensagem.
        /// </summary>
        public string IdDestino { get; set; }

        /// <summary>
        /// Conteúdo (payload) da mensagem, pode ser um comando, uma resposta, etc.
        /// </summary>
        public object Payload { get; set; }

        /// <summary>
        /// Construtor padrão que inicializa o Id e o Timestamp da mensagem.
        /// </summary>
        public MensagemPipe()
        {
            this.IdMensagem = Guid.NewGuid();
            this.Timestamp = DateTime.UtcNow;
        }
    }
}
