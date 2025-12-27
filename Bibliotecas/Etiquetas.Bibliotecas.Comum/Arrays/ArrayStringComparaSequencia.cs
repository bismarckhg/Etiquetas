using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Compara dois arrays de string verificando se possuem a mesma sequência de elementos.
    /// </summary>
    public static class ArrayStringComparaSequencia
    {
        /// <summary>
        /// Compara dois arrays de string verificando se possuem a mesma sequência de elementos.
        /// </summary>
        /// <param name="array1">Array 1 de comparação.</param>
        /// <param name="array2">Array 2 de comparação.</param>
        /// <returns>retorna true</returns>
        public static bool Execute(string[] array1, string[] array2)
        {
            var array1Vazio = EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array1);
            var array2Vazio = EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array2);

            if (array1Vazio && array2Vazio)
            {
                return true;
            }

            if (array1Vazio || array2Vazio)
            {
                return false;
            }

            return array1.SequenceEqual(array2);
        }
    }
}
