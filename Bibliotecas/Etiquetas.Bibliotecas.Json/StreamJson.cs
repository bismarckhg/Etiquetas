using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;
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
        private readonly JsonSerializerSettings _settings;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="StreamJson{T}"/>.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho completo para o arquivo JSON.</param>
        /// <param name="settings">Configurações opcionais do Json.NET.</param>
        public StreamJson(string caminhoArquivo, JsonSerializerSettings settings = null)
        {
            if (string.IsNullOrWhiteSpace(caminhoArquivo))
                throw new ArgumentNullException(nameof(caminhoArquivo));

            _caminhoArquivo = caminhoArquivo;

            // Padrão: JSON identado, mantendo nomes como no C# (sem camelCase).
            _settings = settings ?? new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new DefaultContractResolver()
            };
        }

        /// <summary>
        /// Lê e desserializa um objeto de um arquivo JSON de forma assíncrona.
        /// </summary>
        public async Task<T> LerAsync(params object[] parametros)
        {
            if (!File.Exists(_caminhoArquivo))
                return null;

            var jsonString = await Task.Run(
                () => File.ReadAllText(_caminhoArquivo, Encoding.UTF8)
            ).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(jsonString))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString, _settings);
            }
            catch (JsonException)
            {
                // Opcional: logar o erro/arquivo inválido
                return null;
            }
        }

        /// <summary>
        /// Serializa e escreve um objeto em um arquivo JSON de forma assíncrona.
        /// </summary>
        public async Task EscreverAsync(T dados, params object[] parametros)
        {
            if (dados == null) throw new ArgumentNullException(nameof(dados));

            var dir = Path.GetDirectoryName(_caminhoArquivo);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var jsonString = JsonConvert.SerializeObject(dados, _settings);

            // UTF-8 sem BOM para padronizar entre .NET Framework e .NET Core
            var utf8SemBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

            await Task.Run(
                () => File.WriteAllText(_caminhoArquivo, jsonString, utf8SemBom)
            ).ConfigureAwait(false);
        }

        public override bool EstaAberto() => !string.IsNullOrEmpty(_caminhoArquivo);

        public override bool PossuiDados()
        {
            try
            {
                return File.Exists(_caminhoArquivo) && new FileInfo(_caminhoArquivo).Length > 0;
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
