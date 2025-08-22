namespace Etiquetas.Bibliotecas.LibString
{
    public static class ExtrairTextoDireita
    {
        /// <summary>
        /// Extrair o texto a Direita da string informada
        /// </summary>
        /// <param name="texto">
        /// string de texto a ser extraida.
        /// </param>
        /// <param name="numeroCaracteres">
        /// Numero inteiro com a quantidade de caracteres mais a direita dentro do texto a ser extraido.
        /// </param>
        /// <returns>
        /// Retorna o string extraida conforme o numeroCaracteres informado.
        /// </returns>
        public static string Execute(this string texto, int numeroCaracteres)
        {
            if (numeroCaracteres <= 0)
                return texto;

            if (numeroCaracteres >= NumeroCaracteres.Execute(texto))
                return string.Empty;

            return texto.Substring(NumeroCaracteres.Execute(texto) - numeroCaracteres, numeroCaracteres);
        }
    }
}
