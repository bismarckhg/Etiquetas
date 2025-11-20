namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class RepeteCaracter
    {
        /// <summary>
        /// Repete caractere em uma string.
        /// </summary>
        /// <param name="caractere">
        /// caractere a ser repetido.
        /// </param>
        /// <param name="repete">
        /// Inteiro com a quantidade de caracteres a ser repetido na string.
        /// </param>
        /// <returns>
        /// Retorna string com caracteres repetidos.
        /// </returns>
        public static string Execute(char caractere, int repete)
        {
            return new string(caractere, repete);
        }
    }
}
