using System.Linq;

namespace Etiquetas.Bibliotecas.LibArrayChar
{
    public static class ConcaternarCharArraysEmCharArray
    {
        public static char[] Execute(char[] charArray1, char[] charArray2)
        {
            if (Etiquetas.Bibliotecas.EhArrayCharNuloVazioComEspacosBranco.Execute(charArray1))
            {
                if (Etiquetas.Bibliotecas.EhArrayCharNuloVazioComEspacosBranco.Execute(charArray2))
                    return new char[] { };
                return charArray2;
            }
            if (Etiquetas.Bibliotecas.EhArrayCharNuloVazioComEspacosBranco.Execute(charArray2))
                return charArray1;
            var retornoCharArray = charArray1.Concat(charArray2).ToArray();
            return retornoCharArray;
        }
    }
}
