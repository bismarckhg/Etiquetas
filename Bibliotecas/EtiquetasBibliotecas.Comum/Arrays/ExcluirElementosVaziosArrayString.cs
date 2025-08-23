using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class ExcluirElementosVaziosArrayString
    {
        public static string[] Execute(this string[] array)
        {
            var invalido = array is null;
            invalido = invalido && array.All(x => EhStringNuloVazioComEspacosBranco.Execute(x));
            if (invalido)
            {
                return array;
            }

            var retorno = array.Where(x => !EhStringNuloVazioComEspacosBranco.Execute(x)).ToArray();
            return retorno;
        }
    }
}
