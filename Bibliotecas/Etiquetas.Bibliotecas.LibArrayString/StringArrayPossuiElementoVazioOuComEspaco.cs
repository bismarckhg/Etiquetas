using System.Linq;

namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class StringArrayPossuiElementoVazioOuComEspaco
    {
        public static bool Execute(string[] array)
        {
            bool retorno = !(array is null);
            retorno = retorno && array.Any(x => Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(x));
            return retorno;
        }

    }
}
