namespace Etiquetas.Bibliotecas.LibString
{
    public static class NumeroCaracteres
    {
        public static int Execute(this string texto)
        {
            if (Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto))
            {
                return 0;
            }
            return texto.Length;
        }
    }
}
