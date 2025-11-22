using Etiquetas.Bibliotecas.Comum.Arrays;
using Etiquetas.Bibliotecas.Comum.Evento;
using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    public class StreamBaseTCPCliente : StreamBase
    {
        protected TCPClient ClienteTCP { get; set; }

        public StreamBaseTCPCliente()
        {
            // Constructor logic here
        }

        public override async Task ConectarAsync(params object[] parametros)
        {
            ThrowIfDisposed();

            if (parametros == null)
            {
                throw new ArgumentNullException("Parâmetros inválidos!");
            }

            var posicaoint = 0;
            string serverIpAdress = string.Empty;
            int serverPort = 0;
            int timeout = 0;
            var posicaoCancellationToken = 0;
            var cancellationTokenBruto = CancellationToken.None;
            var cancellationTokenStop = CancellationToken.None;

            foreach (var item in parametros)
            {
                switch (item)
                {
                    case string texto:
                        serverIpAdress = texto;
                        break;
                    case int inteiro:
                        switch (posicaoint)
                        {
                            case 0:
                                serverPort = inteiro > 0
                                ? inteiro
                                : throw new ArgumentOutOfRangeException("Porta inválida!");
                                posicaoint++;
                                break;
                            case 1:
                                timeout = inteiro;
                                posicaoint++;
                                break;
                            default:
                                break;
                        }
                        break;
                    case TCPClient clienteTCP:
                        this.ClienteTCP = clienteTCP;
                        break;
                    case CancellationToken token:
                        switch (posicaoCancellationToken)
                        {
                            case 0:
                                cancellationTokenBruto = token;
                                posicaoCancellationToken++;
                                break;
                            case 1:
                                cancellationTokenStop = token;
                                posicaoCancellationToken++;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            var hasServerIpAdress = !StringEhNuloVazioComEspacosBranco.Execute(serverIpAdress);
            var hasServerPort = serverPort > 0;

            if (hasServerIpAdress && hasServerPort)
            {
                await ConnectAsync(serverIpAdress, serverPort, timeout, cancellationBruto).ConfigureAwait(false);
                return;
            }

            if (TCPClient == default)
            {
                throw new ArgumentNullException("Ip Adress e TcpClient não informado!");
            }
            await ConnectAsync(cancellationBruto).ConfigureAwait(false);
            return;
        }

        public override async Task ConectarAsync(ITaskParametros parametros)
        {
            ThrowIfDisposed();

            if (parametros == null)
            {
                throw new ArgumentNullException("Parâmetros inválidos!");
            }
            var cancellationToken = parametros.RetornaSeExistir<CancellationToken>("CancellationBruto");
            var timeout = parametros.RetornaSeExistir<int>("Timeout");

            this.TCPClient = parametros.RetornaSeExistir<TcpClient>("TcpClient");
            if (this.TCPClient == default)
            {
                var serverIpAdress = parametros.RetornaSeExistir<string>("ServerIpAdress");
                var serverPort = parametros.RetornaSeExistir<int>("ServerPort");
                var hasServerIpAdress = !StringEhNuloVazioComEspacosBranco.Execute(serverIpAdress);
                var hasServerPort = serverPort > 0;

                if (hasServerIpAdress && hasServerPort)
                {
                    await ConnectAsync(serverIpAdress, serverPort, timeout, cancellationToken).ConfigureAwait(false);
                    return;
                }

                throw new ArgumentNullException("Ip Adress e TcpClient não informado!");
            }

            await ConnectAsync(cancellationToken).ConfigureAwait(false);
            return;
        }

        public override Task ConectarReaderOnlyUnshareAsync()
        {
            throw new NotImplementedException();
        }

        public override Task ConectarWriterAndReaderUnshareAsync()
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc/>
        public override bool EstaAberto()
        {
        }

        /// <inheritdoc/>
        public override Task FecharAsync()
        {
        }

        /// <inheritdoc/>
        public override bool PossuiDados()
        {
        }

    }
}
