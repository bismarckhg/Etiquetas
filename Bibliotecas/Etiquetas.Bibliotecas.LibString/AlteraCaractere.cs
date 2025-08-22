namespace Etiquetas.Bibliotecas.LibString
{
    public static class AlteraCaractere
    {
        /// <summary>
        /// Substitui no Texto informado, o caracter pelo novo caractere.
        /// </summary>
        /// <param name="texto">
        /// Texto string informado contendo o texto a ser substituido.
        /// </param>
        /// <param name="caractere">
        /// char informado que devera ser substituido do texto.
        /// </param>
        /// <param name="novoCaractere">
        /// char informado que devera substituir do texto.
        /// </param>
        /// <returns>
        /// Retorna string com o texto já substituido.
        /// </returns>
        public static string Execute(this string texto, char caractere, char novoCaractere)
        {
            return texto.Replace(caractere, novoCaractere);
        }
    }
}
