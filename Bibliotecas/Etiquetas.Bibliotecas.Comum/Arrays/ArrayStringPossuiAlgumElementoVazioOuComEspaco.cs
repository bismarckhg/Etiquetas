using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Classe responsavel por verificar se string[] (Array string) informada possui elementos com Nulo, ou em branco(ou vazio) ou com caractere de espaço.
    /// </summary>
    public static class ArrayStringPossuiAlgumElementoVazioOuComEspaco
    {
        /// <summary>
        /// Verifica se string[] (Array string) informada possui elementos com Nulo, ou em branco(ou vazio) ou com caractere de espaço.
        /// </summary>
        /// <param name="array">
        /// string a ser verificada.
        /// </param>
        /// <returns>
        /// True se a array string[] estiver com um ou todos os elementos com nulo, em branco ou com espaço. Caso contrario false.
        /// </returns>
        public static bool Execute(string[] array)
        {
            if (array == null || array.Length == 0)
            {
                return false;
            }

            return array.Any(x => EhStringNuloVazioComEspacosBranco.Execute(x));
        }
    }
}
