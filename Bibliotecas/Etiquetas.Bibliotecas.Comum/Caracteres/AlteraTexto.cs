namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Altera parte do texto informado por um novo conteudo informado.
    /// </summary>
    public static class AlteraTexto
    {
        /// <summary>
        /// Substitui no Texto informado, parte do texto informado pelo novo conteudo também informado.
        /// </summary>
        /// <param name="texto">
        /// Texto string informado contendo o texto a ser substituido.
        /// </param>
        /// <param name="parteTexto">
        /// String informado com parte do texto a ser substituido.
        /// </param>
        /// <param name="novoConteudo">
        /// String informado com novo texto a ser substituido.
        /// </param>
        /// <returns>
        /// Retorna string com o texto já substituido.
        /// </returns>
        public static string Execute(this string texto, string parteTexto, string novoConteudo)
        {
            if (texto == null)
            {
                return null;
            }

            if (parteTexto == null)
            {
                return texto;
            }

            return texto.Replace(parteTexto, novoConteudo);
        }
    }
}
