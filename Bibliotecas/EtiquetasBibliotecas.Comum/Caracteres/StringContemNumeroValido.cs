namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringContemNumeroValido
    {
        public static bool Execute(string texto)
        {
            var naoEhNuloOuVazio = !EhStringNuloVazioComEspacosBranco.Execute(texto);
            var ehNumerico = naoEhNuloOuVazio && decimal.TryParse(texto, out decimal test);
            return ehNumerico;
        }
    }
}
