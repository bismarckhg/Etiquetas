using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringArrayPossuiElementoVazioOuComEspaco
    {
        public static bool Execute(string[] array)
        {
            bool retorno = !(array is null);
            retorno = retorno && array.Any(x => EhStringNuloVazioComEspacosBranco.Execute(x));
            return retorno;
        }

    }
}
