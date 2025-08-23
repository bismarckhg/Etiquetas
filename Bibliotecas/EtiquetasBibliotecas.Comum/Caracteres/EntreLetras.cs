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
            //char[]
            //var letras = Biblioteca.LibArrayChar.ConverteStringParaArrayChar.Execute(texto);
            var retorno = string.Empty;

            foreach (var letra in texto)
            {
                retorno = ConcatenarTextoCaracter.Execute(retorno, letra);
                retorno = ConcatenarTexto.Execute(retorno, entreLetras);
            }

            return retorno;
        }
    }
}
