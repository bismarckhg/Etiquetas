using System;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class EhArrayCharNuloVazioComEspacosBranco
    {
        /// <summary>
        /// Verifica se char[] (Array string) informada esta Nula, ou em branco(ou vazio) ou com caractere de espaço.
        /// </summary>
        /// <param name="arrayChar">
        /// Char a ser verificada.
        /// </param>
        /// <returns>
        /// True se a array char[] estiver nula, ou com todos os elementos em branco ou com espaço. Caso contrario false.
        /// </returns>
        public static bool Execute(this char[] arrayChar)
        {
            if (arrayChar == null || arrayChar.Length == 0)
            {
                return true;
            }
            return arrayChar.All(c => char.IsWhiteSpace(c));
        }

    }
}
