using System;
using System.Linq;

namespace Etiquetas.Bibliotecas
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
        public static bool Execute(this Char[] arrayChar)
        {
            var arrayCharNulo = (arrayChar == null);
            var arrayCharNuloOuVazio = arrayCharNulo || arrayChar.Length == 0;
            arrayCharNuloOuVazio = arrayCharNuloOuVazio || arrayChar.All(x => Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(x.ToString()));
            //Bibliotecas.COLibString.COEhStringNuloOuVazioOuComEspacosBranco.Execute(x.ToString()))
            return arrayCharNuloOuVazio;
        }

    }
}
