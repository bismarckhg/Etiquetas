using System;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Interfaces
{
    /// <summary>
    /// Define o contrato para o pipe de uma tarefa individual.
    /// </summary>
    public interface IPipeTarefa : IDisposable
    {
        /// <summary>
        /// Inicia o pipe da tarefa, que começa a escutar por comandos do servidor.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação de inicialização.</returns>
        Task IniciarAsync();

        /// <summary>
        /// Para o pipe da tarefa e libera os recursos.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação de parada.</returns>
        Task PararAsync();

        /// <summary>
        /// Evento acionado quando um comando é recebido do servidor.
        /// </summary>
        event Func<IComandoPipe, Task<object>> ComandoRecebido;
    }
}
