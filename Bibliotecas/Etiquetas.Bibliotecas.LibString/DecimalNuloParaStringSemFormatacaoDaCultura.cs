namespace Etiquetas.Bibliotecas.LibString
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
