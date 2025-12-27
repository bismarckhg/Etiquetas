using System.Linq;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Classe responsavel por verificar se string[] (Array string) contem determinado elemento string. 
    /// </summary>
    public static class StringArrayContemElementoString
    {
        /// <summary>
        /// Verifica se string[] (Array string) informada contem determinado elemento string.
        /// </summary>
        /// <param name="array">array de string a ser verificado.</param>
        /// <param name="contem">string a ser verificado no array.</param>
        /// <returns>retorna true ou false se string contem existir ou nao no array string.</returns>
        public static bool Execute(string[] array, string contem)
        {
            if (EhStringNuloVazioComEspacosBrancoDBNull.Execute(contem))
            {
                return EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array);
            }

            if (EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array))
            {
                return false;
            }

            return array.Any(x => string.Compare(x, contem, System.StringComparison.Ordinal) == 0);
        }
    }
}
