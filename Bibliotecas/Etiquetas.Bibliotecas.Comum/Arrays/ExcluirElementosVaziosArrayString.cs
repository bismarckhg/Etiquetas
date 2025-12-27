using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Classe de extensão para excluir elementos vazios de um array de strings.
    /// </summary>
    public static class ExcluirElementosVaziosArrayString
    {
        /// <summary>
        /// Exclui elementos nulos, vazios ou compostos apenas por espaços em branco de um array de strings.
        /// </summary>
        /// <param name="array">string array.</param>
        /// <returns>string array alterado.</returns>
        public static string[] Execute(this string[] array)
        {
            var invalido = array is null;
            invalido = invalido && array.All(x => EhStringNuloVazioComEspacosBranco.Execute(x));
            if (invalido)
            {
                return array;
            }

            var retorno = array.Where(x => !EhStringNuloVazioComEspacosBranco.Execute(x)).ToArray();
            return retorno;
        }
    }
}
