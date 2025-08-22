using System.Linq;

namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class ExcluirElementosVaziosArrayString
    {
        public static string[] Execute(this string[] array)
        {
            var invalido = array is null;
            invalido = invalido && array.All(x => Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(x));
            if (invalido)
            {
                return array;
            }

            var retorno = array.Where(x => !Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(x)).ToArray();
            return retorno;
        }
    }
}
