using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.GUI.Texto
{
    public class TCPClient1
    {
        public TCPClient1()
        {
        }

        protected async Task<bool> ConnectHostPortTimeout(
            string host,
            int port,
            int timeoutMs)
        {
            var tcpTask = Task.Run(async () =>
            {
                var tcpClient = new TcpClient();
                var connectTask = tcpClient.ConnectAsync(host, port);
                await connectTask.ConfigureAwait(false);
                return true;
            });

            return await tcpTask.ConfigureAwait(false);
        }
    }
}
