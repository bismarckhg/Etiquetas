using System;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Converte um array de char em um array de string.
    /// </summary>
    public static class CharArrayEmStringArray
    {
        /// <summary>
        /// Converte um array de char em um array de string.
        /// </summary>
        /// <param name="arrayChar">Array de char a serr convertido em array string</param>
        /// <returns>retorna array de string.</returns>
        public static string[] Execute(char[] arrayChar)
        {
            if (arrayChar == null || arrayChar is null || arrayChar.Length == 0)
            {
                return System.Array.Empty<string>();
            }

            //var retorno = arrayChar.Select(x => x.ToString()).ToArray();

            var arrayString = Array.ConvertAll(arrayChar, c => c.ToString());

            return arrayString;
        }
    }
}
