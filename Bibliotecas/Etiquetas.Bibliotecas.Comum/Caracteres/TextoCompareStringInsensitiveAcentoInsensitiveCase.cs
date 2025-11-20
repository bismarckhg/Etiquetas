using System.Globalization;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class TextoCompareStringInsensitiveAcentoInsensitiveCase
    {
        public static bool Execute(string texto, string comparacao)
        {
            int posicao = string.Compare(texto, comparacao, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase);
            bool resultado = posicao.Equals(0);
            return resultado;
        }

    }
}
