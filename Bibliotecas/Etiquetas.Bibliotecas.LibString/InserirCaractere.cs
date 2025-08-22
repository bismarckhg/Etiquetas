namespace Etiquetas.Bibliotecas.LibString
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
            if ((posicao + 1) == NumeroCaracteres.Execute(texto))
            {
                var textoCaractere = ConcatenarTextoCaracter.Execute(texto, caractere);
                return textoCaractere;
            }

            var esquerda = ExtrairTextoEsquerda.Execute(texto, posicao);
            var direita = ExtrairTextoDireita.Execute(texto, NumeroCaracteres.Execute(texto) - posicao);
            var textoCaractereTexto = ConcatenarTextoCaracterTexto.Execute(esquerda, caractere, direita);
            return textoCaractereTexto;
        }
    }
}
