namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class RetiraCaracterDireita
    {
        /// <summary>
        /// Retira da string informada os caracyetes mais a direita, conforme a quantidade informada.
        /// </summary>
        /// <param name="texto">
        /// string informada com os caracteres a serem extraidos.
        /// </param>
        /// <param name="numeroCaracteres">
        /// Numero inteiro com a quantidade de caracteres a direita a ser excluido.
        /// </param>
        /// <returns>
        /// Retorna a string com o restante dos caracteres depois de retirar os mais a esquerda.
        /// </returns>
        public static string Execute(this string texto, int numeroCaracteres)
        {
            var esquerda = ExtrairTextoEsquerda.Execute(texto, numeroCaracteres);
            var direita = ExtrairTextoDireita.Execute(texto, NumeroCaracteres.Execute(texto) - (numeroCaracteres + 1));
            return ConcatenarTexto.Execute(esquerda, direita);
        }
    }
}
