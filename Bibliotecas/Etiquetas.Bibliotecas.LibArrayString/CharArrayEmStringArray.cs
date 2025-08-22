using System.Linq;

namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class CharArrayEmStringArray
    {
        public static string[] Execute(char[] arrayChar)
        {
            if (arrayChar == null)
            {
                return new string[] { };
            }

            var retorno = arrayChar.Select(x => x.ToString()).ToArray();
            return retorno;
        }

    }
}
