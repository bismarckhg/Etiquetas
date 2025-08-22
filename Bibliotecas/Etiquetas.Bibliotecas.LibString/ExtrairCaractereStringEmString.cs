namespace Etiquetas.Bibliotecas.LibString
{
    public static class ExtrairCaractereStringEmString
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
        /// Retorna o caractere da string em formato string.
        /// </returns>
        public static string Execute(string texto, int posicao)
        {
            var ehNuloTexto = Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto);
            var textoOuPosicaoInvalida = ehNuloTexto || posicao >= texto.Length || posicao < 0;
            var retornoCaractere = textoOuPosicaoInvalida ? string.Empty : ExtrairTexto.Execute(texto, posicao, 1);
            return retornoCaractere;
        }

    }
}
