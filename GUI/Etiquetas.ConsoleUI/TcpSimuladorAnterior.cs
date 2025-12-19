using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.ConsoleUI
{
    /// <summary>
    /// Simulador de cliente TCP para testes.
    /// </summary>
    public static class TcpSimulatorAnterior
    {
        /// <summary>
        /// Cria um <see cref="System.Net.Sockets.TcpClient"/> conectado a um servidor TCP local que simula o envio
        /// de uma sequência de mensagens.
        /// </summary>
        /// <remarks>O <see cref="System.Net.Sockets.TcpClient"/> retornado é imediatamente conectado a um
        /// servidor TCP local que começa a enviar mensagens de bytes aleatórios. Cada mensagem termina com o valor de byte 0x03 (ETX).
        /// O servidor fecha a conexão após o envio de todas as mensagens. <para> Este método destina-se a testes ou
        /// cenários de simulação em que um cliente precisa receber um fluxo de mensagens de um servidor TCP. </para> <para>
        /// O chamador é responsável por descartar o <see cref="System.Net.Sockets.TcpClient"/> retornado quando ele
        /// não for mais necessário. </para></remarks>
        /// <param name="totalMessages">O número total de mensagens a serem enviadas pelo servidor simulado. Deve ser não negativo. O padrão é 100.</param>
        /// <param name="bytesPerMessage">O tamanho, em bytes, de cada mensagem enviada pelo servidor simulado. Deve ser positivo. O padrão é 256.</param>
        /// <param name="intervalMs">O intervalo, em milissegundos, entre cada mensagem enviada pelo servidor simulado. Deve ser não negativo. O padrão é 10.</param>
        /// <returns>Uma instância de <see cref="System.Net.Sockets.TcpClient"/> conectada a um servidor local. O servidor irá
        /// enviar de forma assíncrona <paramref name="totalMessages"/> mensagens, cada uma com <paramref name="bytesPerMessage"/>
        /// bytes, no intervalo especificado de <paramref name="intervalMs"/>.</returns>
        public static System.Net.Sockets.TcpClient CreateSimulatedClient(int totalMessages = 100, int bytesPerMessage = 256, int intervalMs = 10)
        {
            var listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Loopback, 0);
            listener.Start();
            int port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;

            var client = new System.Net.Sockets.TcpClient();
            var connect = client.ConnectAsync(System.Net.IPAddress.Loopback, port);
            var serverTask = listener.AcceptTcpClientAsync();
            Task.WaitAll(connect);
            var server = serverTask.Result;

            Task.Run(async () =>
            {
                using (var ns = server.GetStream())
                {
                    var rnd = new Random();
                    for (int i = 0; i < totalMessages; i++)
                    {
                        var payload = new byte[bytesPerMessage];
                        rnd.NextBytes(payload);
                        payload[payload.Length - 1] = 0x03; // ETX
                        await ns.WriteAsync(payload, 0, payload.Length).ConfigureAwait(false);
                        await Task.Delay(intervalMs).ConfigureAwait(false);
                    }

                    server.Close();
                    listener.Stop();
                }
            });

            return client;
        }
    }
}
