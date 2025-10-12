using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    public class StreamBaseTCPCliente : StreamBase
    {
        private TcpClient TCPClient;

        public StreamBaseTCPCliente()
        {
            // Constructor logic here
        }
        public override Task ConectarAsync(params object[] parametros)
        {
            throw new NotImplementedException();
        }

        public override Task ConectarAsync(ITaskParametros parametros)
        {
            throw new NotImplementedException();
        }

        public override Task ConectarReaderOnlyUnshareAsync()
        {
            throw new NotImplementedException();
        }

        public override Task ConectarWriterAndReaderUnshareAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="tcpClient">Cliente TCP</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        private async Task ConnectAsync(string serverIpAdress, int serverPort, int timeout, CancellationToken cancellationToken)
        {
            try
            {
                // .NET 4.5 não tem ConnectAsync nativo, usamos Task.Run com timeout
                var connectTask = Task.Run(() =>
                {
                    var ipAddress = System.Net.IPAddress.Parse(serverIpAdress);
                    TCPClient = new TcpClient();
                    TCPClient.Connect(ipAddress, serverPort);
                }, cancellationToken);

                // Aplica timeout de conexão
                var timeoutTask = Task.Delay(timeout, cancellationToken);
                var completedTask = await Task.WhenAny(connectTask, timeoutTask).ConfigureAwait(false);

                var conectado = EstaAberto();

                if (completedTask == timeoutTask && !conectado)
                {
                    throw new TimeoutException($"Timeout {timeout}ms ao tentar conectar com {serverIpAdress}:{serverPort}");
                }

                await connectTask; // Aguarda a conexão completar ou propagar exceção
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao conectar com {serverIpAdress}:{serverPort}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Conecta ao servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="tcpClient">Cliente TCP</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        private async Task ConnectAsync(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            try
            {
                // .NET 4.5 não tem ConnectAsync nativo, usamos Task.Run com timeout
                var connectTask = Task.Run(() =>
                {
                    TCPClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
                }, cancellationToken);

                var conectado = EstaAberto();

                if (conectado)
                {
                    throw new TimeoutException($"Timeout ao conectar com {_config.ServerIpAddress}:{_config.ServerPort}");
                }

                await connectTask; // Aguarda a conexão completar ou propagar exceção
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao conectar com {_config.ServerIpAddress}:{_config.ServerPort}: {ex.Message}", ex);
            }
        }

        public override bool EstaAberto()
        {
            return TCPClient?.Connected ?? false;
        }

        public override Task FecharAsync()
        {
            throw new NotImplementedException();
        }

        public override bool PossuiDados()
        {
            throw new NotImplementedException();
        }
    }
}
