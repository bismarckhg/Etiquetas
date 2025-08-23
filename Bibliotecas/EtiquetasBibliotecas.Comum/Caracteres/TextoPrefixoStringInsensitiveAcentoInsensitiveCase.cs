using System.Globalization;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class TextoPrefixoStringInsensitiveAcentoInsensitiveCase
    {
        public static bool Execute(string texto, string prefixo)
        {
            var myComp = CultureInfo.InvariantCulture.CompareInfo;
            bool resultado = myComp.IsPrefix(texto, prefixo, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase);
            return resultado;
        }

    }
}
