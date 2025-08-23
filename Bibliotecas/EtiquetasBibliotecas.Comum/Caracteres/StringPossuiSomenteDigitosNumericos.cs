using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringPossuiSomenteDigitosNumericos
    {
        public static bool Execute(this string texto)
        {
            var naoEhNuloOuVazio = !EhStringNuloVazioComEspacosBrancoDBNull.Execute(texto);
            var ehNumerico = naoEhNuloOuVazio && texto.All(char.IsDigit);
            return ehNumerico;
        }

    }
}
