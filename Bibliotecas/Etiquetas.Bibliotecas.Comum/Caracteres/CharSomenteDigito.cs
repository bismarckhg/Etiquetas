namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Verifica se um caractere é um dígito numérico.
    /// </summary>
    public static class CharSomenteDigito
    {
        /// <summary>
        /// Verifica se o caractere fornecido é um dígito numérico. 
        /// </summary>
        /// <param name="caractere">caractere a ser verificado.</param>
        /// <returns>true ou falso para digito númerico.</returns>
        public static bool Execute(char caractere)
        {
            return char.IsDigit(caractere);
        }

        /// <summary>
        /// Verifica se o caractere fornecido é um dígito numérico.
        /// </summary>
        /// <param name="caractere">caractere a ser verificado.</param>
        /// <returns>true ou falso para digito númerico.</returns>
        public static bool Execute(char? caractere)
        {
            if (caractere == null)
            {
                return false;
            }

            return char.IsDigit((char)caractere);
        }
    }
}
