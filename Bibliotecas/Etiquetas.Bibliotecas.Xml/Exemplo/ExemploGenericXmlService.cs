using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.Xml.Exemplo.Modelos;
using Etiquetas.Bibliotecas.Xml.Servicos;

namespace Etiquetas.Bibliotecas.Xml.Exemplo
{
    /// <summary>
    /// Classe de exemplo demonstrando o uso do GenericXmlService.
    /// </summary>
    public class ExemploGenericXmlService
    {
        private readonly GenericXmlService _xmlService;
        private readonly XmlDataGenerator _dataGenerator;

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public ExemploGenericXmlService()
        {
            _xmlService = new GenericXmlService();
            _dataGenerator = new XmlDataGenerator();
        }

        /// <summary>
        /// Exemplo 1: Serializar e desserializar uma loja completa (Root).
        /// </summary>
        public async Task Exemplo1_SerializarEDesserializarRoot()
        {
            Console.WriteLine("=== Exemplo 1: Serializar e Desserializar Root ===\n");

            // Gerar dados de teste
            var loja = _dataGenerator.GerarLoja(5, 10, 15);
            var filePath = "loja_completa.xml";

            // Serializar para arquivo
            using (var stream = File.OpenWrite(filePath))
            {
                await _xmlService.SerializeAsync(loja, stream);
            }
            
            Console.WriteLine($"Loja serializada para: {filePath}");

            // Desserializar de arquivo
            using (var stream = File.OpenRead(filePath))
            {
                using (var reader = new StreamReader(stream))
                {
                    var lojaDesserializada = await _xmlService.DeserializeRootAsync<Loja>(stream);
                    Console.WriteLine($"Loja desserializada com sucesso!");
                    Console.WriteLine($"- Fornecedores: {lojaDesserializada.FornecedoresLista.Fornecedores.Count}");
                    Console.WriteLine($"- Produtos: {lojaDesserializada.ProdutosLista.Produtos.Count}");
                    Console.WriteLine($"- Clientes: {lojaDesserializada.ClientesLista.Clientes.Count}\n");
                }
            }
        }

        /// <summary>
        /// Exemplo 2: Desserializar apenas uma sub-root (ListaProdutos).
        /// </summary>
        public async Task Exemplo2_DesserializarSubRoot()
        {
            Console.WriteLine("=== Exemplo 2: Desserializar Sub-Root ===\n");

            // Preparar arquivo
            var loja = _dataGenerator.GerarLoja(3, 8, 5);
            var filePath = "loja_subroot.xml";

            // Serializar para arquivo
            using (var stream = File.OpenWrite(filePath))
            {
                await _xmlService.SerializeAsync(loja, stream);
            }

            // Desserializar apenas a ListaProdutos
            using (var stream = File.OpenRead(filePath))
            {
                var listaProdutos = await _xmlService.DeserializeSubRootAsync<ListaProdutos>(
                stream,
                "Produtos");

                Console.WriteLine($"ListaProdutos desserializada com sucesso!");
                Console.WriteLine($"- Total de produtos: {listaProdutos.Produtos.Count}");
                Console.WriteLine($"- Preço total: R$ {listaProdutos.PrecoTotal:F2}\n");
            }
        }

        //public async Task Exemplo3_DesserializarColecao()
        //{
        //    Console.WriteLine("=== Exemplo 3: Desserializar Coleção ===\n");

        //    // Preparar arquivo
        //    var loja = _dataGenerator.GerarLoja(2, 5, 10);
        //    var filePath = "loja_colecao.xml";
        //    await _xmlService.SerializeToFileAsync(loja, filePath);

        //    // Desserializar todos os clientes
        //    using (var stream = File.OpenRead(filePath))
        //    {
        //        var clientes = await _xmlService.DeserializeCollectionAsync<Cliente>(
        //            stream,
        //            "Clientes",
        //            "Cliente");

        //        Console.WriteLine($"Clientes desserializados: {clientes.Count()}");
        //        foreach (var cliente in clientes)
        //        {
        //            Console.WriteLine($"  - {cliente.Nome} ({cliente.Email})");
        //        }
        //        Console.WriteLine();
        //    }
        //}

        /// <summary>
        /// Exemplo 4: Buscar um cliente específico por email usando predicado.
        /// </summary>
        public async Task Exemplo4_BuscarComPredicado()
        {
            Console.WriteLine("=== Exemplo 4: Buscar com Predicado ===\n");

            // Preparar arquivo
            var loja = _dataGenerator.GerarLoja(2, 5, 20);
            var filePath = "loja_busca.xml";

            // Serializar para arquivo
            using (var stream = File.OpenWrite(filePath))
            {
                await _xmlService.SerializeAsync(loja, stream);
            }

            // Buscar cliente com ID específico
            using (var stream = File.OpenRead(filePath))
            {
                var clienteProcurado = await _xmlService.DeserializeItemByPredicateAsync<Cliente>(
                stream,
                "Clientes",
                "Cliente",
                c => c.Id == 10);

                if (clienteProcurado != null)
                {
                    Console.WriteLine($"Cliente encontrado:");
                    Console.WriteLine($"  - ID: {clienteProcurado.Id}");
                    Console.WriteLine($"  - Nome: {clienteProcurado.Nome}");
                    Console.WriteLine($"  - Email: {clienteProcurado.Email}\n");
                }
                else
                {
                    Console.WriteLine("Cliente não encontrado.\n");
                }
            }

        }

