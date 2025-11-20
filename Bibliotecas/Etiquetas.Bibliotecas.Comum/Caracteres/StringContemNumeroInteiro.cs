namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringContemNumeroInteiro
    {
        public static bool Execute(this string inteiro)
        {
            var stringNaoVazio = !EhStringNuloVazioComEspacosBranco.Execute(inteiro);
            var contemNumeroInteiro = stringNaoVazio && int.TryParse(inteiro, out int output);
            return contemNumeroInteiro;
        }
    }
}
