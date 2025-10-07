using Etiquetas.Bibliotecas.Streams.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

namespace Etiquetas.Bibliotecas.Xml
{
    public class XmlFlexReader
    {
        private readonly XmlReaderSettings SettingsReader;
        private readonly ConcurrentDictionary<Type, XmlSerializer> CacheReader =
            new ConcurrentDictionary<Type, XmlSerializer>();

        public XmlFlexReader(XmlReaderSettings settings = null)
        {
            SettingsReader = settings ?? new XmlReaderSettings
            {
                IgnoreWhitespace = true,
                IgnoreComments = true,
                CloseInput = false
            };
        }

        private XmlSerializer Ser(Type t) => CacheReader.GetOrAdd(t, tt => new XmlSerializer(tt));

        private static void Rewind(Stream s)
        {
            if (s != null && s.CanSeek) s.Seek(0, SeekOrigin.Begin);
        }

        private static string ElementNameFor<T>()
        {
            var t = typeof(T);
            var xr = t.GetCustomAttribute<XmlRootAttribute>();
            if (xr != null && !string.IsNullOrWhiteSpace(xr.ElementName)) return xr.ElementName;
            var xt = t.GetCustomAttribute<XmlTypeAttribute>();
            if (xt != null && !string.IsNullOrWhiteSpace(xt.TypeName)) return xt.TypeName;
            return t.Name;
        }

