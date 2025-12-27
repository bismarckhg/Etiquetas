namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Classe responsável por concatenar um texto a um caractere.
    /// </summary>
    public static class ConcatenarTextoCaracter
    {
        /// <summary>
        /// Concatena um texto a um caractere.
        /// </summary>
        /// <param name="texto">string texto</param>
        /// <param name="caractere">caractere</param>
        /// <returns>string texto concatenado com caractere.</returns>
        public static string Execute(string texto, char caractere)
        {
            return $"{texto}{caractere}";
        }

    }
}
