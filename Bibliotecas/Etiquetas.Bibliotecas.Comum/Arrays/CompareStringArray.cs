using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Compara dois arrays do tipo string
    /// </summary>
    public static class CompareStringArray
    {
        /// <summary>
        /// Compara dois arrays do tipo string
        /// </summary>
        /// <param name="array1">Array 1 de comparação.</param>
        /// <param name="array2">Array 2 de Comparação.</param>
        /// <returns>retorna true se array são iguais e false se não são.</returns>
        public static bool Execute(object array1, object array2)
        {
            if (array1 is string[] arrayString1)
            {
                if (array2 is string[] arrayString2)
                {
                    bool retorno = arrayString1.SequenceEqual(arrayString2);
                    return retorno;
                }
            }

            return false;
        }
    }
}
