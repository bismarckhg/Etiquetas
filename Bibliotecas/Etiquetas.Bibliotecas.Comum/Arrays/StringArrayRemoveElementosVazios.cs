using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringArrayRemoveElementosVazios
    {
        public static string[] Execute(string[] array)
        {
            if (array == null)
            {
                return new string[0];
            }
            return array.Where(x => !EhStringNuloVazioComEspacosBranco.Execute(x)).ToArray();
        }

    }
}
