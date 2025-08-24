namespace Etiquetas.Bibliotecas.Comum.Caracteres
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
            if (string.IsNullOrEmpty(texto) || posicao < 0 || posicao >= texto.Length)
            {
                return ' ';
            }
            return texto[posicao];
        }
    }
}
