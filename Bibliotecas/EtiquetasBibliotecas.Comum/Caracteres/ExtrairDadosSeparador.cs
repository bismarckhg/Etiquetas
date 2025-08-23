namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ExtrairDadosSeparador
    {
        /// <summary>
        /// Insere uma string entre letras de outra string.
        /// </summary>
        /// <param name="texto">
        /// String completa para extração do texto.
        /// </param>
        /// <param name="ocorrencia">
        /// Quantidade do Separador para escolha da parte da string a ser extraida.
        /// </param>
        /// <param name="separadores">
        /// String[] (array) que indica separadores da string de texto.
        /// </param>
        /// <returns>
        /// Retorna string serparada pelo numero de ocorrencia dos separadores.
        /// </returns>
        public static string Execute(this string texto, int ocorrencia, params string[] separadores)
        {
            var dados = texto.Split(separadores, System.StringSplitOptions.None);
            var posicao = ocorrencia - 1;
            if (ocorrencia < 1)
            {
                return null;
            }
            return dados[posicao];
        }
    }
}
