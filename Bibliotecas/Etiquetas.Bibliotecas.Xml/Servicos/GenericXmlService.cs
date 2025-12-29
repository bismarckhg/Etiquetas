using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Etiquetas.Bibliotecas.Comum.Geral;
using Etiquetas.Bibliotecas.Streams.Core;

namespace Etiquetas.Bibliotecas.Xml.Servicos
{
    /// <summary>
    /// Serviço genérico para serialização e desserialização XML com suporte assíncrono e cache de serializers.
    /// </summary>
    public class GenericXmlService
    {
        private readonly XmlReaderSettings SettingsReader;
        private readonly XmlWriterSettings SettingsWriter;
        private readonly ConcurrentDictionary<Type, XmlSerializer> CacheSerializer =
            new ConcurrentDictionary<Type, XmlSerializer>();

        public GenericXmlService(XmlReaderSettings readerSettings = null, XmlWriterSettings writerSettings = null)
        {
            SettingsReader = readerSettings ?? new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true,
                CloseInput = false,
                Async = true
            };

            SettingsWriter = writerSettings ?? new XmlWriterSettings
            {
                Encoding = ConversaoEncoding.UTF8BOM,
                OmitXmlDeclaration = false,
                Indent = true,
                IndentChars = "  ",
                CloseOutput = false,
                Async = true
            };
        }

        /// <summary>
        /// Obtém ou cria um XmlSerializer para o tipo especificado, utilizando cache.
        /// </summary>
        private XmlSerializer GetSerializer(Type type) =>
            CacheSerializer.GetOrAdd(type, t => new XmlSerializer(t));

