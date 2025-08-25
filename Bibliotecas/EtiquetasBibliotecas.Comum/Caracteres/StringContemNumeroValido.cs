using System.Globalization;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringContemNumeroValido
    {
        public static bool Execute(string texto)
        {
            var naoEhNuloOuVazio = !EhStringNuloVazioComEspacosBranco.Execute(texto);
            // Using a specific set of NumberStyles for consistent parsing.
            var styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
            var ehNumerico = naoEhNuloOuVazio && decimal.TryParse(texto, styles, CultureInfo.InvariantCulture, out _);
            return ehNumerico;
        }
    }
}
