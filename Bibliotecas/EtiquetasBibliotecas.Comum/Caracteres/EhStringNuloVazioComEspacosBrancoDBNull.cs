
namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class EhStringNuloVazioComEspacosBrancoDBNull
    {
        /// <summary>
        /// Verifica se string informada esta com Nulo, ou em branco(ou vazio) ou com caracteres de espaço.
        /// </summary>
        /// <param name="texto">
        /// string a ser verificada.
        /// </param>
        /// <returns>
        /// True se a string estiver nula, ou em branco ou com espaços. Caso contrario false.
        /// Continuação da verificação de que a variável possui DBNull. Verificada dessa forma se a variável original
        /// possui declaração com var e tiver sido carregada com DBNull.
        /// </returns>
        public static bool Execute(this string texto)
        {
            var retorno = (texto is null) || EhStringNuloVazioComEspacosBranco.Execute(texto);
            return retorno;
        }

        public static bool Execute(this System.DBNull texto)
        {
            return (texto == System.DBNull.Value) || (texto is null);
        }

    }
}
