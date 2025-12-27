using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Concatena dois arrays de char em um único array de char.
    /// </summary>
    public static class ConcaternarCharArraysEmCharArray
    {
        /// <summary>
        /// Concatena dois arrays de char em um único array de char.
        /// </summary>
        /// <param name="charArray1">char array 1 para concatenar.</param>
        /// <param name="charArray2">char array 2 para concatenar.</param>
        /// <returns>char array concatenado.</returns>
        public static char[] Execute(char[] charArray1, char[] charArray2)
        {
            if (EhArrayCharNuloVazioComEspacosBranco.Execute(charArray1))
            {
                if (EhArrayCharNuloVazioComEspacosBranco.Execute(charArray2))
                {
                    return new char[] { };
                }

                return charArray2;
            }
            if (EhArrayCharNuloVazioComEspacosBranco.Execute(charArray2))
            {
                return charArray1;
            }

            var retornoCharArray = charArray1.Concat(charArray2).ToArray();
            return retornoCharArray;
        }
    }
}
