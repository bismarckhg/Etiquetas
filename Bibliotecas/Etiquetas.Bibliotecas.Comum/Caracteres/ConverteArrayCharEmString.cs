using Etiquetas.Bibliotecas.Comum.Arrays;
namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConverteArrayCharEmString
    {
        /// <summary>
        /// Converte um Array do tipo Char em String
        /// </summary>
        /// <param name="arrayChar">
        /// String a ser convertida.
        /// </param>
        /// <returns>
        /// retorna null se string nula. Caso contrário converte para Array do tipo Char.
        /// </returns>
        public static string Execute(this char[] arrayChar)
        {
            var ehNuloArrayChar = EhArrayCharNuloVazioComEspacosBranco.Execute(arrayChar);
            var retorno = ehNuloArrayChar ? string.Empty : new string(arrayChar);
            return retorno;
        }
    }
}
