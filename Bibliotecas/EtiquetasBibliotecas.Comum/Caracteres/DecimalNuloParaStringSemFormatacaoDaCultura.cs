namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class DecimalNuloParaStringSemFormatacaoDaCultura
    {
        public static string Execute(this decimal? value)
        {
            var retornoString = value == null ? string.Empty : value?.ToString();
            return retornoString;
        }
    }
}
