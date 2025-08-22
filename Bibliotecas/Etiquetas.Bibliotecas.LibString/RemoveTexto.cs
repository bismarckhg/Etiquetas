namespace Etiquetas.Bibliotecas.LibString
{
    public static class RemoveTexto
    {
        /// <summary>
        /// Remove parte da string na posicao e tamanho informada.
        /// </summary>
        /// <param name="texto">
        /// Texto string original outra string.
        /// </param>
        /// <param name="posicao">
        /// Inteiro com a posicao inicial da string a ser removida.
        /// </param>
        /// <param name="tamanho">
        /// Inteiro com o tamanho do texto informado para ser removido.
        /// </param>
        /// <returns>
        /// Retorna string com texto removido.
        /// </returns>
        public static string Execute(this string texto, int posicao, int tamanho)
        {
            return texto.Remove(posicao, tamanho);
        }
    }
}
