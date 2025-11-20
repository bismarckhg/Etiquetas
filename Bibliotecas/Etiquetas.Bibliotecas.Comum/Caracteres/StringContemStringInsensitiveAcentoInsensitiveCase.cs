using System.Globalization;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringContemStringInsensitiveAcentoInsensitiveCase
    {
        public static bool Execute(string texto, string contem)
        {
            if (EhStringNuloVazioComEspacosBranco.Execute(texto))
            {
                return false;
            }
            if (EhStringNuloVazioComEspacosBranco.Execute(contem))
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
