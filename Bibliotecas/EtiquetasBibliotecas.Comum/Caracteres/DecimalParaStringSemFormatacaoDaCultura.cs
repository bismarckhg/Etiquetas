namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class DecimalParaStringSemFormatacaoDaCultura
    {
        public static string Execute(this decimal value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
