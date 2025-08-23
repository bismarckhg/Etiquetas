namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringEmCharNull
    {
        /// <summary>
        /// Converte o primeiro caracter de uma string em um tipo Char?
        /// </summary>
        /// <param name="conteudo">
        /// String a ser convertida.
        /// </param>
        /// <returns>
        /// retorna null se string ou vazia nula. Caso contrário converte para Array do tipo Char.
        /// </returns>
        public static char? Execute(this string conteudo)
        {
            if (EhStringNuloVazioComEspacosBranco.Execute(conteudo))
            {
                return null;
            }

            return conteudo[0];
        }
    }
}
