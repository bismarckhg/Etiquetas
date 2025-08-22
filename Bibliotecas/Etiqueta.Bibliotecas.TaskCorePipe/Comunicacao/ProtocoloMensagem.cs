using Etiqueta.Bibliotecas.TaskCorePipe.Interfaces;
using Etiqueta.Bibliotecas.TaskCorePipe.Modelos;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Comunicacao
{
    /// <summary>
    /// Gerencia a formatação e o parsing de mensagens de acordo com o protocolo do pipe.
    /// </summary>
    public class ProtocoloMensagem
    {
        private readonly ISerializadorMensagem _serializador;
        private const int TamanhoHeader = 4; // 4 bytes para o tamanho da mensagem em Int32

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ProtocoloMensagem"/>.
        /// </summary>
        /// <param name="serializador">A implementação do serializador a ser usada para as mensagens.</param>
        public ProtocoloMensagem(ISerializadorMensagem serializador)
        {
            _serializador = serializador;
        }

        /// <summary>
        /// Escreve uma mensagem formatada com cabeçalho de tamanho no stream.
        /// </summary>
        /// <param name="stream">O stream para escrever a mensagem.</param>
        /// <param name="mensagem">O objeto MensagemPipe a ser enviado.</param>
        public async Task EscreverMensagemAsync(Stream stream, MensagemPipe mensagem)
        {
            var jsonString = _serializador.Serializar(mensagem);
            var buffer = Encoding.UTF8.GetBytes(jsonString);
            var tamanho = buffer.Length;
            var header = System.BitConverter.GetBytes(tamanho);

            // Escreve o cabeçalho com o tamanho
            await stream.WriteAsync(header, 0, TamanhoHeader);
            // Escreve o corpo da mensagem
            await stream.WriteAsync(buffer, 0, tamanho);
            await stream.FlushAsync();
        }

        /// <summary>
        /// Lê uma mensagem formatada com cabeçalho de tamanho do stream.
        /// </summary>
        /// <param name="stream">O stream para ler a mensagem.</param>
        /// <returns>O objeto MensagemPipe lido, ou null se o stream for fechado.</returns>
        public async Task<MensagemPipe> LerMensagemAsync(Stream stream)
        {
            var headerBuffer = new byte[TamanhoHeader];
            var bytesLidos = await stream.ReadAsync(headerBuffer, 0, TamanhoHeader);

            if (bytesLidos < TamanhoHeader)
            {
                // Conexão fechada ou dados insuficientes
                return null;
            }

            var tamanhoMensagem = System.BitConverter.ToInt32(headerBuffer, 0);
            var corpoBuffer = new byte[tamanhoMensagem];
            bytesLidos = await stream.ReadAsync(corpoBuffer, 0, tamanhoMensagem);

            if (bytesLidos < tamanhoMensagem)
            {
                // Conexão fechada inesperadamente
                return null;
            }

            var jsonString = Encoding.UTF8.GetString(corpoBuffer);
            return _serializador.Deserializar<MensagemPipe>(jsonString);
        }
    }
}
