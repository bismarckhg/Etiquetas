namespace Etiquetas.Bibliotecas.LibString
{
    public static class StringContemNumeroValido
    {
        public static bool Execute(string texto)
        {
            var naoEhNuloOuVazio = !Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto);
            var ehNumerico = naoEhNuloOuVazio && decimal.TryParse(texto, out decimal test);
            return ehNumerico;
        }
    }
}
