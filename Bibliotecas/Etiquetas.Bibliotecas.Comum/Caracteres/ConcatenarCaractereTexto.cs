namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Classe responsável por concatenar um caractere a um texto.
    /// </summary>
    public static class ConcatenarCaractereTexto
    {
        /// <summary>
        /// Concatena um caractere a um texto.
        /// </summary>
        /// <param name="caractere">caractere</param>
        /// <param name="texto">string texto.</param>
        /// <returns>string concatenado</returns>
        public static string Execute(char caractere, string texto)
        {
            return $"{caractere}{texto}";
        }
    }
}
