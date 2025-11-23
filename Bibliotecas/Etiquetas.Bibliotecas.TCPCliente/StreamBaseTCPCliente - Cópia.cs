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
    /// <summary>
    /// class
    /// </summary>
    public class StreamBaseTCPCliente : StreamBase
    {
        protected TCPClient ClienteTCP { get; set; } = default;

        /// <summary>
        /// ctor
        /// </summary>
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
            int timeout = Timeout.Infinite;
            int bufferSize = 8192;
            var posicaoCancellationToken = 0;
            var cancellationTokenBruto = CancellationToken.None;
            var cancellationTokenStop = CancellationToken.None;
            var throwOnTimeout = false;
            Encoding encoding = null;

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
                                bufferSize = inteiro;
                                posicaoint++;
                                break;
                            case 2:
                                timeout = inteiro;
                                posicaoint++;
                                break;
                            default:
                                break;
                        }
                        break;
                    case TCPClient client:
                        this.ClienteTCP = client;
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
                    case bool booleano:
                        throwOnTimeout = booleano;
                        break;
                    default:
                        break;
                }
            }

            var hasServerIpAdress = !StringEhNuloVazioComEspacosBranco.Execute(serverIpAdress);
            var hasServerPort = serverPort > 0;

            this.ClienteTCP = this.ClienteTCP ?? new TCPClient(cancellationTokenStop, cancellationTokenBruto, bufferSize, timeout, throwOnTimeout, encoding);

            if (hasServerIpAdress && hasServerPort)
            {
                //await ConnectAsync(serverIpAdress, serverPort, timeout, cancellationBruto).ConfigureAwait(false);
                await this.ClienteTCP.ConnectAsync(serverIpAdress, serverPort).ConfigureAwait(false);
                return;
            }

            if (Client == default)
            {
                throw new ArgumentNullException("Ip Adress e TcpClient não informado!");
            }

            //await ConnectAsync(cancellationBruto).ConfigureAwait(false);
            await this.ClienteTCP.ConnectAsync(Client).ConfigureAwait(false);
            return;
        }

        public override async Task ConectarAsync(ITaskParametros parametros)
        {
            ThrowIfDisposed();

            if (parametros == null)
            {
                throw new ArgumentNullException("Parâmetros inválidos!");
            }

            this.ClienteTCP = parametros.RetornaSeExistir<TCPClient>("ClienteTCP");
            if (this.ClienteTCP == default)
            {
                var cancellationTokenBruto = parametros.RetornaSeExistir<CancellationToken>("cancellationTokenBruto");
                var cancellationTokenStop = parametros.RetornaSeExistir<CancellationToken>("cancellationTokenStop");
                var bufferSize = parametros.RetornaSeExistir<int>("bufferSize");
                var timeout = parametros.RetornaSeExistir<int>("timeout");
                var throwOnTimeout = parametros.RetornaSeExistir<bool>("throwOnTimeout");
                var encoding = parametros.RetornaSeExistir<Encoding>("encoding");
                this.ClienteTCP = new TCPClient(cancellationTokenStop, cancellationTokenBruto, bufferSize, timeout, throwOnTimeout, encoding);
            }
            
            if (this.ClienteTCP.ObtemTcpClient() != default)
            {
                this.Client = this.ClienteTCP.ObtemTcpClient();
            }
            else if (parametros.RetornaSeExistir<TcpClient>("TcpClient") != default)
            {
                this.Client = parametros.RetornaSeExistir<TcpClient>("TcpClient");
            }

            if (this.Client == default)
            {
                var serverIpAdress = parametros.RetornaSeExistir<string>("ServerIpAdress");
                var serverPort = parametros.RetornaSeExistir<int>("ServerPort");
                var hasServerIpAdress = !StringEhNuloVazioComEspacosBranco.Execute(serverIpAdress);
                var hasServerPort = serverPort > 0;

                if (hasServerIpAdress && hasServerPort)
                {
                    //await ConnectAsync(serverIpAdress, serverPort, timeout, cancellationToken).ConfigureAwait(false);
                    await this.ClienteTCP.ConnectAsync(serverIpAdress, serverPort).ConfigureAwait(false);
                    return;
                }

                throw new ArgumentNullException("Ip Adress e TcpClient não informado!");
            }

            //await ConnectAsync(cancellationToken).ConfigureAwait(false);
            await this.ClienteTCP.ConnectAsync(Client).ConfigureAwait(false);
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

        /// <summary>
        /// Retorna se a conexão está aberta.
        /// </summary>
        /// <returns>Retorna veradeiro se a conexão esta aberto.</returns>
        public override bool EstaAberto()
        {
            return this.ClienteTCP.EstaAberto();
        }

        /// <summary>
        /// Fecha a conexão TCP.
        /// </summary>
        public override Task FecharAsync()
        {
            return this.ClienteTCP.FecharAsync();
        }

        /// <summary>
        /// Verifica se existem dados disponíveis para leitura.
        /// </summary>
        /// <returns>Retorna true se existem dados disponiveis para leitura.</returns>
        public override bool PossuiDados()
        {
            return this.ClienteTCP.PossuiDados();
        }
    }
}
