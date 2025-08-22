using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TaskCore
{
    public class SingleThreadTaskScheduler : TaskScheduler, IDisposable
    {
        private readonly Thread ThreadPrincipal;
        private readonly BlockingCollection<Task> Tasks = new BlockingCollection<Task>();

        public SingleThreadTaskScheduler()
        {
            ThreadPrincipal = new Thread(new ThreadStart(Execute))
            {
                IsBackground = true
            };
            ThreadPrincipal.Start();
        }

        private void Execute()
        {
            foreach (var task in Tasks.GetConsumingEnumerable())
            {
                TryExecuteTask(task);
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Tasks.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            Tasks.Add(task);
        }

        // Impede execução inline para manter a execução na thread dedicada
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        public void Dispose()
        {
            Tasks.CompleteAdding();
        }
    }

}
