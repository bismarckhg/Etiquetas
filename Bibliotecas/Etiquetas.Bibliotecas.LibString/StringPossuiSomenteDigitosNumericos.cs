using System.Linq;

namespace Etiquetas.Bibliotecas.LibString
{
    public static class StringPossuiSomenteDigitosNumericos
    {
        public static bool Execute(this string texto)
        {
            var naoEhNuloOuVazio = !Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBrancoDBNull.Execute(texto);
            var ehNumerico = naoEhNuloOuVazio && texto.All(char.IsDigit);
            return ehNumerico;
        }

    }
}
