using System.Linq;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Remove elementos vazios de um array de strings.
    /// </summary>
    public static class StringArrayRemoveElementosVazios
    {
        /// <summary>
        /// Remove elementos vazios de um array de strings.
        /// </summary>
        /// <param name="array">array de string.</param>
        /// <returns>retorna array de string.</returns>
        public static string[] Execute(string[] array)
        {
            if (array == null)
            {
                return new string[0];
            }

            return array.Where(x => !EhStringNuloVazioComEspacosBranco.Execute(x)).ToArray();
        }
    }
}