        /// <summary>
        /// Rebobina o stream para o início, se possível.
        /// </summary>
        private static void Rewind(Stream stream)
        {
            if (stream != null && stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <summary>
        /// Cria um CancelableStream se o token não for padrão, caso contrário retorna o stream original.
        /// </summary>
        private static Stream WrapWithCancellation(Stream stream, CancellationToken cancellationToken)
        {
            if (cancellationToken == default || cancellationToken == CancellationToken.None)
            {
                return stream;
            }

            return new CancelableStream(stream, cancellationToken, leaveOpen: true);
        }

        #region Desserialização Completa (Root)

        /// <summary>
        /// Desserializa o XML completo para o tipo raiz especificado.
        /// </summary>
        /// <typeparam name="T">Tipo raiz a ser desserializado</typeparam>
        /// <param name="stream">Stream contendo o XML</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Objeto desserializado do tipo T</returns>
        public async Task<T> DeserializeRootAsync<T>(Stream stream, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
            {
                encoding = ConversaoEncoding.UTF8BOM;
            }

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);

            using (var streamReader = new StreamReader(cancelableStream, encoding, true, 1024, true))
            using (var xmlReader = XmlReader.Create(streamReader, SettingsReader))
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Lê de forma assíncrona até o elemento raiz
                while (await xmlReader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        var serializer = GetSerializer(typeof(T));
                        return (T)serializer.Deserialize(xmlReader);
                    }
                }
            }
            throw new InvalidOperationException("Nenhum elemento raiz encontrado no XML.");
        }

        #endregion

        #region Desserialização de Sub-Root

        /// <summary>
        /// Desserializa uma sub-root específica do XML pelo nome do elemento.
        /// </summary>
        /// <typeparam name="T">Tipo da sub-root a ser desserializada</typeparam>
        /// <param name="stream">Stream contendo o XML</param>
        /// <param name="elementName">Nome do elemento da sub-root</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Objeto desserializado do tipo T</returns>
        public async Task<T> DeserializeSubRootAsync<T>(
            Stream stream,
            string elementName,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
            {
                encoding = ConversaoEncoding.UTF8BOM;
            }

            if (string.IsNullOrWhiteSpace(elementName))
                throw new ArgumentException("Nome do elemento não pode ser nulo ou vazio.", nameof(elementName));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);

            //using (var reader = XmlReader.Create(cancelableStream, SettingsReader))

            using (var streamReader = new StreamReader(cancelableStream, encoding, true, 1024, true))
            using (var xmlReader = XmlReader.Create(streamReader, SettingsReader))
            {
                while (await xmlReader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == elementName)
                    {
                        var serializer = GetSerializer(typeof(T));
                        return (T)serializer.Deserialize(xmlReader);
                    }
                }
            }
            throw new InvalidOperationException($"Elemento '{elementName}' não encontrado no XML.");
        }

        #endregion

        #region Desserialização de Coleções com Streaming

        /// <summary>
        /// Desserializa uma coleção de elementos de forma incremental (streaming).
        /// Permite processar elementos conforme são lidos, com suporte a cancelamento.
        /// </summary>
        /// <typeparam name="T">Tipo dos elementos da coleção</typeparam>
        /// <param name="stream">Stream contendo o XML</param>
        /// <param name="arrayElementName">Nome do elemento array (ex: "Clientes")</param>
        /// <param name="itemElementName">Nome do elemento item (ex: "Cliente")</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>IEnumerable assíncrono de elementos do tipo T</returns>
        public async Task<IEnumerable<T>> DeserializeCollectionAsync<T>(
            Stream stream,
            string arrayElementName,
            string itemElementName,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
            {
                encoding = ConversaoEncoding.UTF8BOM;
            }

            if (string.IsNullOrWhiteSpace(arrayElementName))
                throw new ArgumentException("Nome do elemento array não pode ser nulo ou vazio.", nameof(arrayElementName));

            if (string.IsNullOrWhiteSpace(itemElementName))
                throw new ArgumentException("Nome do elemento item não pode ser nulo ou vazio.", nameof(itemElementName));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);
            var results = new List<T>();

            using (var streamReader = new StreamReader(cancelableStream, encoding, true, 1024, true))
            using (var xmlReader = XmlReader.Create(streamReader, SettingsReader))
            {
                var serializer = GetSerializer(typeof(T));
                var insideArray = false;

                while (await xmlReader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name == arrayElementName)
                        {
                            insideArray = true;
                        }
                        else if (insideArray && xmlReader.Name == itemElementName)
                        {
                            // Usar subtree reader para não afetar o reader principal
                            using (var subtreeReader = xmlReader.ReadSubtree())
                            {
                                subtreeReader.Read(); // Move para o primeiro nó
                                var item = (T)serializer.Deserialize(subtreeReader);
                                results.Add(item);
                            }
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == arrayElementName)
                    {
                        insideArray = false;
                        break;
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Desserializa uma coleção de elementos de forma incremental com callback para processar cada item.
        /// Permite processar elementos conforme são lidos (streaming parcial), com suporte a cancelamento.
        /// </summary>
        /// <typeparam name="T">Tipo dos elementos da coleção</typeparam>
        /// <param name="stream">Stream contendo o XML</param>
        /// <param name="arrayElementName">Nome do elemento array (ex: "Clientes")</param>
        /// <param name="itemElementName">Nome do elemento item (ex: "Cliente")</param>
        /// <param name="onItemRead">Callback executado para cada item lido</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Task representando a operação assíncrona</returns>
        public async Task DeserializeCollectionWithCallbackAsync<T>(
            Stream stream,
            string arrayElementName,
            string itemElementName,
            Action<T> onItemRead,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
            {
                encoding = ConversaoEncoding.UTF8BOM;
            }

            if (string.IsNullOrWhiteSpace(arrayElementName))
                throw new ArgumentException("Nome do elemento array não pode ser nulo ou vazio.", nameof(arrayElementName));

            if (string.IsNullOrWhiteSpace(itemElementName))
                throw new ArgumentException("Nome do elemento item não pode ser nulo ou vazio.", nameof(itemElementName));

            if (onItemRead == null)
                throw new ArgumentNullException(nameof(onItemRead));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);

            using (var streamReader = new StreamReader(cancelableStream, encoding, true, 1024, true))
            using (var xmlReader = XmlReader.Create(streamReader, SettingsReader))
            {

                var serializer = GetSerializer(typeof(T));
                var insideArray = false;

                while (await xmlReader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name == arrayElementName)
                        {
                            insideArray = true;
                        }
                        else if (insideArray && xmlReader.Name == itemElementName)
                        {
                            // Usar subtree reader para não afetar o reader principal
                            using (var subtreeReader = xmlReader.ReadSubtree())
                            {
                                subtreeReader.Read(); // Move para o primeiro nó
                                var item = (T)serializer.Deserialize(subtreeReader);
                                onItemRead(item);
                            }
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == arrayElementName)
                    {
                        insideArray = false;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Desserialização com Filtro

        /// <summary>
        /// Desserializa um elemento específico de uma coleção baseado em um predicado.
        /// </summary>
        /// <typeparam name="T">Tipo do elemento a ser desserializado</typeparam>
        /// <param name="stream">Stream contendo o XML</param>
        /// <param name="arrayElementName">Nome do elemento array</param>
        /// <param name="itemElementName">Nome do elemento item</param>
        /// <param name="predicate">Função de filtro para encontrar o elemento desejado</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Primeiro elemento que satisfaz o predicado, ou null se não encontrado</returns>
        public async Task<T> DeserializeItemByPredicateAsync<T>(
            Stream stream,
            string arrayElementName,
            string itemElementName,
            Func<T, bool> predicate,
            Encoding encoding = null,
            CancellationToken cancellationToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
            {
                encoding = ConversaoEncoding.UTF8BOM;
            }

            if (string.IsNullOrWhiteSpace(arrayElementName))
                throw new ArgumentException("Nome do elemento array não pode ser nulo ou vazio.", nameof(arrayElementName));

            if (string.IsNullOrWhiteSpace(itemElementName))
                throw new ArgumentException("Nome do elemento item não pode ser nulo ou vazio.", nameof(itemElementName));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);

            using (var streamReader = new StreamReader(cancelableStream, encoding, true, 1024, true))
            using (var xmlReader = XmlReader.Create(streamReader, SettingsReader))
            {

                var serializer = GetSerializer(typeof(T));
                var insideArray = false;

                while (await xmlReader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name == arrayElementName)
                        {
                            insideArray = true;
                        }
                        else if (insideArray && xmlReader.Name == itemElementName)
                        {
                            // Usar subtree reader para não afetar o reader principal
                            using (var subtreeReader = xmlReader.ReadSubtree())
                            {
                                subtreeReader.Read(); // Move para o primeiro nó
                                var item = (T)serializer.Deserialize(subtreeReader);
                                if (predicate(item))
                                {
                                    return item;
                                }
                            }
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == arrayElementName)
                    {
                        insideArray = false;
                        break;
                    }
                }
            }
            return default;
        }

        #endregion

        #region Serialização

        /// <summary>
        /// Serializa um objeto para XML em um stream.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser serializado</typeparam>
        /// <param name="obj">Objeto a ser serializado</param>
        /// <param name="stream">Stream de destino</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        public async Task SerializeAsync<T>(T obj, Stream stream, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (encoding == null)
            {
                encoding = ConversaoEncoding.UTF8BOM;
            }

            SettingsWriter.Encoding = encoding;

            cancellationToken.ThrowIfCancellationRequested();

            using (var streamWriter = new StreamWriter(stream, encoding, 1024, true))
            using (var xmlWriter = XmlWriter.Create(streamWriter, SettingsWriter))
            {
                await Task.Run(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var serializer = GetSerializer(typeof(T));
                    serializer.Serialize(xmlWriter, obj);
                }, cancellationToken).ConfigureAwait(false);

                await xmlWriter.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Serializa um objeto para string XML.
        /// </summary>
        public async Task<string> SerializeToStringAsync<T>(T obj, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            using (var memoryStream = new MemoryStream())
            {
                await SerializeAsync(obj, memoryStream, encoding, cancellationToken).ConfigureAwait(false);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(memoryStream))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        #endregion
    }
}
