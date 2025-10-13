using Etiquetas.Bibliotecas.Streams.Interfaces;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{
    public class StreamTCPCliente : StreamBaseTCPCliente, IStreamLeitura, IStreamEscrita
    {
        public Task<T> LerAsync<T>(params object[] parametros)
        {
            throw new NotImplementedException();
        }

        public Task<T> LerAsync<T>(ITaskParametros parametros)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Envia dados para o servidor TCP de forma assíncrona
        /// </summary>
        /// <param name="tcpClient">Cliente TCP conectado</param>
        /// <param name="message">Mensagem a ser enviada</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        private async Task SendDataAsync(TcpClient tcpClient, string message, CancellationToken cancellationToken)
        {
            try
            {
                using (var networkStream = tcpClient.GetStream())
                {
                    // Converte a mensagem para bytes
                    var data = Encoding.UTF8.GetBytes(message);

                    // .NET 4.5 não tem WriteAsync nativo para NetworkStream, usamos Task.Run
                    await Task.Run(() =>
                    {
                        networkStream.Write(data, 0, data.Length);
                        networkStream.Flush();
                    }, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao enviar dados: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lê dados de uma conexão TCP de forma assíncrona
        /// </summary>
        /// <param name="tcpClient">Cliente TCP</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Dados lidos da conexão</returns>
        private async Task<string> ReadDataFromConnectionAsync(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            try
            {
                using (var networkStream = tcpClient.GetStream())
                {
                    var buffer = new byte[_config.BufferSize];
                    var stringBuilder = new StringBuilder();

                    // Lê dados até não haver mais ou timeout
                    while (networkStream.DataAvailable || stringBuilder.Length == 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // .NET 4.5 não tem ReadAsync nativo para NetworkStream, usamos Task.Run
                        var bytesRead = await Task.Run(() =>
                        {
                            try
                            {
                                return networkStream.Read(buffer, 0, buffer.Length);
                            }
                            catch (IOException)
                            {
                                return 0; // Conexão fechada
                            }
                        }, cancellationToken).ConfigureAwait(false);

                        if (bytesRead == 0)
                            break; // Conexão fechada

                        // Converte bytes para string
                        var data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        stringBuilder.Append(data);

                        // Se não há mais dados disponíveis, sai do loop
                        if (!networkStream.DataAvailable)
                            break;
                    }

                    return stringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred(ex);
                return null;
            }
        }

        public Task EscreverAsync<T>(params object[] parametros)
        {
            throw new NotImplementedException();
        }

        public Task EscreverAsync<T>(ITaskParametros parametros)
        {
            throw new NotImplementedException();
        }

    }
}
