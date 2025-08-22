using System.Linq;

namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class StringArrayRemoveElementosVazios
    {
        public static string[] Execute(string[] array)
        {
            bool retorno = array is null;
            retorno = retorno && Etiquetas.Bibliotecas.EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            if (retorno)
            {
                return new string[] { string.Empty };
            }

            var novoArray = array.Where(x => !Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(x)).ToArray();
            bool novoRetorno = !retorno && !Etiquetas.Bibliotecas.EhArrayStringNuloVazioComEspacosBranco.Execute(novoArray);
            if (novoRetorno)
            {
                return novoArray;
            }
            return array;
        }

    }
}
