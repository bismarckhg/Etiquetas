using System;

namespace Etiquetas.Bibliotecas
{
    public static class EhArrayCharNuloVazioComEspacosBrancoDBNull
    {
        /// <summary>
        /// Verifica se char[] (Array string) informada esta com Nula, ou em branco(ou vazio) ou com caractere de espaço.
        /// </summary>
        /// <param name="arrayChar">
        /// Char a ser verificada.
        /// </param>
        /// <returns>
        /// True se a array char[] estiver nula, ou com todos os elementos em branco ou com espaço. Caso contrario false.
        /// Continuação da verificação de que a variável possui DBNull. Verificada dessa forma se a variável original
        /// possui declaração com var.
        /// </returns>
        public static bool Execute(this Char[] arrayChar)
        {
            var vazio = EhArrayCharNuloVazioComEspacosBranco.Execute(arrayChar);
            return vazio;
        }

    }
}
