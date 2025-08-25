using System;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class EhArrayStringNuloVazioComEspacosBrancoDBNull
    {
        /// <summary>
        /// Verifica se string[] (Array string) informada esta com Nula, ou em branco(ou vazio) ou com caractere de espaço.
        /// </summary>
        /// <param name="array">
        /// string a ser verificada.
        /// </param>
        /// <returns>
        /// True se a array string[] estiver nula, ou com todos os elementos em branco ou com espaço. Caso contrario false.
        /// Continuação da verificação de que a variável possui DBNull. Verificada dessa forma se a variável original
        /// possui declaração com var.
        /// </returns>
        public static bool Execute(this string[] array)
        {
            // Chama a verificacao padrao e reusa o mesmo resultado
            var vazio = EhArrayStringNuloVazioComEspacosBranco.Execute(array);
            return vazio;
        }

        public static bool Execute(this System.DBNull texto)
        {
            return (texto == System.DBNull.Value) || (texto is null);
        }
    }
}
