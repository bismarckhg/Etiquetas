namespace Etiquetas.Bibliotecas.LibString
{
    public static class ExtrairTextoEsquerda
    {
        /// <summary>
        /// Extrair o Texto a Esquerda da string informada.
        /// </summary>
        /// <param name="texto">
        /// string de texto a ser extraida.
        /// </param>
        /// <param name="numeroCaracteres">
        /// Numero inteiro com a quantidade de caracteres mais a esquerda dentro do texto a ser extraido.
        /// </param>
        /// <returns>
        /// Retorna o string extraida conforme o numeroCaracteres informado.
        /// </returns>
        public static string Execute(this string texto, int numeroCaracteres)
        {
            if (numeroCaracteres <= 0)
                return string.Empty;

            if (numeroCaracteres >= NumeroCaracteres.Execute(texto))
                return texto;

            return texto.Substring(0, numeroCaracteres);
        }
    }
}
