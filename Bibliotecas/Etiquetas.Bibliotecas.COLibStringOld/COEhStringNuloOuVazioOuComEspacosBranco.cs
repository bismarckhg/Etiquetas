namespace Etiquetas.Bibliotecas.COLibString
{
    //public static class COEhStringNuloOuVazioOuComEspacosBranco
    public static class COEhStringNuloOuVazioOuComEspacosBranco
    {

        /// <summary>
        /// Verifica se string informada esta Nula, ou em branco(ou vazio) ou com caracteres de espaços.
        /// </summary>
        /// <param name="texto">
        /// String a ser verificada.
        /// </param>
        /// <returns>
        /// True se a string estiver nula, em branco ou com espaços. Caso contrario false.
        /// </returns>
        public static bool Execute(string texto)
        {
            var retorno = string.IsNullOrWhiteSpace(texto);
            return retorno;
        }

        //public static bool Execute(this object texto)
        //{
        //    var retorno = texto is null;
        //    retorno = retorno || ((texto is string @string) && COEhStringNuloOuVazioOuComEspacosBranco.Execute(@string));
        //    retorno = retorno || ((texto is char @char) && COEhStringNuloOuVazioOuComEspacosBranco.Execute((@char).ToString()));
        //    retorno = retorno || (texto is string[] @arraystring) && COEhStringNuloOuVazioOuComEspacosBranco.Execute(@arraystring);
        //    retorno = retorno || (texto is char[] @arraychar) && COEhStringNuloOuVazioOuComEspacosBranco.Execute(@arraychar);
        //    return retorno;
        //}

    }
}
