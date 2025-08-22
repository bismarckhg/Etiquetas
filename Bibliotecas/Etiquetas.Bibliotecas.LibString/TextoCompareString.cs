namespace Etiquetas.Bibliotecas.LibString
{
    public static class TextoCompareString
    {
        public static bool Execute(string texto, string comparacao)
        {
            int posicao = string.Compare(texto, comparacao);
            bool resultado = posicao.Equals(0);
            return resultado;
        }

    }
}
