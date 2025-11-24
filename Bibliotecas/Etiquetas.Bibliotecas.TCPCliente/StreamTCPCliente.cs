using Etiquetas.Bibliotecas.Comum.Geral;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente
{

    public class StreamTCPCliente : StreamBaseTCPCliente, IStreamLeitura, IStreamEscrita
    {
        protected NetworkStream NetStream;

        /// <summary>
        /// Leitura assíncrona de dados do stream TCP Cliente.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parametros"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<T> LerAsync<T>(ITaskParametros parametros)
        {
            if (typeof(T) == typeof(byte[]))
            {
                var retorno = this.ClienteTCP.LerBufferAsync();
                return (Task<T>)(object)retorno;
            }

            throw new InvalidCastException($"Tipo de retorno não suportado: {typeof(T).FullName}");
        }

        /// <summary>
        /// Gravação assíncrona de dados no stream TCP Cliente.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parametros"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task EscreverAsync<T>(ITaskParametros parametros)
        {
            if (typeof(T) == typeof(byte[]))
            {
                var buffer = parametros.RetornaSeExistir<byte[]>("buffer");
                var timeout = parametros.RetornaSeExistir<int>("timeout");
                var addLineBreak = parametros.RetornaSeExistir<bool>("addLineBreak");

                await this.ClienteTCP.GravarBufferAsync(
                    buffer,
                    timeout,
                    addLineBreak
                );
            }
        }
    }
}
