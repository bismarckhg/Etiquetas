using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Concatena um array de strings com um novo elemento string.
    /// </summary>
    public static class ConcatenarArrayComNovoElementoString
    {
        /// <summary>
        /// Concatena um array de strings com um novo elemento string.
        /// </summary>
        /// <param name="array">Array de String.</param>
        /// <param name="elemento">Elemeto String</param>
        /// <returns>Novo array com elemento adicionado string.</returns>
        public static string[] Execute(string[] array, string elemento)
        {
            if (EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array))
            {
                var arrayNovo = new string[] { elemento };
                return arrayNovo;
            }

            var novoArray = array.Concat(new string[] { elemento }).ToArray();
            return novoArray;
        }
    }
}
