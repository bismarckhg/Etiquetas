using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringPossuiSomenteLetras
    {
        public static bool Execute(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return false;
            }
            return texto.All(char.IsLetter);
        }

    }
}
