using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Enums
{
    /// <summary>
    /// Define os tipos de comandos que podem ser enviados através do sistema de pipe.
    /// </summary>
    public enum TipoComando
    {
        /// <summary>
        /// Solicita a parada suave (graceful shutdown) de uma tarefa.
        /// </summary>
        STOP,

        /// <summary>
        /// Solicita a parada forçada (forced termination) de uma tarefa.
        /// </summary>
        BREAK,

        /// <summary>
        /// Realiza uma verificação de saúde (health check) para verificar se a tarefa está responsiva.
        /// </summary>
        PING,

        /// <summary>
        /// Solicita o estado detalhado de uma tarefa específica.
        /// </summary>
        STATUS,

        /// <summary>
        /// Solicita a lista de todas as tarefas gerenciadas pelo servidor.
        /// </summary>
        LIST
    }
}
