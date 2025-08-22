namespace Etiquetas.Bibliotecas.LibString
{
    public static class InserirString
    {
        /// <summary>
        /// Inserir string em outra string na posicao informada.
        /// </summary>
        /// <param name="texto">
        /// Texto string a receber outra string.
        /// </param>
        /// <param name="caractere">
        /// String a ser inserida em outra string.
        /// </param>
        /// <param name="posicao">
        /// Inteiro com a posicao no texto informado para inserir outro texto.
        /// </param>
        /// <returns>
        /// Retorna string infromada com a string inserida na posicao.
        /// </returns>
        public static string Execute(this string texto, string caractere, int posicao)
        {
            if (posicao == NumeroCaracteres.Execute(texto))
                return ConcatenarTexto.Execute(texto, caractere);
            var esquerda = ExtrairTextoEsquerda.Execute(texto, posicao);
            var direita = ExtrairTextoDireita.Execute(texto, NumeroCaracteres.Execute(texto) - posicao);
            return ConcatenarTexto.Execute(esquerda, caractere, direita);
        }
    }
}