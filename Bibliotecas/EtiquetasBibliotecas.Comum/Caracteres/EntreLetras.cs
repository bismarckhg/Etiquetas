namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class EntreLetras
    {
        /// <summary>
        /// Insere uma string entre letras de outra string.
        /// </summary>
        /// <param name="texto">
        /// String a receber outra string entre seus caracteres(letras).
        /// </param>
        /// <param name="entreLetras">
        /// String a ser inserida entre os caracteres(letras) da outra string.
        /// </param>
        /// <returns>
        /// Retorna string inserida entre letras(caracteres).
        /// </returns>
        public static string Execute(this string texto, string entreLetras)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            if (string.IsNullOrEmpty(entreLetras))
                return texto;

            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < texto.Length; i++)
            {
                sb.Append(texto[i]);
                if (i < texto.Length - 1) // Don't append after the last character
                {
                    sb.Append(entreLetras);
                }
            }
            return sb.ToString();
        }
    }
}
