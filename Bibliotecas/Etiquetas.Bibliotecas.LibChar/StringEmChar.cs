namespace Etiquetas.Bibliotecas.LibChar
{
    public static class StringEmChar
    {
        /// <summary>
        /// Converte o primeiro caracter de uma string em um tipo Char
        /// </summary>
        /// <param name="conteudo">
        /// String a ser convertida.
        /// </param>
        /// <returns>
        /// retorna null se string nula. Caso contrário converte para Array do tipo Char.
        /// </returns>
        public static char Execute(this string conteudo)
        {
            if (Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(conteudo))
                return default;
            return conteudo[0];
        }
    }
}
