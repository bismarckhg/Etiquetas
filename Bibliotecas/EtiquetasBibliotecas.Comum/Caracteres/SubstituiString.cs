namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class SubstituiString
    {
        public static string Execute(string texto, string textoAntigo, string textoNovo)
        {
            if (string.IsNullOrEmpty(texto) || textoAntigo == null)
            {
                return texto;
            }
            return texto.Replace(textoAntigo, textoNovo);
        }

    }
}
