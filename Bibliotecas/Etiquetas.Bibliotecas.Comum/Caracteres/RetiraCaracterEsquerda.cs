namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class RetiraCaracterEsquerda
    {
        /// <summary>
        /// Retira da string informada os caracteres mais a esquerda , conforme a quantidade informada.
        /// </summary>
        /// <param name="texto">
        /// string informada com os caracteres a serem extraidos.
        /// </param>
        /// <param name="numeroCaracteres">
        /// Numero inteiro com quantidade de caracteres a esquerda a ser excluido.
        /// </param>
        /// <returns>
        /// Retorna a string com o restante dos caracteres depois de retirar os mais a esquerda.
        /// </returns>
        public static string Execute(this string texto, int numeroCaracteres)
        {
            if (string.IsNullOrEmpty(texto) || numeroCaracteres <= 0)
            {
                return texto;
            }

            if (numeroCaracteres >= texto.Length)
            {
                return string.Empty;
            }

            return texto.Substring(numeroCaracteres);
        }
    }
}