        /// <summary>
        /// Exemplo 5: Processar coleção com callback (streaming parcial).
        /// </summary>
        public async Task Exemplo5_ProcessarComCallback()
        {
            Console.WriteLine("=== Exemplo 5: Processar com Callback ===\n");

            // Preparar arquivo
            var loja = _dataGenerator.GerarLoja(2, 5, 12);
            var filePath = "loja_callback.xml";

            // Serializar para arquivo
            using (var stream = File.OpenWrite(filePath))
            {
                await _xmlService.SerializeAsync(loja, stream);
            }

            // Processar cada fornecedor conforme é lido
            var contador = 0;
            using (var stream = File.OpenRead(filePath))
            {
                await _xmlService.DeserializeCollectionWithCallbackAsync<Fornecedor>(
                    stream,
                    "Fornecedores",
                    "Fornecedor",
                    fornecedor =>
                    {
                        contador++;
                        Console.WriteLine($"  [{contador}] {fornecedor.Nome} - {fornecedor.Contato}");
                    });
            }
            Console.WriteLine($"\nTotal processado: {contador} fornecedores\n");
        }

        /// <summary>
        /// Exemplo 6: Usar CancellationToken para cancelar operação.
        /// </summary>
        public async Task Exemplo6_UsarCancellationToken()
        {
            Console.WriteLine("=== Exemplo 6: Usar CancellationToken ===\n");

            // Preparar arquivo grande
            var loja = _dataGenerator.GerarLojaGrande();
            var filePath = "loja_grande.xml";

            // Serializar para arquivo
            using (var stream = File.OpenWrite(filePath))
            {
                await _xmlService.SerializeAsync(loja, stream);
            }

            // Criar token de cancelamento com timeout
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(50)); // Cancela após 50ms

            try
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var clientes = await _xmlService.DeserializeCollectionAsync<Cliente>(
                        stream,
                        "Clientes",
                        "Cliente",
                        null,
                        cts.Token);

                    Console.WriteLine($"Operação concluída: {clientes.Count()} clientes");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operação cancelada pelo CancellationToken!\n");
            }
        }

        /// <summary>
        /// Exemplo 7: Serializar para string XML.
        /// </summary>
        public async Task Exemplo7_SerializarParaString()
        {
            Console.WriteLine("=== Exemplo 7: Serializar para String ===\n");

            // Criar um cliente simples
            var cliente = new Cliente
            {
                Id = 1,
                Nome = "João Silva",
                Email = "joao.silva@email.com"
            };

            // Serializar para string
            var xml = await _xmlService.SerializeToStringAsync(cliente);
            Console.WriteLine("XML gerado:");
            Console.WriteLine(xml);
            Console.WriteLine();
        }

        /// <summary>
        /// Exemplo 8: Reutilizar stream (rewind automático).
        /// </summary>
        public async Task Exemplo8_ReutilizarStream()
        {
            Console.WriteLine("=== Exemplo 8: Reutilizar Stream ===\n");

            // Criar loja e serializar em memória
            var loja = _dataGenerator.GerarLojaPequena();
            var stream = new MemoryStream();
            await _xmlService.SerializeAsync(loja, stream);

            // Primeira leitura
            var loja2 = await _xmlService.DeserializeRootAsync<ListaFornecedores>(stream);
            Console.WriteLine($"Segunda leitura: {loja2.Fornecedores.Count} fornecedores");

            // Segunda leitura (rewind automático)
            var loja1 = await _xmlService.DeserializeRootAsync<ListaClientes>(stream);
            Console.WriteLine($"Primeira leitura: {loja1.Clientes.Count} clientes");

            // Terceira leitura de sub-root
            var listaProdutos = await _xmlService.DeserializeSubRootAsync<ListaProdutos>(stream, "Produtos");
            Console.WriteLine($"Terceira leitura: {listaProdutos.Produtos.Count} produtos\n");

            stream.Flush();
            stream.Close();
        }

        /// <summary>
        /// Executa todos os exemplos.
        /// </summary>
        public async Task ExecutarTodosExemplos()
        {
            await Exemplo1_SerializarEDesserializarRoot();
            await Exemplo2_DesserializarSubRoot();
            //await Exemplo3_DesserializarColecao();
            await Exemplo4_BuscarComPredicado();
            await Exemplo5_ProcessarComCallback();
            //await Exemplo6_UsarCancellationToken();
            await Exemplo7_SerializarParaString();
            await Exemplo8_ReutilizarStream();

            Console.WriteLine("=== Todos os exemplos executados com sucesso! ===");
        }
    }
}
