namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Concatena dois textos com um caractere entre eles.
    /// </summary>
    public static class ConcatenarTextoCaracterTexto
    {
        /// <summary>
        /// Concatena dois textos com um caractere entre eles.
        /// </summary>
        /// <param name="texto1">string texto 1 a ser concatenado</param>
        /// <param name="caractere">caractere a ser concatenado.</param>
        /// <param name="texto2">string texto2 a ser concatenado.</param>
        /// <returns>string concatenada.</returns>
        public static string Execute(string texto1, char caractere, string texto2)
        {
            return $"{texto1}{caractere}{texto2}";
        }
    }
}
