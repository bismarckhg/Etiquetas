namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ExcluiCaractere
    {
        /// <summary>
        /// Retira da string informada os caracteres iguais ao caractere informado.
        /// </summary>
        /// <param name="texto">
        /// string informada com os caracteres a serem extraidos.
        /// </param>
        /// <param name="caractere">
        /// caractere a ser extraido da string.
        /// </param>
        /// <returns>
        /// Retorna  o restante da string sem os caracteres retirados.
        /// </returns>
        public static string Execute(string texto, char caractere)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }
            return texto.Replace(caractere.ToString(), string.Empty);
        }
    }
}
