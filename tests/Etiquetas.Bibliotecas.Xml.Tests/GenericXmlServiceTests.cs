using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Etiquetas.Bibliotecas.Xml.Servicos;
using Etiquetas.Bibliotecas.Xml.Exemplo.Modelos;
using Etiquetas.Bibliotecas.Xml.Exemplo;

namespace XmlSerializationSolution.Tests
{
    /// <summary>
    /// Testes abrangentes para o GenericXmlService.
    /// </summary>
    public class GenericXmlServiceTests : IDisposable
    {
        private readonly GenericXmlService _service;
        private readonly XmlDataGenerator _generator;
        private readonly string _testFilePath;

        public GenericXmlServiceTests()
        {
            _service = new GenericXmlService();
            _generator = new XmlDataGenerator();
            //_testFilePath = Path.Combine(Path.GetTempPath(), $"test_loja_{Guid.NewGuid()}.xml");
            _testFilePath = Path.Combine("C:\\Temp\\", $"test_loja1_{Guid.NewGuid()}.xml");
        }

        public void Dispose()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        #region Testes de Serialização

        [Fact]
        public async Task SerializeAsync_DeveSerializarLojaCompleta()
        {
            // Arrange
            var loja = _generator.GerarLojaPequena();
            var _testFilePath = Path.Combine("C:\\Temp\\", $"Serializacao_{Guid.NewGuid()}.xml");

            // Act
            //using (var stream = new MemoryStream())
            using (var stream = new FileStream(_testFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                await _service.SerializeAsync(loja, stream);
                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    var xml = await reader.ReadToEndAsync();

                    // Assert
                    Assert.NotEmpty(xml);
                    Assert.Contains("<Loja", xml);
                    Assert.Contains("<Fornecedores>", xml);
                    Assert.Contains("<Clientes>", xml);
                    Assert.Contains("<Produtos>", xml);
                    Assert.Contains("</Loja", xml);
                    Assert.Contains("</Fornecedores>", xml);
                    Assert.Contains("</Clientes>", xml);
                    Assert.Contains("</Produtos>", xml);
                }
            }
        }

        [Fact]
        public async Task SerializeToFileAsync_DeveCriarArquivoXml()
        {
            // Arrange
            var loja = _generator.GerarLojaPequena();

            // Act
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Assert
            Assert.True(File.Exists(_testFilePath));
            //var content = await File.ReadAllTextAsync(_testFilePath);
            // Substitua esta linha:
            // var content = await File.ReadAllTextAsync(_testFilePath);

            // Por estas linhas:
            string content;
            using (var reader = new StreamReader(_testFilePath))
            {
                content = await reader.ReadToEndAsync();
            }
            Assert.Contains("<Loja", content);
            Assert.Contains("</Loja", content);
        }

        [Fact]
        public async Task SerializeToStringAsync_DeveRetornarXmlComoString()
        {
            // Arrange
            var loja = _generator.GerarLojaPequena();

            // Act
            var xml = await _service.SerializeToStringAsync(loja);

            // Assert
            Assert.NotEmpty(xml);
            Assert.Contains("<Loja", xml);
            Assert.Contains("<?xml version=\"1.0\"", xml);
            Assert.Contains("</Loja", xml);
        }

        #endregion

        #region Testes de Desserialização Root

        [Fact]
        public async Task DeserializeRootAsync_DeveDesserializarLojaCompleta()
        {
            // Arrange
            var lojaOriginal = _generator.GerarLoja(3, 5, 4);
            await _service.SerializeToFileAsync(lojaOriginal, _testFilePath);

            // Act
            using (var stream = File.OpenRead(_testFilePath))
            {
                var lojaDesserializada = await _service.DeserializeRootAsync<Loja>(stream);

                // Assert
                Assert.NotNull(lojaDesserializada);
                Assert.Equal(lojaOriginal.FornecedoresLista.Fornecedores.Count, lojaDesserializada.FornecedoresLista.Fornecedores.Count);
                Assert.Equal(lojaOriginal.ProdutosLista.Produtos.Count, lojaDesserializada.ProdutosLista.Produtos.Count);
                Assert.Equal(lojaOriginal.ClientesLista.Clientes.Count, lojaDesserializada.ClientesLista.Clientes.Count);
            }
        }

