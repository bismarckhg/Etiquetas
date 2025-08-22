namespace Etiquetas.Bibliotecas.LibString
{
    public static class SubstituiString
    {
        public static string Execute(string texto, string textoAntigo, string textoNovo)
        {
            return texto.Replace(textoAntigo, textoNovo);
        }

    }
}
