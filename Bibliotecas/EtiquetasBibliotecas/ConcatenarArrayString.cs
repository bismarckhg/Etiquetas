using System.Collections.Generic;
using System.Linq;

namespace Etiquetas.Bibliotecas
{
    public static class ConcatenarArrayString
    {
        public static string[] Execute(string[] array1, string[] array2)
        {
            if (EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array1))
            {
                if (EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array2))
                    return System.Array.Empty<string>();
                return array2;
            }
            if ((EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array2)))
                return array1;
            var retornoStringArray = array1.Concat(array2).ToArray();
            return retornoStringArray;
        }

        public static string[] Execute(params string[][] arrays)
        {
            if (arrays == null || arrays.Length == 0)
                return System.Array.Empty<string>();

            var listaResultado = new List<string>();

            foreach (var array in arrays)
            {
                if (array != null && array.Length > 0)
                {
                    listaResultado.AddRange(array.Where(item => !Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(item)));
                }
            }

            return listaResultado.ToArray();
        }
    }
}