        [Fact]
        public async Task DeserializeRootFromFileAsync_DeveDesserializarDeArquivo()
        {
            // Arrange
            var lojaOriginal = _generator.GerarLojaPequena();
            await _service.SerializeToFileAsync(lojaOriginal, _testFilePath);

            // Act
            var stream = File.OpenRead(_testFilePath);
            var lojaDesserializada = await _service.DeserializeRootFromFileAsync<Loja>(stream);
            stream.Close();

            // Assert
            Assert.NotNull(lojaDesserializada);
            Assert.Equal(lojaOriginal.FornecedoresLista.Fornecedores.Count, lojaDesserializada.FornecedoresLista.Fornecedores.Count);
        }

        #endregion

        #region Testes de Desserialização Sub-Root

        [Fact]
        public async Task DeserializeSubRootAsync_DeveDesserializarListaProdutos()
        {
            // Arrange
            var loja = _generator.GerarLoja(3, 7, 4);
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Act
            using (var stream = File.OpenRead(_testFilePath))
            {
                var listaProdutos = await _service.DeserializeSubRootAsync<ListaProdutos>(stream, "Produtos");

                // Assert
                Assert.NotNull(listaProdutos);
                Assert.Equal(loja.ProdutosLista.Produtos.Count, listaProdutos.Produtos.Count);
                Assert.Equal(loja.ProdutosLista.PrecoTotal, listaProdutos.PrecoTotal);
            }
        }

        [Fact]
        public async Task DeserializeSubRootFromFileAsync_DeveDesserializarDeArquivo()
        {
            // Arrange
            var loja = _generator.GerarLojaPequena();
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Act
            var stream = File.OpenRead(_testFilePath);
            var listaProdutos = await _service.DeserializeSubRootFromFileAsync<ListaProdutos>(stream, "Produtos");
            stream.Close();

            // Assert
            Assert.NotNull(listaProdutos);
            Assert.NotEmpty(listaProdutos.Produtos);
        }

        [Fact]
        public async Task DeserializeSubRootAsync_ElementoInexistente_DeveLancarExcecao()
        {
            // Arrange
            var loja = _generator.GerarLojaPequena();
            var _testFilePath = Path.Combine("C:\\Temp\\", $"Serializacao_{Guid.NewGuid()}.xml");
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Act & Assert
            var stream = File.OpenRead(_testFilePath);
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _service.DeserializeSubRootAsync<ListaProdutos>(stream, "ElementoInexistente"));
        }

        #endregion

        #region Testes de Desserialização de Coleções

        [Fact]
        public async Task DeserializeCollectionAsync_DeveDesserializarTodosClientes()
        {
            // Arrange
            var loja = _generator.GerarLoja(3, 5, 8);
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Act
            using (var stream = File.OpenRead(_testFilePath))
            {
                var clientes = await _service.DeserializeCollectionAsync<Cliente>(stream, "Clientes", "Cliente");

                // Assert
                Assert.NotNull(clientes);
                Assert.Equal(loja.ClientesLista.Clientes.Count, clientes.Count());
            }
        }

        [Fact]
        public async Task DeserializeCollectionWithCallbackAsync_DeveProcessarCadaItem()
        {
            // Arrange
            var loja = _generator.GerarLoja(3, 5, 6);
            await _service.SerializeToFileAsync(loja, _testFilePath);
            var clientesProcessados = new List<Cliente>();

            // Act
            using (var stream = File.OpenRead(_testFilePath))
            {
                await _service.DeserializeCollectionWithCallbackAsync<Cliente>(
                    stream,
                    "Clientes",
                    "Cliente",
                    cliente => clientesProcessados.Add(cliente));
            }

            // Assert
            Assert.Equal(loja.ClientesLista.Clientes.Count, clientesProcessados.Count);
        }

        #endregion

        #region Testes de Desserialização com Filtro

        [Fact]
        public async Task DeserializeItemByPredicateAsync_DeveEncontrarClientePorEmail()
        {
            // Arrange
            var loja = _generator.GerarLoja(3, 5, 10);
            var clienteEsperado = loja.ClientesLista.Clientes[5];
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Act
            using (var stream = File.OpenRead(_testFilePath))
            {
                var clienteEncontrado = await _service.DeserializeItemByPredicateAsync<Cliente>(
                    stream,
                    "Clientes",
                    "Cliente",
                    c => c.Email == clienteEsperado.Email);

                // Assert
                Assert.NotNull(clienteEncontrado);
                Assert.Equal(clienteEsperado.Id, clienteEncontrado.Id);
                Assert.Equal(clienteEsperado.Nome, clienteEncontrado.Nome);
                Assert.Equal(clienteEsperado.Email, clienteEncontrado.Email);
            }
        }

