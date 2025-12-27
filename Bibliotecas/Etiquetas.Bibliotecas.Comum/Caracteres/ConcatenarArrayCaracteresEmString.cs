namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Concatena um array de caracteres em uma string.
    /// </summary>
    public static class ConcatenarArrayCaracteresEmString
    {
        /// <summary>
        /// Concatena um array de caracteres em uma string.
        /// </summary>
        /// <param name="array">array de caracteres.</param>
        /// <returns>retorna string de array char.</returns>
        public static string Execute(char[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }

            return new string(array);
        }
    }
}
