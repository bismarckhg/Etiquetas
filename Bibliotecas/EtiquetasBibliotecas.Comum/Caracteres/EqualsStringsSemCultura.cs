namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class EqualsStringsSemCultura
    {
        public static bool Execute(this string texto1, string texto2)
        {
            return string.Equals(texto1, texto2, System.StringComparison.Ordinal);
        }

    }
}
