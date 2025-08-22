using Etiqueta.Bibliotecas.TaskCorePipe.Modelos;
using System;
using System.Collections.Generic;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Comunicacao
{
    /// <summary>
    /// Fornece métodos para validar mensagens do protocolo de pipe.
    /// </summary>
    public class ValidadorMensagem
    {
        /// <summary>
        /// Valida uma MensagemPipe e seu payload.
        /// </summary>
        /// <param name="mensagem">A mensagem a ser validada.</param>
        /// <param name="erros">Uma lista de erros de validação encontrados.</param>
        /// <returns>True se a mensagem for válida, False caso contrário.</returns>
        public bool EhValida(MensagemPipe mensagem, out List<string> erros)
        {
            erros = new List<string>();

            if (mensagem == null)
            {
                erros.Add("A mensagem não pode ser nula.");
                return false;
            }

            if (mensagem.IdMensagem == Guid.Empty)
            {
                erros.Add("O Id da mensagem é inválido.");
            }

            if (string.IsNullOrWhiteSpace(mensagem.IdOrigem))
            {
                erros.Add("O Id de origem não pode ser nulo ou vazio.");
            }

            if (mensagem.Payload == null)
            {
                erros.Add("O payload da mensagem não pode ser nulo.");
            }

            return erros.Count == 0;
        }
    }
}
