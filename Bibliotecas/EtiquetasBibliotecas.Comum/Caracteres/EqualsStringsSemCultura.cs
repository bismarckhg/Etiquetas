namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class EqualsStringsSemCultura
    {
        public static bool Execute(this string texto1, string texto2)
        {
            var texto2Vazio = EhStringNuloVazioComEspacosBranco.Execute(texto1);
            var parametrosVazio = texto2Vazio && EhStringNuloVazioComEspacosBranco.Execute(texto2);
            var text2NaoVazio = !texto2Vazio;
            var igualdade = parametrosVazio || (text2NaoVazio && texto1.Equals(texto2));
            return igualdade;
        }

    }
}
