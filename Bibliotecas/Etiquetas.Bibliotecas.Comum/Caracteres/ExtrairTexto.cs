namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ExtrairTexto
    {
        /// <summary>
        /// Extrair o Texto posicao da string informada.
        /// </summary>
        /// <param name="texto">
        /// string de texto  completa para ser extraida.
        /// </param>
        /// /// <param name="posicao">
        /// Numero inteiro com a posicao de caracteres dentro do texto a ser extraido.
        /// </param>
        /// <param name="numeroCaracteres">
        /// Numero inteiro com a quantidade de caracteres dentro do texto a ser extraido depois da posicao.
        /// </param>
        /// <returns>
        /// Retorna o string extraida conforme o numeroCaracteres informado.
        /// </returns>
        public static string Execute(this string texto, int posicao, int numeroCaracteres)
        {
            if (string.IsNullOrEmpty(texto) || numeroCaracteres <= 0 || posicao < 0 || posicao >= texto.Length)
            {
                return string.Empty;
            }

            if (posicao + numeroCaracteres > texto.Length)
            {
                return texto.Substring(posicao);
            }

            return texto.Substring(posicao, numeroCaracteres);
        }
    }
}
