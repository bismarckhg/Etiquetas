using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Core.Constants
{
    /// <summary>
    /// Constantes padrão utilizadas na aplicação.
    /// </summary>
    public static class PadraoConstantes
    {
        /// <summary>
        /// Representa o caractere de controle ASCII - Start of Text (constSTX), com valor = 2.
        /// </summary>
        public const byte ConstSTX = 2;

        /// <summary>
        /// Representa o caractere de controle ASCII - End of Text (constETX), com valore = 3.
        /// </summary>
        public const byte ConstETX = 3;

        /// <summary>
        /// Representa o ASCII control character constENQ (Enquiry), com o valor de 5.
        /// </summary>
        public const byte ConstENQ = 5;

        /// <summary>
        /// Representa o tamanho do protocolo de comunicação da Impressora Sato.
        /// </summary>
        public const int ConstTamanhoProtocolo = 28;
    }
}
