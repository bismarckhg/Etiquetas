namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ExcluiString
    {

        /// <summary>
        /// Retira da string informada os caracteres iguais ao caractere informado.
        /// </summary>
        /// <param name="texto">
        /// string informada com texto para ser retirado com conteudo de outra string.
        /// </param>
        /// <param name="textoASerExcluido">
        /// string a ser extraido da string texto.
        /// </param>
        /// <returns>
        /// Retorna o restante da string texto sem a string retirada.
        /// </returns>
        public static string Execute(string texto, string textoASerExcluido)
        {
            if (string.IsNullOrEmpty(texto) || string.IsNullOrEmpty(textoASerExcluido))
            {
                return texto;
            }
            return texto.Replace(textoASerExcluido, string.Empty);
        }
    }
}
