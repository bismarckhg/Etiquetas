using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringArrayRemoveElementosVazios
    {
        public static string[] Execute(string[] array)
        {
            bool retorno = array is null;
            retorno = retorno && EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            if (retorno)
            {
                return new string[] { string.Empty };
            }

            var novoArray = array.Where(x => !EhStringNuloVazioComEspacosBranco.Execute(x)).ToArray();
            bool novoRetorno = !retorno && !EhArrayStringNuloVazioComEspacosBranco.Execute(novoArray);
            if (novoRetorno)
            {
                return novoArray;
            }
            return array;
        }

    }
}
