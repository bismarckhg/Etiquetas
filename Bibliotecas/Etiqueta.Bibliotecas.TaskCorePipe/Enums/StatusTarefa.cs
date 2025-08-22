using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Enums
{
    /// <summary>
    /// Define os possíveis estados de uma tarefa durante seu ciclo de vida no sistema de pipe.
    /// </summary>
    public enum StatusTarefa
    {
        /// <summary>
        /// A tarefa está sendo criada e configurada.
        /// </summary>
        Inicializando,
        /// <summary>
        /// A tarefa está tentando estabelecer a conexão de pipe com o servidor.
        /// </summary>
        PipeConectando,
        /// <summary>
        /// A conexão de pipe foi estabelecida e está funcional.
        /// </summary>
        PipeConectado,
        /// <summary>
        /// A lógica principal da tarefa está em execução normal.
        /// </summary>
        Executando,
        /// <summary>
        /// A tarefa está ativa e aguardando comandos via pipe.
        /// </summary>
        AguardandoComando,
        /// <summary>
        /// A tarefa está processando um comando recebido via pipe.
        /// </summary>
        ProcessandoComando,
        /// <summary>
        /// A tarefa está executando uma parada suave (graceful shutdown).
        /// </summary>
        ParandoSuave,
        /// <summary>
        /// A tarefa está executando uma parada forçada (forced termination).
        /// </summary>
        ParandoBruto,
        /// <summary>
        /// A conexão de pipe com a tarefa foi perdida.
        /// </summary>
        PipeDesconectado,
        /// <summary>
        /// A tarefa foi completada com sucesso.
        /// </summary>
        Finalizada,
        /// <summary>
        /// A tarefa terminou com um erro.
        /// </summary>
        Erro,
        /// <summary>
        /// A tarefa foi cancelada pelo usuário ou sistema.
        /// </summary>
        Cancelada
    }
}
