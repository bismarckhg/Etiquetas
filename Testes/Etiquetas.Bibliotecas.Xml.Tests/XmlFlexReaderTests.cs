using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Etiquetas.Bibliotecas.Json.Tests
{
    // ===== MODELOS DO EXEMPLO =====
    [Serializable]
    public class Fornecedor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Contato { get; set; }
    }

    [Serializable]
    public class Produto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
    }

    [Serializable]
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    // Contêiner <Produtos> com itens <Produto> e o campo <PrecoTotal>
    [Serializable, System.Xml.Serialization.XmlType("Produtos")]
    public class ListaProdutos
    {
        [System.Xml.Serialization.XmlElement("Produto")]
        public List<Produto> Produtos { get; set; } = new List<Produto>();

        public double PrecoTotal { get; set; }
    }

    [Serializable, System.Xml.Serialization.XmlRoot("Loja")]
    public class Loja
    {
        [System.Xml.Serialization.XmlArray("Fornecedores")]
        [System.Xml.Serialization.XmlArrayItem("Fornecedor")]
        public List<Fornecedor> Fornecedores { get; set; } = new List<Fornecedor>();

        [System.Xml.Serialization.XmlElement("Produtos")]
        public ListaProdutos Produtos { get; set; }

        [System.Xml.Serialization.XmlArray("Clientes")]
        [System.Xml.Serialization.XmlArrayItem("Cliente")]
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
    }

    // ===== SUA IMPLEMENTAÇÃO (SUT) =====
    // -> Assuma que XmlFlexReader e CancelableStream estão no projeto principal.
    // Aqui, apenas referenciamos a classe; se preferir, cole sua implementação num arquivo compartilhado.
    public class XmlFlexReader
    {
        // --- cole aqui a sua versão final do XmlFlexReader (com CancelableStream) ---
        // Para o teste compilar localmente, você pode incluir a implementação completa.
        // Abaixo vai um "stub" simplificado de assinatura para ilustrar. Troque pelo seu código real.

        private readonly System.Xml.XmlReaderSettings _settings = new System.Xml.XmlReaderSettings
        {
            IgnoreWhitespace = true,
            IgnoreComments = true,
            CloseInput = false
        };
        private readonly System.Collections.Concurrent.ConcurrentDictionary<Type, System.Xml.Serialization.XmlSerializer> _cache =
            new System.Collections.Concurrent.ConcurrentDictionary<Type, System.Xml.Serialization.XmlSerializer>();
        private System.Xml.Serialization.XmlSerializer Ser(Type t) => _cache.GetOrAdd(t, tt => new System.Xml.Serialization.XmlSerializer(tt));
        private static void Rewind(Stream s) { if (s != null && s.CanSeek) s.Seek(0, SeekOrigin.Begin); }

        public T DeserializeRoot<T>(Stream stream, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            Rewind(stream); ct.ThrowIfCancellationRequested();

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    using (var sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true))
                    using (var xr = System.Xml.XmlReader.Create(sr, _settings))
                    {
                        ct.ThrowIfCancellationRequested();
                        return (T)Ser(typeof(T)).Deserialize(xr);
                    }
                }
                else
                {
                    using (var xr = System.Xml.XmlReader.Create(cstream, _settings))
                    {
                        ct.ThrowIfCancellationRequested();
                        return (T)Ser(typeof(T)).Deserialize(xr);
                    }
                }
            }
        }

        public Task<T> DeserializeRootAsync<T>(Stream stream, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
            => Task.Run(() => DeserializeRoot<T>(stream, encoding, forceEncoding, leaveStreamOpen, ct), ct);

        public T DeserializeSubRoot<T>(Stream stream, string subRootName, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(subRootName)) throw new ArgumentNullException(nameof(subRootName));
            Rewind(stream); ct.ThrowIfCancellationRequested();

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                System.Xml.XmlReader xr;
                TextReader sr = null;

                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true);
                    xr = System.Xml.XmlReader.Create(sr, _settings);
                }
                else
                {
                    xr = System.Xml.XmlReader.Create(cstream, _settings);
                }

                using (sr) using (xr)
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

        public Task<T> DeserializeSubRootAsync<T>(Stream stream, string subRootName, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
            => Task.Run(() => DeserializeSubRoot<T>(stream, subRootName, encoding, forceEncoding, leaveStreamOpen, ct), ct);

        private static string ElementNameFor<T>()
        {
            var t = typeof(T);
            var xr = (System.Xml.Serialization.XmlRootAttribute)Attribute.GetCustomAttribute(t, typeof(System.Xml.Serialization.XmlRootAttribute));
            if (xr != null && !string.IsNullOrWhiteSpace(xr.ElementName)) return xr.ElementName;
            var xt = (System.Xml.Serialization.XmlTypeAttribute)Attribute.GetCustomAttribute(t, typeof(System.Xml.Serialization.XmlTypeAttribute));
            if (xt != null && !string.IsNullOrWhiteSpace(xt.TypeName)) return xt.TypeName;
            return t.Name;
        }

        public IEnumerable<T> ReadSubRootItems<T>(Stream stream, string subRootName, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(subRootName)) throw new ArgumentNullException(nameof(subRootName));
            Rewind(stream);

            var itemName = ElementNameFor<T>();
            var ser = Ser(typeof(T));

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                System.Xml.XmlReader xr;
                TextReader sr = null;

                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true);
                    xr = System.Xml.XmlReader.Create(sr, _settings);
                }
                else
                {
                    xr = System.Xml.XmlReader.Create(cstream, _settings);
                }

                using (sr) using (xr)
                {
                    if (!xr.ReadToFollowing(subRootName)) yield break;
                    if (xr.IsEmptyElement) yield break;

                    var depth = xr.Depth;
                    xr.Read();

                    while (!xr.EOF)
                    {
                        ct.ThrowIfCancellationRequested();

                        if (xr.NodeType == System.Xml.XmlNodeType.EndElement &&
                            xr.Depth == depth && xr.LocalName == subRootName)
                            yield break;

                        if (xr.NodeType == System.Xml.XmlNodeType.Element && xr.LocalName == itemName)
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

        public Task<List<T>> ReadSubRootItemsAllAsync<T>(Stream stream, string subRootName, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
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

        public bool TryReadSubRootElementValue<TValue>(Stream stream, string subRootName, string elementName, out TValue value, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(subRootName)) throw new ArgumentNullException(nameof(subRootName));
            if (string.IsNullOrWhiteSpace(elementName)) throw new ArgumentNullException(nameof(elementName));
            Rewind(stream); ct.ThrowIfCancellationRequested();
            value = default(TValue);

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                System.Xml.XmlReader xr;
                TextReader sr = null;

                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true);
                    xr = System.Xml.XmlReader.Create(sr, _settings);
                }
                else
                {
                    xr = System.Xml.XmlReader.Create(cstream, _settings);
                }

                using (sr) using (xr)
                {
                    if (!xr.ReadToFollowing(subRootName)) return false;
                    if (xr.IsEmptyElement) return false;

                    var depth = xr.Depth;
                    xr.Read();

                    while (!xr.EOF)
                    {
                        ct.ThrowIfCancellationRequested();

                        if (xr.NodeType == System.Xml.XmlNodeType.EndElement &&
                            xr.Depth == depth && xr.LocalName == subRootName)
                            return false;

                        if (xr.NodeType == System.Xml.XmlNodeType.Element && xr.LocalName == elementName)
                        {
                            var ser = Ser(typeof(TValue));
                            using (var subtree = xr.ReadSubtree())
                            {
                                subtree.MoveToContent();
                                var obj = ser.Deserialize(subtree);
                                if (obj is TValue ok)
                                {
                                    value = ok;
                                    return true;
                                }
                                return false;
                            }
                        }
                        else
                        {
                            xr.Read();
                        }
                    }
                    return false;
                }
            }
        }

        public IEnumerable<T> ReadAllItems<T>(Stream stream, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            Rewind(stream);

            var itemName = ElementNameFor<T>();
            var ser = Ser(typeof(T));

            using (var cstream = new CancelableStream(stream, ct, leaveOpen: leaveStreamOpen))
            {
                System.Xml.XmlReader xr;
                TextReader sr = null;

                if (encoding != null)
                {
                    bool detectBom = !forceEncoding;
                    sr = new StreamReader(cstream, encoding, detectBom, 4096, leaveOpen: true);
                    xr = System.Xml.XmlReader.Create(sr, _settings);
                }
                else
                {
                    xr = System.Xml.XmlReader.Create(cstream, _settings);
                }

                using (sr) using (xr)
                {
                    while (!xr.EOF)
                    {
                        ct.ThrowIfCancellationRequested();

                        if (xr.NodeType == System.Xml.XmlNodeType.Element && xr.LocalName == itemName)
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

        public Task<List<T>> ReadAllItemsAsync<T>(Stream stream, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
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

        public Task<T> FindOneByFieldAsync<T>(Stream stream, string fieldName, object expectedValue, string subRootName = null, Encoding encoding = null, bool forceEncoding = false, bool leaveStreamOpen = true, CancellationToken ct = default(CancellationToken))
        {
            return Task.Run(() =>
            {
                if (stream == null) throw new ArgumentNullException(nameof(stream));
                Rewind(stream); ct.ThrowIfCancellationRequested();

                var prop = typeof(T).GetProperty(fieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
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

    // Stream “cancelável” do SUT (use a mesma implementação do seu projeto).
    public sealed class CancelableStream : Stream
    {
        private readonly Stream _inner;
        private readonly CancellationToken _ct;
        private readonly bool _leaveOpen;

        public CancelableStream(Stream inner, CancellationToken ct, bool leaveOpen = true)
        { _inner = inner ?? throw new ArgumentNullException(nameof(inner)); _ct = ct; _leaveOpen = leaveOpen; }

        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => _inner.CanSeek;
        public override bool CanWrite => false;
        public override long Length => _inner.Length;
        public override long Position { get => _inner.Position; set { _ct.ThrowIfCancellationRequested(); _inner.Position = value; } }
        public override void Flush() => _inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) { _ct.ThrowIfCancellationRequested(); return _inner.Read(buffer, offset, count); }
        public override int ReadByte() { _ct.ThrowIfCancellationRequested(); return _inner.ReadByte(); }
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            _ct.ThrowIfCancellationRequested(); cancellationToken.ThrowIfCancellationRequested();
            using (var linked = CancellationTokenSource.CreateLinkedTokenSource(_ct, cancellationToken))
                return _inner.ReadAsync(buffer, offset, count, linked.Token);
        }
        public override long Seek(long offset, SeekOrigin origin) { _ct.ThrowIfCancellationRequested(); return _inner.Seek(offset, origin); }
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        protected override void Dispose(bool disposing) { if (disposing && !_leaveOpen) _inner.Dispose(); base.Dispose(disposing); }
    }

    // ===== Helpers de teste =====
    internal static class TestXml
    {
        public static string LojaXmlUtf8 =
    @"<?xml version=""1.0"" encoding=""utf-8""?>
<Loja>
  <Fornecedores>
    <Fornecedor><Id>1</Id><Nome>Acme</Nome><Contato>contato@acme.com</Contato></Fornecedor>
  </Fornecedores>
  <Produtos>
    <Produto><Codigo>P1</Codigo><Descricao>Café</Descricao><Preco>10.5</Preco></Produto>
    <Produto><Codigo>P2</Codigo><Descricao>Açúcar</Descricao><Preco>5.25</Preco></Produto>
    <PrecoTotal>15.75</PrecoTotal>
  </Produtos>
  <Clientes>
    <Cliente><Id>42</Id><Nome>Ana</Nome><Email>ana@exemplo.com</Email></Cliente>
    <Cliente><Id>43</Id><Nome>José</Nome><Email>jose@exemplo.com</Email></Cliente>
  </Clientes>
</Loja>";

        // Mesmo XML mas com PRÓLOGO enganoso: declara UTF-8, porém bytes serão Latin1 (útil para testar forceEncoding)
        public static string LojaXmlPrologoUtf8_MasVamosGravarLatin1 =
    @"<?xml version=""1.0"" encoding=""utf-8""?>
<Loja>
  <Produtos>
    <Produto><Codigo>P3</Codigo><Descricao>Maçã</Descricao><Preco>3.40</Preco></Produto>
    <PrecoTotal>3.40</PrecoTotal>
  </Produtos>
</Loja>";

        public static MemoryStream ToStream(string xml, Encoding enc) =>
            new MemoryStream(enc.GetBytes(xml));
    }

    // Stream que “segura” cada leitura por um pequeno delay (ajuda a exercitar cancelamento no meio da operação)
    internal sealed class ThrottledStream : Stream
    {
        private readonly Stream _inner;
        private readonly int _delayMs;
        public ThrottledStream(Stream inner, int delayMs) { _inner = inner; _delayMs = delayMs; }
        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => _inner.CanSeek;
        public override bool CanWrite => false;
        public override long Length => _inner.Length;
        public override long Position { get => _inner.Position; set => _inner.Position = value; }
        public override void Flush() => _inner.Flush();
        public override int Read(byte[] buffer, int offset, int count)
        {
            Thread.Sleep(_delayMs);
            return _inner.Read(buffer, offset, count);
        }
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(_delayMs);
                return _inner.Read(buffer, offset, count);
            }, cancellationToken);
        }
        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    // ===== TESTES =====
    public class XmlFlexReaderTests
    {
        private readonly XmlFlexReader _sut = new XmlFlexReader();

        [Fact]
        public async Task DeserializeRoot_DeveLerLojaInteira()
        {
            using (var ms = TestXml.ToStream(TestXml.LojaXmlUtf8, new UTF8Encoding(false)))
            {
                var loja = await _sut.DeserializeRootAsync<Loja>(ms, encoding: null, forceEncoding: false, leaveStreamOpen: true, ct: CancellationToken.None);
                Assert.NotNull(loja);
                Assert.NotNull(loja.Fornecedores);
                Assert.NotNull(loja.Produtos);
                Assert.NotNull(loja.Clientes);

                Assert.Equal(2, loja.Produtos.Produtos.Count);
                Assert.Equal(15.75, loja.Produtos.PrecoTotal, 3);
                Assert.Equal("Café", loja.Produtos.Produtos[0].Descricao);
                Assert.Equal("Açúcar", loja.Produtos.Produtos[1].Descricao);
            }
        }

        [Fact]
        public void DeserializeSubRoot_Container_DeveLerListaProdutosEPrecoTotal()
        {
            using (var ms = TestXml.ToStream(TestXml.LojaXmlUtf8, Encoding.UTF8))
            {
                var lp = _sut.DeserializeSubRoot<ListaProdutos>(ms, "Produtos", encoding: null, forceEncoding: false, leaveStreamOpen: true);
                Assert.NotNull(lp);
                Assert.Equal(2, lp.Produtos.Count);
                Assert.Equal(15.75, lp.PrecoTotal, 3);
            }
        }

        [Fact]
        public void ReadSubRootItems_Stream_DeveIterarItensProduto()
        {
            using (var ms = TestXml.ToStream(TestXml.LojaXmlUtf8, Encoding.UTF8))
            {
                var itens = _sut.ReadSubRootItems<Produto>(ms, "Produtos").ToList();
                Assert.Equal(2, itens.Count);
                Assert.Contains(itens, p => p.Codigo == "P1" && p.Preco == 10.5);
                Assert.Contains(itens, p => p.Codigo == "P2" && Math.Abs(p.Preco - 5.25) < 1e-6);
            }
        }

        [Fact]
        public void TryReadSubRootElementValue_DevePegarPrecoTotal()
        {
            using (var ms = TestXml.ToStream(TestXml.LojaXmlUtf8, Encoding.UTF8))
            {
                double total;
                var ok = _sut.TryReadSubRootElementValue(ms, "Produtos", "PrecoTotal", out total);
                Assert.True(ok);
                Assert.Equal(15.75, total, 3);
            }
        }

        [Fact]
        public async Task ReadAllItemsAsync_DeveListarTodosClientes()
        {
            using (var ms = TestXml.ToStream(TestXml.LojaXmlUtf8, Encoding.UTF8))
            {
                var clientes = await _sut.ReadAllItemsAsync<Cliente>(ms);
                Assert.Equal(2, clientes.Count);
                Assert.Contains(clientes, c => c.Email == "ana@exemplo.com");
                Assert.Contains(clientes, c => c.Nome == "José");
            }
        }

        [Fact]
        public async Task FindOneByFieldAsync_DeveEncontrarClientePorEmail()
        {
            using (var ms = TestXml.ToStream(TestXml.LojaXmlUtf8, Encoding.UTF8))
            {
                var cli = await _sut.FindOneByFieldAsync<Cliente>(ms, "Email", "ana@exemplo.com", subRootName: "Clientes");
                Assert.NotNull(cli);
                Assert.Equal(42, cli.Id);
                Assert.Equal("Ana", cli.Nome);
            }
        }

        [Fact]
        public void DeserializeRoot_LeaveStreamOpen_DeveManterStreamUsavel()
        {
            using (var ms = TestXml.ToStream(TestXml.LojaXmlUtf8, Encoding.UTF8))
            {
                var loja = _sut.DeserializeRoot<Loja>(ms, encoding: null, leaveStreamOpen: true);
                Assert.NotNull(loja);

                // O stream deve continuar aberto; reposiciona e lê 5 primeiros bytes
                Assert.True(ms.CanRead);
                ms.Seek(0, SeekOrigin.Begin);
                var buf = new byte[5];
                var n = ms.Read(buf, 0, buf.Length);
                Assert.Equal(5, n);
            }
        }

        [Fact]
        public void DeserializeRoot_ForcedEncoding_DeveIgnorarPrologoIncorreto()
        {
            // Grava bytes em Latin1 mas com prólogo "utf-8". Sem forçar encoding, tende a falhar.
            var latin1 = Encoding.GetEncoding("ISO-8859-1");
            using (var ms = TestXml.ToStream(TestXml.LojaXmlPrologoUtf8_MasVamosGravarLatin1, latin1))
            {
                // Forçando ISO-8859-1 deve desserializar corretamente
                var loja = _sut.DeserializeRoot<Loja>(ms, encoding: latin1, forceEncoding: true, leaveStreamOpen: true);
                Assert.NotNull(loja);
                Assert.NotNull(loja.Produtos);
                Assert.Single(loja.Produtos.Produtos);
                Assert.Equal("Maçã", loja.Produtos.Produtos[0].Descricao); // caractere Latin1
            }
        }

        [Fact]
        public async Task DeserializeRoot_CancelamentoDuranteLeitura_DeveLancarOperationCanceled()
        {
            // XML grande para dar tempo de cancelar
            var sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?><Loja><Clientes>");
            for (int i = 0; i < 5000; i++)
            {
                sb.Append(@"<Cliente><Id>").Append(i).Append("</Id><Nome>Nome ").Append(i)
                  .Append("</Nome><Email>e").Append(i).Append("@x.com</Email></Cliente>");
            }
            sb.AppendLine("</Clientes><Fornecedores/><Produtos><PrecoTotal>0</PrecoTotal></Produtos></Loja>");
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var slow = new ThrottledStream(new MemoryStream(bytes), delayMs: 1);

            var cts = new CancellationTokenSource();
            var task = Task.Run(() => _sut.DeserializeRoot<Loja>(slow, encoding: null, forceEncoding: false, leaveStreamOpen: true, ct: cts.Token));

            // cancela pouco depois de iniciar
            await Task.Delay(10);
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
        }

        [Fact]
        public async Task ReadSubRootItems_CancelamentoDuranteIteracao_DeveLancarOperationCanceled()
        {
            // reaproveita XML grande
            var sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?><Loja><Produtos>");
            for (int i = 0; i < 8000; i++)
            {
                sb.Append(@"<Produto><Codigo>P").Append(i).Append("</Codigo><Descricao>Item ").Append(i)
                  .Append("</Descricao><Preco>1.1</Preco></Produto>");
            }
            sb.AppendLine("<PrecoTotal>8800</PrecoTotal></Produtos></Loja>");

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var slow = new ThrottledStream(new MemoryStream(bytes), delayMs: 1);

            var cts = new CancellationTokenSource();

            // Enumerar em Task separada para poder cancelar durante a iteração
            var enumerateTask = Task.Run(() =>
            {
                var count = 0;
                foreach (var p in _sut.ReadSubRootItems<Produto>(slow, "Produtos", encoding: null, forceEncoding: false, leaveStreamOpen: true, ct: cts.Token))
                {
                    count++;
                    if (count == 100) // cancela após alguns itens
                        cts.Cancel();
                }
            });

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await enumerateTask);
        }
    }


}