        [Fact]
        public async Task DeserializeItemByPredicateAsync_DeveEncontrarClientePorId()
        {
            // Arrange
            var loja = _generator.GerarLoja(3, 5, 15);
            var idProcurado = 7;
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Act
            using (var stream = File.OpenRead(_testFilePath))
            {
                var clienteEncontrado = await _service.DeserializeItemByPredicateAsync<Cliente>(
                    stream,
                    "Clientes",
                    "Cliente",
                    c => c.Id == idProcurado);

                // Assert
                Assert.NotNull(clienteEncontrado);
                Assert.Equal(idProcurado, clienteEncontrado.Id);
            }
        }

        [Fact]
        public async Task DeserializeItemByPredicateFromFileAsync_DeveEncontrarClienteDeArquivo()
        {
            // Arrange
            var loja = _generator.GerarLoja(3, 5, 12);
            var emailProcurado = loja.ClientesLista.Clientes[3].Email;
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Act
            var stream = File.OpenRead(_testFilePath);
            var clienteEncontrado = await _service.DeserializeItemByPredicateFromFileAsync<Cliente>(
                stream,
                "Clientes",
                "Cliente",
                c => c.Email == emailProcurado);

            // Assert
            Assert.NotNull(clienteEncontrado);
            Assert.Equal(emailProcurado, clienteEncontrado.Email);
        }

        [Fact]
        public async Task DeserializeItemByPredicateAsync_ClienteNaoEncontrado_DeveRetornarNull()
        {
            // Arrange
            var loja = _generator.GerarLojaPequena();
            await _service.SerializeToFileAsync(loja, _testFilePath);

            // Act
            using (var stream = File.OpenRead(_testFilePath))
            {
                var clienteEncontrado = await _service.DeserializeItemByPredicateAsync<Cliente>(
                    stream,
                    "Clientes",
                    "Cliente",
                    c => c.Email == "naoexiste@email.com");

                // Assert
                Assert.Null(clienteEncontrado);
            }
        }

        #endregion

        #region Testes de Validação de Parâmetros

