namespace Etiquetas.Bibliotecas.LibChar
{
    public static class ExtrairCharEmString
    {
 
        /// <summary>
        /// Retira da string informada o caracter na posicao informada.
        /// </summary>
        /// <param name="texto">
        /// string informada com o caracter a ser extraido.
        /// </param>
        /// <param name="posicao">
        /// posicao do caractere na string a ser extraido.
        /// </param>
        /// <returns>
        /// Retorna o caractere da string em formato char.
        /// </returns>
        public static char Execute(string texto, int posicao)
        {
            var ehNuloTexto = Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto);
            var textoOuPosicaoInvalida = ehNuloTexto || posicao >= texto.Length || posicao < 0;
            var retornoCaractere = textoOuPosicaoInvalida ? ' ' : texto[posicao];
            return retornoCaractere;
        }
    }
}
