using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringPossuiSomenteDigitosNumericos
    {
        public static bool Execute(this string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return false;
            }
            return texto.All(char.IsDigit);
        }

    }
}
