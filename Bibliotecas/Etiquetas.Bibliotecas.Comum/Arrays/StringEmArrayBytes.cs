using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Converte uma string em um array de bytes utilizando a codificação especificada.
    /// </summary>
    public static class StringEmArrayBytes
    {
        /// <summary>
        /// Converte uma string em um array de bytes utilizando a codificação ASCII.
        /// </summary>
        /// <param name="texto">string</param>
        /// <returns>retorno byte em ASCII.</returns>
        public static byte[] ExecuteASCII(string texto)
        {
            return Encoding.ASCII.GetBytes(texto);
        }

        /// <summary>
        /// Converte uma string em um array de bytes utilizando a codificação UTF8.
        /// </summary>
        /// <param name="texto">string </param>
        /// <returns></returns>
        public static byte[] ExecuteUTF8(string texto)
        {
            return Encoding.UTF8.GetBytes(texto);
        }

        public static byte[] Execute(Encoding encoder, string texto)
        {
           return encoder.GetBytes(texto);
        }
    }
}