        [Fact]
        public async Task DeserializeRootAsync_StreamNull_DeveLancarArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _service.DeserializeRootAsync<Loja>(null));
        }

        [Fact]
        public async Task DeserializeSubRootAsync_ElementNameVazio_DeveLancarArgumentException()
        {
            using (var stream = new MemoryStream())
            {
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await _service.DeserializeSubRootAsync<ListaProdutos>(stream, ""));
            }
        }

        [Fact]
        public async Task DeserializeCollectionAsync_ArrayElementNameNull_DeveLancarArgumentException()
        {
            using (var stream = new MemoryStream())
            {
                await Assert.ThrowsAsync<ArgumentException>(
                    async () => await _service.DeserializeCollectionAsync<Cliente>(stream, null, "Cliente"));
            }
        }

        [Fact]
        public async Task DeserializeItemByPredicateAsync_PredicateNull_DeveLancarArgumentNullException()
        {
            using (var stream = new MemoryStream())
            {
                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await _service.DeserializeItemByPredicateAsync<Cliente>(
                        stream,
                        "Clientes",
                        "Cliente",
                        null));
            }
        }

        [Fact]
        public async Task SerializeAsync_ObjectNull_DeveLancarArgumentNullException()
        {
            using (var stream = new MemoryStream())
            {
                await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await _service.SerializeAsync<Loja>(null, stream));
            }
        }

        #endregion

        #region Testes de Integração Completos

        [Fact]
        public async Task IntegracaoCompleta_SerializarEDesserializarComTodasOperacoes()
        {
            // Arrange
            var lojaOriginal = _generator.GerarLoja(5, 10, 15);
            await _service.SerializeToFileAsync(lojaOriginal, _testFilePath);

            var stream = File.OpenRead(_testFilePath);

            // Act & Assert - Desserialização Root
            var lojaCompleta = await _service.DeserializeRootFromFileAsync<Loja>(stream);
            Assert.NotNull(lojaCompleta);
            Assert.Equal(lojaOriginal.FornecedoresLista.Fornecedores.Count, lojaCompleta.FornecedoresLista.Fornecedores.Count);

            stream.Close();

            stream = File.OpenRead(_testFilePath);

            // Act & Assert - Desserialização Sub-Root
            var listaProdutos = await _service.DeserializeSubRootFromFileAsync<ListaProdutos>(stream, "Produtos");
            Assert.NotNull(listaProdutos);
            Assert.Equal(lojaOriginal.ProdutosLista.Produtos.Count, listaProdutos.Produtos.Count);

            stream.Close();

            // Act & Assert - Desserialização de Coleção
            using (var fstream = File.OpenRead(_testFilePath))
            {
                var fornecedores = await _service.DeserializeCollectionAsync<Fornecedor>(fstream, "Fornecedores", "Fornecedor");
                Assert.Equal(lojaOriginal.FornecedoresLista.Fornecedores.Count, fornecedores.Count());
            }

            stream = File.OpenRead(_testFilePath);

            // Act & Assert - Busca com Predicado
            var clienteEsperado = lojaOriginal.ClientesLista.Clientes[7];
            var clienteEncontrado = await _service.DeserializeItemByPredicateFromFileAsync<Cliente>(
                stream,
                "Clientes",
                "Cliente",
                c => c.Id == clienteEsperado.Id);

            stream.Close();

            Assert.NotNull(clienteEncontrado);
            Assert.Equal(clienteEsperado.Email, clienteEncontrado.Email);
        }

        [Fact]
        public async Task IntegracaoCompleta_TestarStreamRewind()
        {
            // Arrange
            var loja = _generator.GerarLojaPequena();
            var stream = new MemoryStream();
            await _service.SerializeAsync(loja, stream);

            // Act - Múltiplas leituras do mesmo stream
            var loja1 = await _service.DeserializeRootAsync<Loja>(stream);
            var loja2 = await _service.DeserializeRootAsync<Loja>(stream);

            // Assert
            Assert.NotNull(loja1);
            Assert.NotNull(loja2);
            Assert.Equal(loja1.ClientesLista.Clientes.Count, loja2.ClientesLista.Clientes.Count);
        }

        #endregion

        //[Fact]
        //public async Task DeserializeRootAsync_ComCancellationToken_DeveCancelarOperacao()
        //{
        //    // Arrange
        //    var loja = _generator.GerarLojaGrande();
        //    await _service.SerializeToFileAsync(loja, _testFilePath);
        //    var cts = new CancellationTokenSource();
        //    cts.Cancel();

        //    // Act & Assert
        //    using (var stream = File.OpenRead(_testFilePath))
        //    {
        //        await Assert.ThrowsAsync<OperationCanceledException>(
        //            async () => await _service.DeserializeRootAsync<Loja>(stream, cts.Token));
        //    }
        //}
        //[Fact]
        //public async Task DeserializeItemByPredicateAsync_ComCancellationToken_DeveCancelarOperacao()
        //{
        //    // Arrange
        //    var loja = _generator.GerarLojaGrande();
        //    await _service.SerializeToFileAsync(loja, _testFilePath);
        //    var cts = new CancellationTokenSource();
        //    cts.CancelAfter(1); // Cancela após 1ms

        //    // Act & Assert
        //    using (var stream = File.OpenRead(_testFilePath))
        //    {
        //        await Assert.ThrowsAnyAsync<OperationCanceledException>(
        //            async () => await _service.DeserializeItemByPredicateAsync<Cliente>(
        //                stream,
        //                "Clientes",
        //                "Cliente",
        //                c => c.Id == 999,
        //                cts.Token));
        //    }
        //}
        //[Fact]
        //public async Task DeserializeCollectionAsync_ComCancellationToken_DeveCancelarOperacao()
        //{
        //    // Arrange
        //    var loja = _generator.GerarLojaGrande();
        //    await _service.SerializeToFileAsync(loja, _testFilePath);
        //    var cts = new CancellationTokenSource();
        //    cts.CancelAfter(100); // Cancela após 1ms

        //    // Act & Assert
        //    using (var stream = File.OpenRead(_testFilePath))
        //    {
        //        // await Assert.ThrowsAnyAsync<OperationCanceledException>(
        //        //    async () => await _service.DeserializeCollectionAsync<Cliente>(stream, "Clientes", "Cliente", cts.Token));
        //        try
        //        {
        //            await _service.DeserializeCollectionAsync<Cliente>(stream, "Clientes", "Cliente", cts.Token);
        //        }
        //        catch (Exception ex)
        //        {
        //            var x = ex.Message;
        //        }
        //    }
        //}
        //[Fact]
        //public async Task SerializeAsync_ComCancellationToken_DeveCancelarOperacao()
        //{
        //    // Arrange
        //    var loja = _generator.GerarLojaGrande();
        //    var cts = new CancellationTokenSource();
        //    cts.Cancel();

        //    // Act & Assert
        //    using (var stream = new MemoryStream())
        //    {
        //        await Assert.ThrowsAsync<OperationCanceledException>(
        //            async () => await _service.SerializeAsync(loja, stream, cts.Token));
        //    }
        //}
    }
}
