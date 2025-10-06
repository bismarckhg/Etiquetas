using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Enums
{
    /// <summary>
    /// Define os tipos de mensagens trocadas no protocolo de pipe.
    /// </summary>
    public enum TipoMensagem
    {
        /// <summary>
        /// Uma mensagem que representa um comando a ser executado.
        /// </summary>
        Command,

        /// <summary>
        /// Uma mensagem que representa a resposta a um comando.
        /// </summary>
        Response,

        /// <summary>
        /// Uma mensagem contendo uma atualização de status.
        /// </summary>
        Status,

        /// <summary>
        /// Uma mensagem que representa um erro ocorrido.
        /// </summary>
        Error,

        /// <summary>
        /// Uma mensagem de pulsação para verificar a conectividade.
        /// </summary>
        Heartbeat,

        /// <summary>
        /// Uma mensagem de notificação geral.
        /// </summary>
        Notification
    }
}
