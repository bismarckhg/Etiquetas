using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Globalization;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Verifica se um array de strings contém um elemento string, ignorando acentos e diferenças de maiúsculas/minúsculas.
    /// </summary>
    public static class StringArrayContemElementoStringInsensitiveAcentoInsensitiveCase
    {
        /// <summary>
        /// Verifica se o array de strings contém o elemento string especificado, ignorando acentos e diferenças de maiúsculas/minúsculas.
        /// </summary>
        /// <param name="array">array de string.</param>
        /// <param name="contem">string a ser verificado</param>
        /// <returns>true ou false para verificar se string esta no array.</returns>
        public static bool Execute(string[] array, string contem)
        {
            var contemVazio = EhStringNuloVazioComEspacosBranco.Execute(contem);
            var parametrosVazio = contemVazio && EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array);
            var contemNaoVazio = !contemVazio;
            var culturaAtual = CultureInfo.CurrentCulture;
            var contemString = parametrosVazio || (contemNaoVazio && array.Any(x => string.Compare(x, contem, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase).Equals(0)));
            return contemString;
        }
    }
}
