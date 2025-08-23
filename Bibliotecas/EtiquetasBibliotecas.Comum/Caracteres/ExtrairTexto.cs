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
            if (posicao < 0)
            {
                return string.Empty;
            }

            if (numeroCaracteres <= 0)
            {
                return string.Empty;
            }

            if (posicao >= NumeroCaracteres.Execute(texto))
            {
                return string.Empty;
            }

            var valido = ((posicao + numeroCaracteres) <= NumeroCaracteres.Execute(texto));
            var retorno = valido
                ? texto.Substring(posicao, numeroCaracteres)
                : texto.Substring(posicao, NumeroCaracteres.Execute(texto));

            return retorno;
        }
    }
}
