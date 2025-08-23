using System.Linq;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class EhArrayStringNuloVazioComEspacosBranco
    {
        /// <summary>
        /// Verifica se string[] (Array string) informada esta Nula, ou em branco(ou vazio) ou com caracteres de espaços.
        /// </summary>
        /// <param name="array">
        /// String[] (Array string)  a ser verificada.
        /// </param>
        /// <returns>
        /// True se a array string[] estiver nula, ou com todos os elementos em branco ou com espaços. Caso contrario false.
        /// </returns>
        public static bool Execute(this string[] array)
        {
            //d = d.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            //var retorno = array is null;

            var retorno = array == null;
            retorno = retorno || array.Length == 0;
            retorno = retorno || array.All(x => EhStringNuloVazioComEspacosBranco.Execute(x));
            return retorno;
        }
    }
}
