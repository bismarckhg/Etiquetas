using System.Linq;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Verifica se o array de strings possui algum elemento nulo, vazio ou composto apenas por espaços em branco.
    /// </summary>
    public static class StringArrayPossuiElementoVazioOuComEspaco
    {
        /// <summary>
        /// Verifica se o array de strings possui algum elemento nulo, vazio ou composto apenas por espaços em branco.
        /// </summary>
        /// <param name="array">array de string.</param>
        /// <returns>true ou false se possui algum elemento nulo, vazio ou  composto apenas por espaços em branco.</returns>
        public static bool Execute(string[] array)
        {
            bool retorno = !(array is null);
            retorno = retorno && array.Any(x => EhStringNuloVazioComEspacosBranco.Execute(x));
            return retorno;
        }
    }
}
