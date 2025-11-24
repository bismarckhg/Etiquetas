using Etiquetas.Bibliotecas.Comum.Arrays;
using Etiquetas.Bibliotecas.Comum.Geral;
using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using Etiquetas.Bibliotecas.Xml.Servicos;
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
    public class StreamXml : StreamBaseTXT, IStreamLeitura, IStreamEscrita
    {
        private GenericXmlService XmlService;
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
            this.EncodingTexto = ConversaoEncoding.UTF8BOM;
            this.XmlService = new GenericXmlService();

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
        /// Lê o conteúdo do arquivo XML de forma assíncrona e o retorna como um <see cref="XDocument"/>.
        /// </summary>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        /// <returns>Um <see cref="XDocument"/> com o conteúdo do arquivo, ou null se o arquivo não existir.</returns>
        public async Task<T> LerAsync<T>(ITaskParametros parametros)
        {
            if (EstaAberto() == false)
            {
                await ConectarReaderOnlyUnshareAsync().ConfigureAwait(false);
            }
            this.CancelToken = parametros.RetornoCancellationToken;
            this.EncodingTexto = parametros.RetornoEncoding;
            var subRootName = parametros.RetornaSeExistir<string>("SubRootName");
            var itemNameSubRoot = parametros.RetornaSeExistir<string>("ItemNameSubRoot");
            var predicate = parametros.RetornaSeExistir<Func<T, bool>>("Predicate");

            var hasPredicate = predicate != null;
            var hasItemName = !StringEhNuloVazioComEspacosBranco.Execute(itemNameSubRoot);
            var hasSubRoot = !StringEhNuloVazioComEspacosBranco.Execute(subRootName);

            if (!hasPredicate && !hasItemName)
            {
                if (hasSubRoot)
                    return await LerAsync<T>(FS, subRootName, EncodingTexto, CancelToken).ConfigureAwait(false);

                return await LerAsync<T>(FS, EncodingTexto, CancelToken).ConfigureAwait(false);
            }

            if (!hasSubRoot)
                throw new ArgumentNullException(nameof(subRootName), "SubRootName é obrigatório quando Predicate ou ItemNameSubRoot são fornecidos.");

            return await LerAsync<T>(FS, subRootName, itemNameSubRoot, predicate, EncodingTexto, CancelToken).ConfigureAwait(false);
        }

        protected async Task<T> LerAsync<T>(FileStream filestream, Encoding encoding, CancellationToken cancellation = default)
        {
            // Exemplo 1: Serializar e desserializar uma loja completa (Root).
            var desserializacao = await XmlService.DeserializeRootAsync<T>(filestream, encoding, cancellation);

            // Retorne o resultado ou faça o processamento necessário
            return desserializacao;
        }

        protected async Task<T> LerAsync<T>(FileStream filestream, string subRootName, Encoding encoding, CancellationToken cancellation = default)
        {
            // Exemplo 2: Desserializar apenas uma sub-root (ListaProdutos).
            var desserializacao = await XmlService.DeserializeSubRootAsync<T>(filestream, subRootName, encoding, cancellation);

            // Retorne o resultado ou faça o processamento necessário
            return desserializacao;
        }

        protected async Task<T> LerAsync<T>(FileStream filestream, string subRootName, string itemNameSubRoot, Func<T, bool> predicate, Encoding encoding, CancellationToken cancellation = default)
        {
            // Exemplo 4: Buscar um cliente específico por email usando predicado.
            // var clienteProcurado = await _xmlService.DeserializeItemByPredicateFromFileAsync<Cliente>(stream, "Clientes", "Cliente", c => c.Id == 10);
            var desserializacao = await XmlService.DeserializeItemByPredicateAsync<T>(filestream, subRootName, itemNameSubRoot, predicate, encoding, cancellation);

            // Retorne o resultado ou faça o processamento necessário
            return desserializacao;
        }

        /// <summary>
        /// Escreve o <see cref="XDocument"/> fornecido para o arquivo de forma assíncrona.
        /// Se o arquivo já existir, ele será sobrescrito.
        /// </summary>
        /// <param name="dados">O <see cref="XDocument"/> a ser salvo.</param>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        public async Task EscreverAsync<T>(ITaskParametros parametros)
        {
            if (EstaAberto() == false)
            {
                await ConectarWriterAndReaderUnshareAsync().ConfigureAwait(false);
            }

            this.CancelToken = parametros.RetornoCancellationToken;
            this.EncodingTexto = parametros.RetornoEncoding;
            var objeto = parametros.Retorno<T>("Objeto");

            await XmlService.SerializeAsync<T>((T)objeto, FS).ConfigureAwait(false);
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
