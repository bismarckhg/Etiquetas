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
        private readonly XmlReaderSettings _settingsReader;
        private readonly XmlWriterSettings _settingsWriter;
        private readonly ConcurrentDictionary<Type, XmlSerializer> _cacheSerializer =
            new ConcurrentDictionary<Type, XmlSerializer>();

        public GenericXmlService(XmlReaderSettings readerSettings = null, XmlWriterSettings writerSettings = null)
        {
            _settingsReader = readerSettings ?? new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true,
                CloseInput = false,
                Async = true
            };

            _settingsWriter = writerSettings ?? new XmlWriterSettings
            {
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
            _cacheSerializer.GetOrAdd(type, t => new XmlSerializer(t));

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
        public async Task<T> DeserializeRootAsync<T>(Stream stream, CancellationToken cancellationToken = default)
            where T : class
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);

            using (var reader = XmlReader.Create(cancelableStream, _settingsReader))
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Lê de forma assíncrona até o elemento raiz
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        var serializer = GetSerializer(typeof(T));
                        return (T)serializer.Deserialize(reader);
                    }
                }
            }

            throw new InvalidOperationException("Nenhum elemento raiz encontrado no XML.");
        }

        /// <summary>
        /// Desserializa o XML completo de um arquivo para o tipo raiz especificado.
        /// </summary>
        public async Task<T> DeserializeRootFromFileAsync<T>(FileStream fileStream, CancellationToken cancellationToken = default)
            where T : class
        {
            //using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            using (fileStream)
            {
                return await DeserializeRootAsync<T>(fileStream, cancellationToken).ConfigureAwait(false);
            }
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
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(elementName))
                throw new ArgumentException("Nome do elemento não pode ser nulo ou vazio.", nameof(elementName));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);

            using (var reader = XmlReader.Create(cancelableStream, _settingsReader))
            {
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == elementName)
                    {
                        var serializer = GetSerializer(typeof(T));
                        return (T)serializer.Deserialize(reader);
                    }
                }
            }

            throw new InvalidOperationException($"Elemento '{elementName}' não encontrado no XML.");
        }

        /// <summary>
        /// Desserializa uma sub-root específica de um arquivo pelo nome do elemento.
        /// </summary>
        public async Task<T> DeserializeSubRootFromFileAsync<T>(
            Stream stream,
            string elementName,
            CancellationToken cancellationToken = default)
            where T : class
        {

            //using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            using (stream)
            {
                return await DeserializeSubRootAsync<T>(stream, elementName, cancellationToken).ConfigureAwait(false);
            }
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
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(arrayElementName))
                throw new ArgumentException("Nome do elemento array não pode ser nulo ou vazio.", nameof(arrayElementName));

            if (string.IsNullOrWhiteSpace(itemElementName))
                throw new ArgumentException("Nome do elemento item não pode ser nulo ou vazio.", nameof(itemElementName));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);
            var results = new List<T>();

            using (var reader = XmlReader.Create(cancelableStream, _settingsReader))
            {
                var serializer = GetSerializer(typeof(T));
                var insideArray = false;

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == arrayElementName)
                        {
                            insideArray = true;
                        }
                        else if (insideArray && reader.Name == itemElementName)
                        {
                            // Usar subtree reader para não afetar o reader principal
                            using (var subtreeReader = reader.ReadSubtree())
                            {
                                subtreeReader.Read(); // Move para o primeiro nó
                                var item = (T)serializer.Deserialize(subtreeReader);
                                results.Add(item);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == arrayElementName)
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
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(arrayElementName))
                throw new ArgumentException("Nome do elemento array não pode ser nulo ou vazio.", nameof(arrayElementName));

            if (string.IsNullOrWhiteSpace(itemElementName))
                throw new ArgumentException("Nome do elemento item não pode ser nulo ou vazio.", nameof(itemElementName));

            if (onItemRead == null)
                throw new ArgumentNullException(nameof(onItemRead));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);

            using (var reader = XmlReader.Create(cancelableStream, _settingsReader))
            {
                var serializer = GetSerializer(typeof(T));
                var insideArray = false;

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == arrayElementName)
                        {
                            insideArray = true;
                        }
                        else if (insideArray && reader.Name == itemElementName)
                        {
                            // Usar subtree reader para não afetar o reader principal
                            using (var subtreeReader = reader.ReadSubtree())
                            {
                                subtreeReader.Read(); // Move para o primeiro nó
                                var item = (T)serializer.Deserialize(subtreeReader);
                                onItemRead(item);
                            }
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == arrayElementName)
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
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (string.IsNullOrWhiteSpace(arrayElementName))
                throw new ArgumentException("Nome do elemento array não pode ser nulo ou vazio.", nameof(arrayElementName));

            if (string.IsNullOrWhiteSpace(itemElementName))
                throw new ArgumentException("Nome do elemento item não pode ser nulo ou vazio.", nameof(itemElementName));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            Rewind(stream);

            var cancelableStream = WrapWithCancellation(stream, cancellationToken);

            using (var reader = XmlReader.Create(cancelableStream, _settingsReader))
            {
                var serializer = GetSerializer(typeof(T));
                var insideArray = false;

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == arrayElementName)
                        {
                            insideArray = true;
                        }
                        else if (insideArray && reader.Name == itemElementName)
                        {
                            // Usar subtree reader para não afetar o reader principal
                            using (var subtreeReader = reader.ReadSubtree())
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
                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == arrayElementName)
                    {
                        insideArray = false;
                        break;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Desserializa um elemento específico de uma coleção baseado em um predicado de um arquivo.
        /// </summary>
        public async Task<T> DeserializeItemByPredicateFromFileAsync<T>(
            Stream stream,
            string arrayElementName,
            string itemElementName,
            Func<T, bool> predicate,
            CancellationToken cancellationToken = default)
            where T : class
        {

            using (stream)
            {
                return await DeserializeItemByPredicateAsync(
                    stream,
                    arrayElementName,
                    itemElementName,
                    predicate,
                    cancellationToken).ConfigureAwait(false);
            }
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
            where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (encoding == null)
            {
                encoding = ConversaoEncoding.UTF8BOM;
            }

            _settingsWriter.Encoding = encoding;

            cancellationToken.ThrowIfCancellationRequested();

            using (var writer = XmlWriter.Create(stream, _settingsWriter))
            {
                await Task.Run(() =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var serializer = GetSerializer(typeof(T));
                    serializer.Serialize(writer, obj);
                }, cancellationToken).ConfigureAwait(false);

                await writer.FlushAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Serializa um objeto para XML em um arquivo.
        /// </summary>
        public async Task SerializeToFileAsync<T>(T obj, string filePath, Encoding encoding = null, CancellationToken cancellationToken = default)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Caminho do arquivo não pode ser nulo ou vazio.", nameof(filePath));

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await SerializeAsync(obj, fileStream, encoding, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Serializa um objeto para string XML.
        /// </summary>
        public async Task<string> SerializeToStringAsync<T>(T obj, Encoding encoding = null, CancellationToken cancellationToken = default)
            where T : class
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