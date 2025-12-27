namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Verifica se um caractere é uma letra.
    /// </summary>
    public static class CharSomenteLetra
    {
        /// <summary>
        /// Verifica se o caractere fornecido é uma letra.
        /// </summary>
        /// <param name="caractere">caracteres a ser verificado.</param>
        /// <returns>true e false</returns>
        public static bool Execute(char caractere)
        {
            return char.IsLetter(caractere);
        }
    }
}
