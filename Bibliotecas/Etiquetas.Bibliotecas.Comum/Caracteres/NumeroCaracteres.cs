namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class NumeroCaracteres
    {
        public static int Execute(this string texto)
        {
            if (EhStringNuloVazioComEspacosBranco.Execute(texto))
            {
                return 0;
            }
            return texto.Length;
        }
    }
}
