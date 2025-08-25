namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class InserirCaractere
    {
        /// <summary>
        /// Insere o caractere na posicao informada da string.
        /// </summary>
        /// <param name="texto">
        /// string que tera o caractere inserido.
        /// </param>
        /// <param name="caractere">
        /// caractere do tipo char a ser inserido.
        /// </param>
        /// <param name="posicao">
        /// Numero inteiro informando a posicao de inserção do caractere na string.
        /// </param>
        /// <returns>
        /// Retorna string com caractere inserido na posicao inteira informada.
        /// </returns>
        public static string Execute(this string texto, char caractere, int posicao)
        {
            if (texto == null)
            {
                return null;
            }
            if (posicao < 0 || posicao > texto.Length)
            {
                return texto;
            }
            return texto.Insert(posicao, caractere.ToString());
        }
    }
}
