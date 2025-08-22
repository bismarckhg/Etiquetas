namespace Etiquetas.Bibliotecas.LibString
{
    public static class StringContemString
    {
        public static bool Execute(string texto, string contem)
        {
            var contemVazio =  Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(contem);
            var parametrosVazio = contemVazio && Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto);
            var contemNaoVazio = !contemVazio;
            var contemString = parametrosVazio || (contemNaoVazio && texto.Contains(contem));
            return contemString;
        }
    }
}
