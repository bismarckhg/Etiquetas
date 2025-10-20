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

        public TCPListerner()
        {
            
        }

        protected async Task<TcpClient> StartListener(System.Net.IPAddress ipAdress, int port, CancellationToken cancellationBreak)
        {
            Listerner = new TcpListener(ipAdress, port);
            Listerner.Start();

            try
            {
                cancellationBreak.ThrowIfCancellationRequested();
                while (cancellationBreak.IsCancellationRequested)
                {
                    var client = await AcceptTcpClientAsync(this.Listerner, cancellationBreak).ConfigureAwait(false);

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

        protected async Task<TcpClient> AcceptTcpClientAsync(TcpListener listener, CancellationToken cancellationBreak)
        {
            cancellationBreak.Register(() => listener.Stop());

            // Cria uma task que completa quando o token é cancelado
            var acceptTask = listener.AcceptTcpClientAsync();
            var cancellationTask = Task.Delay(Timeout.Infinite, cancellationBreak);

            var completedTask = await Task.WhenAny(acceptTask, cancellationTask);

            if (completedTask == cancellationTask)
            {
                // Cancelamento solicitado - para o listener
                    listener.Stop();
            }

            return await acceptTask;
        }

    }
}
