using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class ConcaternarCharArraysEmCharArray
    {
        public static char[] Execute(char[] charArray1, char[] charArray2)
        {
            if (EhArrayCharNuloVazioComEspacosBranco.Execute(charArray1))
            {
                if (EhArrayCharNuloVazioComEspacosBranco.Execute(charArray2))
                    return new char[] { };
                return charArray2;
            }
            if (EhArrayCharNuloVazioComEspacosBranco.Execute(charArray2))
                return charArray1;
            var retornoCharArray = charArray1.Concat(charArray2).ToArray();
            return retornoCharArray;
        }
    }
}
