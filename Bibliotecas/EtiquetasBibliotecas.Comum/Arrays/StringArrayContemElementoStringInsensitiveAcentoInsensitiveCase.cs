using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Globalization;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringArrayContemElementoStringInsensitiveAcentoInsensitiveCase
    {
        public static bool Execute(string[] array, string contem)
        {
            var contemVazio = EhStringNuloVazioComEspacosBranco.Execute(contem);
            var parametrosVazio = contemVazio && EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array);
            var contemNaoVazio = !contemVazio;
            var culturaAtual = CultureInfo.CurrentCulture;
            var contemString = parametrosVazio || (contemNaoVazio && array.Any(x => string.Compare(x, contem, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase).Equals(0)));
            return contemString;
        }
    }
}
