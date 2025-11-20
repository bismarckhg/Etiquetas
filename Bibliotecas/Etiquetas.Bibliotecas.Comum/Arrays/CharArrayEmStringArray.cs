using System;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class CharArrayEmStringArray
    {
        public static string[] Execute(char[] arrayChar)
        {
            if (arrayChar == null || arrayChar is null || arrayChar.Length == 0)
            {
                return System.Array.Empty<string>();
            }

            //var retorno = arrayChar.Select(x => x.ToString()).ToArray();

            var arrayString = Array.ConvertAll(arrayChar, c => c.ToString());

            return arrayString;
        }

    }
}
