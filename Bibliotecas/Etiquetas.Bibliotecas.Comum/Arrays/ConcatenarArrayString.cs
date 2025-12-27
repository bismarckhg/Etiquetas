using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Collections.Generic;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Classe responsável por concatenar arrays de strings.
    /// </summary>
    public static class ConcatenarArrayString
    {
        /// <summary>
        /// Concatena dois arrays de strings em um único array.
        /// </summary>
        /// <param name="array1">Array string 1 a ser concatenado.</param>
        /// <param name="array2">Array string 2 a ser concatenado.</param>
        /// <returns>Array concatenado com Array string 1 e Array string 2.</returns>
        public static string[] Execute(string[] array1, string[] array2)
        {
            if (EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array1))
            {
                if (EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array2))
                {
                    return System.Array.Empty<string>();
                }

                return array2;
            }

            if (EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array2))
            {
                return array1;
            }

            var retornoStringArray = array1.Concat(array2).ToArray();
            return retornoStringArray;
        }

        /// <summary>
        /// Concatena múltiplos arrays de strings em um único array.
        /// </summary>
        /// <param name="arrays">Multiplos array string.</param>
        /// <returns>Unico array string concatenado, com os multiplos array string.</returns>
        public static string[] Execute(params string[][] arrays)
        {
            if (arrays == null || arrays.Length == 0)
            {
                return System.Array.Empty<string>();
            }

            var listaResultado = new List<string>();

            foreach (var array in arrays)
            {
                if (array != null && array.Length > 0)
                {
                    listaResultado.AddRange(array.Where(item => !EhStringNuloVazioComEspacosBranco.Execute(item)));
                }
            }

            return listaResultado.ToArray();
        }
    }
}
