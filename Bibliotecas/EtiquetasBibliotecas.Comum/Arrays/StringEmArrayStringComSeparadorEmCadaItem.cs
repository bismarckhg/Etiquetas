using System.Linq;
using System.Text.RegularExpressions;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringEmArrayStringComSeparadorEmCadaItem
    {
        public static string[] Execute(string texto, char separador)
        {
            string pattern = string.Format(@"\{0}.*?(?=\{0}|$)", separador);
            return Regex.Matches(texto, pattern, RegexOptions.Singleline)
                        .Cast<Match>()
                        .Select(match => match.Value)
                        .ToArray();
        }
    }
}
