using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringPossuiSomenteLetras
    {
        public static bool Execute(string array)
        {
            var naoEhNuloOuVazio = !EhStringNuloVazioComEspacosBranco.Execute(array);
            var possuiLetras = naoEhNuloOuVazio && array.All(x => char.IsLetter(x));
            return possuiLetras;
        }

    }
}