        // ==== 1) Root inteiro <T> ====
        public T DeserializeRoot<T>(
                Stream stream,
                CancellationToken ct = default,
                Encoding encoding = null,      // null = autodetect (BOM/prólogo)
                bool forceEncoding = false,    // true = ignora BOM e impõe 'encoding'
                bool leaveStreamOpen = true
                )
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
            ct.ThrowIfCancellationRequested();

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                if (encoding != null)
                {
                    // forceEncoding: false => respeita BOM se houver; true => impõe 'encoding'
                    bool detectBom = !forceEncoding;

                    using (var sr = new StreamReader(
                        cstream,
                        encoding,
                        detectEncodingFromByteOrderMarks: detectBom,
                        bufferSize: 4096,
                        leaveOpen: true)) // mantém o stream do chamador aberto
                    using (var xr = XmlReader.Create(sr, SettingsReader))
                    {
                        ct.ThrowIfCancellationRequested();
                        return (T)Ser(typeof(T)).Deserialize(xr);
                    }
                }
                else
                {
                    // Autodetect (BOM/prólogo do XML)
                    using (var xr = XmlReader.Create(cstream, SettingsReader))
                    {
                        ct.ThrowIfCancellationRequested();
                        return (T)Ser(typeof(T)).Deserialize(xr);
                    }
                }
            }
        }

        public Task<T> DeserializeRootAsync<T>(
            Stream stream,
            CancellationToken ct = default,
            Encoding encoding = null,
            bool forceEncoding = false,
            bool leaveStreamOpen = true
            )
        {
            // Importante: passar os MESMOS parâmetros; agora o token cancela no meio
            return Task.Run(() => DeserializeRoot<T>(stream, ct, encoding, forceEncoding, leaveStreamOpen), ct);
        }

        // ==== 2a) Sub-root -> objeto contêiner <T> (ex.: T=ListaProdutos, subRoot="Produtos") ====
        public T DeserializeSubRoot<T>(
             Stream stream, string subRootName,
             Encoding encoding = null, bool forceEncoding = false,
             bool leaveStreamOpen = true, CancellationToken ct = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(subRootName)) throw new ArgumentNullException(nameof(subRootName));
            Rewind(stream); ct.ThrowIfCancellationRequested();

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    using (var sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true))
                    using (var xr = XmlReader.Create(sr, SettingsReader))
                    {
                        if (!xr.ReadToFollowing(subRootName)) return default(T);
                        using (var subtree = xr.ReadSubtree())
                        {
                            subtree.MoveToContent();
                            return (T)Ser(typeof(T)).Deserialize(subtree);
                        }
                    }
                }
                else
                {
                    using (var xr = XmlReader.Create(cstream, SettingsReader))
                    {
                        if (!xr.ReadToFollowing(subRootName)) return default(T);
                        using (var subtree = xr.ReadSubtree())
                        {
                            subtree.MoveToContent();
                            return (T)Ser(typeof(T)).Deserialize(subtree);
                        }
                    }
                }
            }
        }

        public Task<T> DeserializeSubRootAsync<T>(
            Stream stream, string subRootName,
            Encoding encoding = null, bool forceEncoding = false,
            bool leaveStreamOpen = true, CancellationToken ct = default)
            => Task.Run(() => DeserializeSubRoot<T>(stream, subRootName, encoding, forceEncoding, leaveStreamOpen, ct), ct);

        // ==== 2b) SUB-ROOT -> ITENS <T> (streaming) (ex.: T=Produto, subRoot="Produtos") ====
        public IEnumerable<T> ReadSubRootItems<T>(
            Stream stream, string subRootName,
            Encoding encoding = null, bool forceEncoding = false,
            bool leaveStreamOpen = true, CancellationToken ct = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(subRootName)) throw new ArgumentNullException(nameof(subRootName));
            Rewind(stream);

            var itemName = ElementNameFor<T>();
            var ser = Ser(typeof(T));

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                XmlReader xr;
                TextReader sr = null;

                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true);
                    xr = XmlReader.Create(sr, SettingsReader);
                }
                else
                {
                    xr = XmlReader.Create(cstream, SettingsReader);
                }

                using (sr) // ok se sr for null
                using (xr)
                {
                    if (!xr.ReadToFollowing(subRootName))
                        yield break;

                    if (xr.IsEmptyElement)
                        yield break;

                    var depth = xr.Depth;
                    xr.Read(); // entra na sub-root

                    while (!xr.EOF)
                    {
                        ct.ThrowIfCancellationRequested();

                        if (xr.NodeType == XmlNodeType.EndElement &&
                            xr.Depth == depth && xr.LocalName == subRootName)
                            yield break;

                        if (xr.NodeType == XmlNodeType.Element && xr.LocalName == itemName)
                        {
                            using (var subtree = xr.ReadSubtree())
                            {
                                subtree.MoveToContent();
                                var item = (T)ser.Deserialize(subtree);
                                yield return item;
                            }
                            xr.Read(); // avança após o item
                        }
                        else
                        {
                            xr.Read(); // pula <PrecoTotal> e demais nós
                        }
                    }
                }
            }
        }

        public Task<List<T>> ReadSubRootItemsAllAsync<T>(
            Stream stream, string subRootName,
            Encoding encoding = null, bool forceEncoding = false,
            bool leaveStreamOpen = true, CancellationToken ct = default)
            => Task.Run(() =>
            {
                var list = new List<T>();
                foreach (var x in ReadSubRootItems<T>(stream, subRootName, encoding, forceEncoding, leaveStreamOpen, ct))
                {
                    ct.ThrowIfCancellationRequested();
                    list.Add(x);
                }
                return list;
            }, ct);

        // ==== 2c) Campo escalar DENTRO da sub-root (ex.: <Produtos><PrecoTotal>...) ====
        public bool TryReadSubRootElementValue<TValue>(
    Stream stream, string subRootName, string elementName,
    out TValue value,
    Encoding encoding = null, bool forceEncoding = false,
    bool leaveStreamOpen = true, CancellationToken ct = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(subRootName)) throw new ArgumentNullException(nameof(subRootName));
            if (string.IsNullOrWhiteSpace(elementName)) throw new ArgumentNullException(nameof(elementName));
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
            ct.ThrowIfCancellationRequested();

            value = default(TValue);

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                XmlReader xr;
                TextReader sr = null;

                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true);
                    xr = XmlReader.Create(sr, SettingsReader);
                }
                else
                {
                    xr = XmlReader.Create(cstream, SettingsReader);
                }

                using (sr) using (xr)
                {
                    if (!xr.ReadToFollowing(subRootName))
                        return false;

                    if (xr.IsEmptyElement)
                        return false;

                    var depth = xr.Depth;
                    xr.Read(); // entra na sub-root

                    while (!xr.EOF)
                    {
                        ct.ThrowIfCancellationRequested();

                        if (xr.NodeType == XmlNodeType.EndElement &&
                            xr.Depth == depth && xr.LocalName == subRootName)
                            return false;

                        if (xr.NodeType == XmlNodeType.Element && xr.LocalName == elementName)
                        {
                            // ---- PRIMITIVOS / NULLABLES: usar métodos tipados do XmlReader ----
                            var t = typeof(TValue);
                            var u = Nullable.GetUnderlyingType(t) ?? t; // tipo "de fato"

                            object obj;

                            if (u == typeof(string)) obj = xr.ReadElementContentAsString();
                            else if (u == typeof(int)) obj = xr.ReadElementContentAsInt();
                            else if (u == typeof(long)) obj = xr.ReadElementContentAsLong();
                            else if (u == typeof(bool)) obj = xr.ReadElementContentAsBoolean();
                            else if (u == typeof(double)) obj = xr.ReadElementContentAsDouble();
                            else if (u == typeof(float)) obj = xr.ReadElementContentAsFloat();
                            else if (u == typeof(decimal)) // XmlReader tem AsDecimal em .NET 4.7.2; se preferir, parseie string com InvariantCulture
                                obj = xr.ReadElementContentAsDecimal();
                            else if (u == typeof(DateTime)) obj = xr.ReadElementContentAsDateTime();
                            else
                            {
                                // ---- COMPLEXOS: XmlSerializer com XmlRootAttribute(elementName) ----
                                var root = string.IsNullOrEmpty(xr.NamespaceURI)
                                    ? new XmlRootAttribute(elementName)
                                    : new XmlRootAttribute(elementName) { Namespace = xr.NamespaceURI };

                                var serLocal = new XmlSerializer(typeof(TValue), root); // não usar cache aqui
                                using (var subtree = xr.ReadSubtree())
                                {
                                    subtree.MoveToContent();
                                    obj = serLocal.Deserialize(subtree);
                                }
                            }

                            // Converter para TValue (lida com Nullable<>)
                            if (Nullable.GetUnderlyingType(t) != null)
                            {
                                var cast = Convert.ChangeType(obj, u, CultureInfo.InvariantCulture);
                                value = (TValue)(cast == null ? null : Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(u), cast));
                            }
                            else
                            {
                                // obj já sai no tipo certo nos métodos *AsXxx*
                                value = (TValue)Convert.ChangeType(obj, u, CultureInfo.InvariantCulture);
                            }
                            return true;
                        }

                        xr.Read();
                    }
                    return false;
                }
            }
        }

        // ==== 3) TODOS os elementos <T> no XML (sem sub-root) ====
        public IEnumerable<T> ReadAllItems<T>(
            Stream stream,
            Encoding encoding = null, bool forceEncoding = false,
            bool leaveStreamOpen = true, CancellationToken ct = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            Rewind(stream);

            var itemName = ElementNameFor<T>();
            var ser = Ser(typeof(T));

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                XmlReader xr;
                TextReader sr = null;

                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true);
                    xr = XmlReader.Create(sr, SettingsReader);
                }
                else
                {
                    xr = XmlReader.Create(cstream, SettingsReader);
                }

                using (sr)
                using (xr)
                {
                    while (!xr.EOF)
                    {
                        ct.ThrowIfCancellationRequested();

                        if (xr.NodeType == XmlNodeType.Element && xr.LocalName == itemName)
                        {
                            using (var subtree = xr.ReadSubtree())
                            {
                                subtree.MoveToContent();
                                var item = (T)ser.Deserialize(subtree);
                                yield return item;
                            }
                            xr.Read();
                        }
                        else
                        {
                            xr.Read();
                        }
                    }
                }
            }
        }

        public Task<List<T>> ReadAllItemsAsync<T>(
            Stream stream,
            Encoding encoding = null, bool forceEncoding = false,
            bool leaveStreamOpen = true, CancellationToken ct = default)
            => Task.Run(() =>
            {
                var list = new List<T>();
                foreach (var x in ReadAllItems<T>(stream, encoding, forceEncoding, leaveStreamOpen, ct))
                {
                    ct.ThrowIfCancellationRequested();
                    list.Add(x);
                }
                return list;
            }, ct);

        // ==== 4) Buscar um item <T> por campo/valor ====
        public Task<T> FindOneByFieldAsync<T>(
            Stream stream, string fieldName, object expectedValue,
            string subRootName = null,
            Encoding encoding = null, bool forceEncoding = false,
            bool leaveStreamOpen = true, CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                if (stream == null) throw new ArgumentNullException(nameof(stream));
                Rewind(stream); ct.ThrowIfCancellationRequested();

                var prop = typeof(T).GetProperty(fieldName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null)
                    throw new ArgumentException($"Tipo {typeof(T).Name} não possui a propriedade '{fieldName}'.");

                IEnumerable<T> seq = (subRootName == null)
                    ? ReadAllItems<T>(stream, encoding, forceEncoding, leaveStreamOpen, ct)
                    : ReadSubRootItems<T>(stream, subRootName, encoding, forceEncoding, leaveStreamOpen, ct);

                foreach (var item in seq)
                {
                    ct.ThrowIfCancellationRequested();
                    var val = prop.GetValue(item, null);
                    if (object.Equals(val, expectedValue))
                        return item;
                }
                return default(T);
            }, ct);
        }
    }
}
