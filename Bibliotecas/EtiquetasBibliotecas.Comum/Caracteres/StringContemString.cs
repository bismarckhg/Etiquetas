namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringContemString
    {
        public static bool Execute(string texto, string contem)
        {
            var contemVazio =  EhStringNuloVazioComEspacosBranco.Execute(contem);
            var parametrosVazio = contemVazio && EhStringNuloVazioComEspacosBranco.Execute(texto);
            var contemNaoVazio = !contemVazio;
            var contemString = parametrosVazio || (contemNaoVazio && texto.Contains(contem));
            return contemString;
        }
    }
}
