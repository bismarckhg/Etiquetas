using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using Etiqueta.Bibliotecas.TaskCorePipe.Enums;
using Etiqueta.Bibliotecas.TaskCorePipe.Modelos;
using Etiqueta.Bibliotecas.TaskCorePipe.Servidor;
using Etiqueta.Bibliotecas.TaskCorePipe.Tarefa;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Etiquetas.Bibliotecas.TaskCore;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Tests
{
    public class TasksGruposPipeTests
    {
        [Fact]
        public async Task EnviarComando_Break_Deve_Cancelar_Task()
        {
            // Arrange
            var nomePipe = $"teste-pipe-{Guid.NewGuid()}";
            var servidor = new ServidorPipeControlador();
            await servidor.IniciarAsync(); // Adquire o Mutex

            var tarefaCancelada = false;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            var pipeTask = new TasksGruposPipe(nomePipe);
            await pipeTask.AdicionarTask(1, async (p) => {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), p.RetornoCancellationTokenSource().Token);
                }
                catch (OperationCanceledException)
                {
                    tarefaCancelada = true;
                    throw;
                }
                finally
                {
                    taskCompletionSource.SetResult(true);
                }
                return null;
            }, new TaskParametrosStub());

            pipeTask.IniciarExecucao();

            // Act
            // Pequeno delay para garantir que o pipe server está escutando
            await Task.Delay(100);
            var comando = new ComandoPipe { Comando = TipoComando.BREAK };
            await servidor.EnviarComandoAsync(nomePipe, comando);

            // Espera a task terminar (seja por cancelamento ou conclusão)
            await taskCompletionSource.Task;

            // Assert
            Assert.True(tarefaCancelada, "A tarefa deveria ter sido cancelada pelo comando BREAK.");

            // Cleanup
            pipeTask.Dispose();
            servidor.Dispose();
        }
    }

    // Stub para os parâmetros da task, herdando a implementação concreta.
    public class TaskParametrosStub : TaskReturnValue
    {
        public TaskParametrosStub() : base()
        {
            // Nenhuma lógica adicional é necessária para este stub,
            // pois a classe base TaskReturnValue já fornece toda a implementação necessária.
        }
    }
}
