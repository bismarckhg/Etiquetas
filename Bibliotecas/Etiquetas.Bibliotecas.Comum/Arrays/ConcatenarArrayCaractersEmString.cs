namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Classe responsável por concatenar um array de caracteres em uma string.
    /// </summary>
    public static class ConcatenarArrayCaractersEmString
    {
        /// <summary>
        /// Concatena um array de caracteres em uma string.
        /// </summary>
        /// <param name="array">Array de caracteres.</param>
        /// <returns>retorna string de array de caractere concatenado.</returns>
        public static string Execute(char[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }

            var retornoString = string.Concat(array);
            return retornoString;
        }

    }
}
