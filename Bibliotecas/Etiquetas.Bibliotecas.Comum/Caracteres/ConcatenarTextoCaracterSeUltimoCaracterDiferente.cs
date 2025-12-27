namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Classe responsável por concatenar um caractere a um texto, se o último caractere for diferente.
    /// </summary>
    public static class ConcatenarTextoCaracterSeUltimoCaracterDiferente
    {
        /// <summary>
        /// Concatena um caractere a um texto, se o último caractere for diferente.
        /// </summary>
        /// <param name="texto">string texto.</param>
        /// <param name="caractere">caractere</param>
        /// <returns>retorno string concatenado.</returns>
        public static string Execute(string texto, char caractere)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return caractere.ToString();
            }
            if (texto[texto.Length - 1] != caractere)
            {
                return texto + caractere;
            }

            return texto;
        }
    }
}
