namespace Etiquetas.Bibliotecas.LibString
{
    public static class DecimalParaStringSemFormatacaoDaCultura
    {
        public static string Execute(this decimal value)
        {
            return value.ToString();
        }
    }
}
