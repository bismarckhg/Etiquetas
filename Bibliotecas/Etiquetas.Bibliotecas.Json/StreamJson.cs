using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Streams.Core
{
    /// <summary>
    /// Fornece uma implementação de stream para ler e escrever objetos em formato JSON.
    /// </summary>
    /// <typeparam name="T">O tipo de objeto a ser serializado ou desserializado.</typeparam>
    public class StreamJson<T> : StreamBaseTXT, IStreamLeitura<T>, IStreamEscrita<T> where T : class
    {
        private readonly JsonSerializerSettings JsonSettings;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="StreamJson{T}"/>.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho completo para o arquivo JSON.</param>
        /// <param name="settings">Configurações opcionais do Json.NET.</param>
        public StreamJson(JsonSerializerSettings settings = null)
        {
            // Padrão: JSON identado, mantendo nomes como no C# (sem camelCase).
            JsonSettings = settings ?? new JsonSerializerSettings();
            JsonSettings.Formatting = Formatting.Indented;
            JsonSettings.NullValueHandling = NullValueHandling.Include;
            JsonSettings.ContractResolver = new DefaultContractResolver();
            
        }

        /// <summary>
        /// Lê e desserializa um objeto de um arquivo JSON de forma assíncrona.
        /// </summary>
        public async Task<T> LerAsync(params object[] parametros)
        {
            if (!File.Exists(NomeECaminhoArquivo))
                return null;

            //var jsonString = await Task.Run(
            //    () => File.ReadAllText(_caminhoArquivo, Encoding.UTF8)
            //).ConfigureAwait(false);

            var cancellationToken = CancellationToken.None;
            var encoding = Encoding.UTF8;

            foreach (var item in parametros)
            {
                if (item is CancellationToken ct)
                {
                    cancellationToken = ct;
                }

                if (item is Encoding enc)
                {
                    encoding = enc;
                }
            }

            var jsonString = string.Empty;
            using (var sr = new StreamReader(
                FS,
                encoding,
                detectEncodingFromByteOrderMarks: false
                )
            )   // ANSI não usa BOM; força 1252 ))
            {
                cancellationToken.ThrowIfCancellationRequested();
                jsonString = await sr.ReadToEndAsync().ConfigureAwait(false);
            }

            if (EhStringNuloVazioComEspacosBranco.Execute(jsonString))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString, JsonSettings);
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

            //var dir = Path.GetDirectoryName(NomeECaminhoArquivo);
            //if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            //    Directory.CreateDirectory(dir);

            var jsonString = JsonConvert.SerializeObject(dados, JsonSettings);

            // UTF-8 sem BOM para padronizar entre .NET Framework e .NET Core
            var utf8SemBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

            await Task.Run(
                () => File.WriteAllText(NomeECaminhoArquivo, jsonString, utf8SemBom)
            ).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            // Nenhum recurso não gerenciado para descartar neste modelo.
            base.Dispose(disposing);
        }
    }
}
