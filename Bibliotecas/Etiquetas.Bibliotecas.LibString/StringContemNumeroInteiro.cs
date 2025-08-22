namespace Etiquetas.Bibliotecas.LibString
{
    public static class StringContemNumeroInteiro
    {
        public static bool Execute(this string inteiro)
        {
            var stringNaoVazio = !Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(inteiro);
            var contemNumeroInteiro = stringNaoVazio && int.TryParse(inteiro, out int output);
            return contemNumeroInteiro;
        }
    }
}
