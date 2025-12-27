namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Converte uma string em um Array do tipo Char
    /// </summary>
    public static class ConverteStringParaArrayChar
    {
        /// <summary>
        /// Converte uma string em um Array do tipo Char
        /// </summary>
        /// <param name="texto">
        /// String a ser convertida.
        /// </param>
        /// <returns>
        /// retorna null se string nula. Caso contrário converte para Array do tipo Char.
        /// </returns>
        public static char[] Execute(this string texto)
        {
            if (texto == null)
            {
                return null;
            }

            return texto.ToCharArray();
        }

    }
}
