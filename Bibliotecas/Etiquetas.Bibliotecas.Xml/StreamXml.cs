using Etiquetas.Bibliotecas.Comum.Geral;
using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Etiquetas.Bibliotecas.Xml
{
    /// <summary>
    /// Fornece uma implementação de FS para leitura e escrita de arquivos XML.
    /// </summary>
    public class StreamXml<T> : StreamBaseTXT, IStreamLeitura<T>, IStreamEscrita<T>
    {
        private readonly XmlWriterSettings SettingsWriter;
        private readonly XmlReaderSettings SettingsReader;

        private CancellationToken CancelToken { get; set; }

        private Encoding EncodingTexto { get; set; } = ConversaoEncoding.UTF8BOM;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="StreamXml"/>.
        /// </summary>
        public StreamXml() : this(null, null)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="StreamXml"/>.
        /// </summary>
        public StreamXml(XmlReaderSettings settings) : this(settings, null)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="StreamXml"/>.
        /// </summary>
        public StreamXml(XmlWriterSettings settings) : this(null, settings)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="StreamXml"/>.
        /// </summary>
        public StreamXml(XmlReaderSettings settingsReader = null, XmlWriterSettings settingsWriter = null) : base()
        {
            this.CancelToken = CancellationToken.None;
            this.EncodingTexto = Encoding.UTF8;

            SettingsReader = settingsReader ?? new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true,
                CloseInput = false,
                Async = true
            };

            SettingsWriter = settingsWriter ?? new XmlWriterSettings
            {
                Encoding = this.EncodingTexto,
                OmitXmlDeclaration = false,
                Indent = true,
                IndentChars = "  ",
                CloseOutput = false,
                Async = true
            };
        }

        /// <summary>
        /// As conexões são tratadas por operação, então este método não realiza nenhuma ação.
        /// </summary>
        public override Task ConectarAsync(params object[] parametros) => Task.CompletedTask;

        /// <summary>
        /// O fechamento é tratado por operação, então este método não realiza nenhuma ação.
        /// </summary>
        public override Task FecharAsync() => Task.CompletedTask;

        /// <summary>
        /// Lê o conteúdo do arquivo XML de forma assíncrona e o retorna como um <see cref="XDocument"/>.
        /// </summary>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        /// <returns>Um <see cref="XDocument"/> com o conteúdo do arquivo, ou null se o arquivo não existir.</returns>
        public async Task<T> LerAsync(params object[] parametros)
        {
            if (!File.Exists(NomeECaminhoArquivo))
            {
                return default;
            }
            return default;
        }

        /// <summary>
        /// Lê o conteúdo do arquivo XML de forma assíncrona e o retorna como um <see cref="XDocument"/>.
        /// </summary>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        /// <returns>Um <see cref="XDocument"/> com o conteúdo do arquivo, ou null se o arquivo não existir.</returns>
        public async Task<T> LerAsync(ITaskParametros parametros)
        {
            if (!File.Exists(NomeECaminhoArquivo))
            {
                return default;
            }
            return default;
        }

        protected async Task<string> LerAsync(T objeto)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var sr = new StreamReader(
                FS,
                EncodingTexto,
                detectEncodingFromByteOrderMarks: false
                )
            )   // ANSI não usa BOM; força 1252 ))
            using (var xr = XmlReader.Create(sr, new XmlReaderSettings { Async = true }))
            {
                this.CancelToken.ThrowIfCancellationRequested();
                return await sr.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Escreve o <see cref="XDocument"/> fornecido para o arquivo de forma assíncrona.
        /// Se o arquivo já existir, ele será sobrescrito.
        /// </summary>
        /// <param name="dados">O <see cref="XDocument"/> a ser salvo.</param>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        public async Task EscreverAsync(params object[] parametros)
        {
            //if (dados == null)
            //    throw new ArgumentNullException(nameof(dados));

            //await Task.Run(() => dados.Save(_caminhoArquivo, SaveOptions.None)).ConfigureAwait(false);
        }

        /// <summary>
        /// Escreve o <see cref="XDocument"/> fornecido para o arquivo de forma assíncrona.
        /// Se o arquivo já existir, ele será sobrescrito.
        /// </summary>
        /// <param name="dados">O <see cref="XDocument"/> a ser salvo.</param>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        public async Task EscreverAsync(ITaskParametros parametros)
        {
            //if (dados == null)
            //    throw new ArgumentNullException(nameof(dados));

            //await Task.Run(() => dados.Save(_caminhoArquivo, SaveOptions.None)).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifica se o arquivo XML existe no caminho especificado.
        /// </summary>
        /// <returns>Verdadeiro se o arquivo existir, caso contrário, falso.</returns>
        public override bool EstaAberto()
        {
            return File.Exists(NomeECaminhoArquivo);

        }

        /// <summary>
        /// Verifica se o arquivo XML existe e não está vazio.
        /// </summary>
        /// <returns>Verdadeiro se o arquivo existir e tiver um tamanho maior que zero, caso contrário, falso.</returns>
        public override bool PossuiDados()
        {
            try
            {
                if (File.Exists(NomeECaminhoArquivo))
                {
                    return new FileInfo(NomeECaminhoArquivo).Length > 0;
                }
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Libera os recursos utilizados pela classe.
        /// </summary>
        /// <param name="disposing">Indica se a liberação está sendo feita de forma explícita.</param>
        protected override void Dispose(bool disposing)
        {
            // Nenhuma recurso não gerenciado para liberar neste modelo.
            base.Dispose(disposing);
        }

    }
}
