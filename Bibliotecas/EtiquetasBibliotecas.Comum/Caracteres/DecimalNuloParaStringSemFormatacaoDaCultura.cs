namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class DecimalNuloParaStringSemFormatacaoDaCultura
    {
        public static string Execute(this decimal? value)
        {
            return value.HasValue ? value.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : string.Empty;
        }
    }
}
