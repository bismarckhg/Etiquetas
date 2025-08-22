using System.Globalization;

namespace Etiquetas.Bibliotecas.LibString
{
    public static class StringContemStringInsensitiveAcentoInsensitiveCase
    {
        public static bool Execute(string texto, string contem)
        {
            if (Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto))
            {
                return false;
            }
            if (Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(contem))
            {
                return false;
            }
            var culturaAtual = CultureInfo.CurrentCulture;
            int posicao;
            posicao = culturaAtual.CompareInfo.IndexOf(texto, contem, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase);
            bool resultado = posicao >= 0;
            return resultado;
        }

    }
}
