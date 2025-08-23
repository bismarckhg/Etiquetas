using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Json
{
    /// <summary>
    /// Fornece uma implementação de stream para ler e escrever objetos em formato JSON.
    /// </summary>
    /// <typeparam name="T">O tipo de objeto a ser serializado ou desserializado.</typeparam>
    public class StreamJson<T> : StreamStreamBase, IStreamLeitura<T>, IStreamEscrita<T> where T : class
    {
        private readonly string _caminhoArquivo;
        private readonly JsonSerializerOptions _options;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="StreamJson{T}"/>.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho completo para o arquivo JSON.</param>
        public StreamJson(string caminhoArquivo)
        {
            if (string.IsNullOrEmpty(caminhoArquivo))
                throw new ArgumentNullException(nameof(caminhoArquivo));

            _caminhoArquivo = caminhoArquivo;
            _options = new JsonSerializerOptions { WriteIndented = true };
        }

        /// <summary>
        /// Lê e desserializa um objeto de um arquivo JSON de forma assíncrona.
        /// </summary>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        /// <returns>O objeto desserializado do arquivo, ou null se o arquivo não existir ou estiver vazio.</returns>
        public async Task<T> LerAsync(params object[] parametros)
        {
            if (!File.Exists(_caminhoArquivo))
            {
                return null;
            }

            var jsonString = await Task.Run(() => File.ReadAllText(_caminhoArquivo)).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(jsonString, _options);
        }

        /// <summary>
        /// Serializa e escreve um objeto em um arquivo JSON de forma assíncrona.
        /// </summary>
        /// <param name="dados">O objeto a ser escrito no arquivo.</param>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        public async Task EscreverAsync(T dados, params object[] parametros)
        {
            var jsonString = JsonSerializer.Serialize(dados, _options);
            await Task.Run(() => File.WriteAllText(_caminhoArquivo, jsonString)).ConfigureAwait(false);
        }

        public override bool EstaAberto()
        {
            // O "estado aberto" é transiente; consideramos pronto se o caminho do arquivo for válido.
            // A verificação real da existência do arquivo é feita nas operações de leitura/escrita.
            return !string.IsNullOrEmpty(_caminhoArquivo);
        }

        public override bool PossuiDados()
        {
            try
            {
                if (File.Exists(_caminhoArquivo))
                {
                    return new FileInfo(_caminhoArquivo).Length > 0;
                }
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        public override Task ConectarAsync(params object[] parametros) => Task.CompletedTask;

        public override Task FecharAsync() => Task.CompletedTask;

        protected override void Dispose(bool disposing)
        {
            // Nenhum recurso não gerenciado para descartar neste modelo.
            base.Dispose(disposing);
        }
    }
}
