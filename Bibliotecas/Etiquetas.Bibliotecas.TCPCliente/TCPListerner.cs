using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    public class TCPListerner
    {
        protected TcpListener Listerner;
        protected CancellationToken CancellationTokenBreak { get; }
        protected CancellationToken CancellationTokenStop { get; }

        public TCPListerner(CancellationToken cancellationTokenBreak, CancellationToken cancellationTokenStop)
        {
            this.CancellationTokenStop = cancellationTokenStop;
            this.CancellationTokenBreak = cancellationTokenBreak;
        }

        protected async Task<TcpClient> StartListener(System.Net.IPAddress ipAdress, int port)
        {
            Listerner = new TcpListener(ipAdress, port);
            Listerner.Start();

            try
            {
                CancellationTokenBreak.ThrowIfCancellationRequested();
                while (CancellationTokenBreak.IsCancellationRequested)
                {
                    var client = await AcceptTcpClientAsync(this.Listerner).ConfigureAwait(false);

                    if (client != null)
                    {
                        return client;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<TcpClient> AcceptTcpClientAsync(TcpListener listener)
        {
            CancellationTokenBreak.Register(() => listener?.Stop());

            // Cria uma task que completa quando o token é cancelado
            var acceptTask = listener.AcceptTcpClientAsync();
            var cancellationTask = Task.Delay(Timeout.Infinite, CancellationTokenBreak);

            var completedTask = await Task.WhenAny(acceptTask, cancellationTask);

            if (completedTask == cancellationTask)
            {
                // Cancelamento solicitado - para o listener
                    return null;
            }

            return await acceptTask;
        }

    }
}
