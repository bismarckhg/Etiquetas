using System;
using System.Threading;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Comunicacao
{
    /// <summary>
    /// Fornece funcionalidade para executar tarefas com um timeout.
    /// </summary>
    public static class GerenciadorTimeout
    {
        /// <summary>
        /// Executa uma tarefa com um timeout especificado.
        /// </summary>
        /// <typeparam name="TResult">O tipo de resultado da tarefa.</typeparam>
        /// <param name="tarefa">A tarefa a ser executada.</param>
        /// <param name="timeout">O tempo máximo de espera.</param>
        /// <returns>O resultado da tarefa.</returns>
        /// <exception cref="TimeoutException">Lançada se a tarefa não for concluída dentro do tempo especificado.</exception>
        public static async Task<TResult> ExecutarComTimeoutAsync<TResult>(Task<TResult> tarefa, TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource())
            {
                var tarefaConcluida = await Task.WhenAny(tarefa, Task.Delay(timeout, cts.Token));

                if (tarefaConcluida == tarefa)
                {
                    // A tarefa foi concluída a tempo, cancele o Task.Delay.
                    cts.Cancel();
                    return await tarefa;
                }
                else
                {
                    // O timeout foi atingido primeiro.
                    throw new TimeoutException("A operação atingiu o tempo limite.");
                }
            }
        }
    }
}
