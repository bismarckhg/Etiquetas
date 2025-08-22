using System;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Interfaces
{
    /// <summary>
    /// Define o contrato para o servidor pipe controlador, que gerencia todas as tarefas e a comunicação.
    /// </summary>
    public interface IPipeControlador : IDisposable
    {
        /// <summary>
        /// Inicia o servidor, que começa a escutar por conexões e comandos.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação de inicialização.</returns>
        Task IniciarAsync();

        /// <summary>
        /// Para o servidor e libera todos os recursos.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação de parada.</returns>
        Task PararAsync();

        /// <summary>
        /// Envia um comando para uma tarefa específica.
        /// </summary>
        /// <param name="nomePipe">O nome do pipe da tarefa de destino.</param>
        /// <param name="comando">O comando a ser enviado.</param>
        /// <returns>A resposta da tarefa.</returns>
        Task<object> EnviarComandoAsync(string nomePipe, IComandoPipe comando);
    }
}
