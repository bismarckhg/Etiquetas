using Etiqueta.Bibliotecas.TaskCorePipe.Enums;
using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Modelos
{
    /// <summary>
    /// Representa a resposta a um comando, enviada como payload de uma MensagemPipe.
    /// </summary>
    public class RespostaPipe
    {
        /// <summary>
        /// O código de resposta que indica o resultado da operação.
        /// </summary>
        public CodigoResposta CodigoResposta { get; set; }

        /// <summary>
        /// Os dados de resposta da operação. Pode ser nulo se não houver dados a retornar.
        /// </summary>
        public object Dados { get; set; }

        /// <summary>
        /// Uma mensagem descritiva sobre o resultado, especialmente útil em caso de erro.
        /// </summary>
        public string MensagemErro { get; set; }
    }
}
