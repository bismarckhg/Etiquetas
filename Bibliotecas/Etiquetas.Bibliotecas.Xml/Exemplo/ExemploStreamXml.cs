using Etiquetas.Bibliotecas.TaskCore;
using Etiquetas.Bibliotecas.Xml.Exemplo.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Xml.Exemplo
{
    public class ExemploStreamXml
    {
        private readonly StreamXml XmlStream;
        private readonly XmlDataGenerator _dataGenerator;


        public ExemploStreamXml()
        {
            this.XmlStream = new StreamXml();
            this._dataGenerator = new XmlDataGenerator();
        }

        public async Task ExecutarTodosExemplos()
        {
            await Exemplo1_SerializarEDesserializarRoot();
            await Exemplo2_DesserializarSubRoot();
            await Exemplo3_DesserializarColecao();
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

            // Serializar
            var parametros = new TaskParametros();
            parametros.Armazena<string>(filePath, "NomeCaminhoArquivo");

            Console.WriteLine($"Conectado ao arquivo XML:{filePath}");
            await XmlStream.ConectarAsync(parametros).ConfigureAwait(false);
            Console.WriteLine($"Serializando dados da loja.");

            var novoParametros = new TaskParametros();
            novoParametros.Armazena<Loja>(loja, "Objeto");

            await XmlStream.EscreverAsync<Loja>(novoParametros).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"Dados serializados com sucesso em {filePath}.");
            Console.WriteLine();

            Console.WriteLine("Conteúdo do arquivo XML: ");
            var xml = System.IO.File.ReadAllText(filePath);
            Console.WriteLine(xml);
            Console.WriteLine();


            Console.WriteLine($"Conectado ao arquivo XML:{filePath}");
            await XmlStream.ConectarAsync(parametros).ConfigureAwait(false);

            var parametrosLeitura = new TaskParametros();
            parametrosLeitura.ArmazenaCancellationToken(new CancellationTokenSource().Token);

            Console.WriteLine($"Deserializando dados da loja.");
            var lojaDesserializada = await XmlStream.LerAsync<Loja>(parametrosLeitura).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"Dados serializados com sucesso em {filePath}.");

            Console.WriteLine($"- Fornecedores: {lojaDesserializada.FornecedoresLista.Fornecedores.Count}");
            Console.WriteLine($"- Produtos: {lojaDesserializada.ProdutosLista.Produtos.Count}");
            Console.WriteLine($"- Clientes: {lojaDesserializada.ClientesLista.Clientes.Count}\n");

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

            // Serializar
            var parametros = new TaskParametros();
            parametros.Armazena<string>(filePath, "NomeCaminhoArquivo");

            Console.WriteLine($"Conectado ao arquivo XML:{filePath}");
            await XmlStream.ConectarAsync(parametros).ConfigureAwait(false);
            Console.WriteLine($"Serializando dados da loja.");

            var novoParametros = new TaskParametros();
            novoParametros.Armazena<Loja>(loja, "Objeto");

            await XmlStream.EscreverAsync<Loja>(novoParametros).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"Dados serializados com sucesso em {filePath}.");
            Console.WriteLine();

            Console.WriteLine("Conteúdo do arquivo XML: ");
            var xml = System.IO.File.ReadAllText(filePath);
            Console.WriteLine(xml);
            Console.WriteLine();

            Console.WriteLine($"Conectado ao arquivo XML:{filePath}");
            await XmlStream.ConectarAsync(parametros).ConfigureAwait(false);

            var parametrosLeitura = new TaskParametros();
            parametrosLeitura.ArmazenaCancellationToken(new CancellationTokenSource().Token);
            parametrosLeitura.Armazena<string>("Produtos", "NomeSubRoot");

            Console.WriteLine($"Deserializando dados dos Produtos da loja.");
            var lojaDesserializada = await XmlStream.LerAsync<Loja>(parametrosLeitura).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"Dados serializados com sucesso em {filePath}.");

            Console.WriteLine($"Produtos desserializada com sucesso!");
            Console.WriteLine($"- Total de produtos: {lojaDesserializada.ProdutosLista.Produtos.Count}");
            Console.WriteLine($"- Preço total: R$ {lojaDesserializada.ProdutosLista.PrecoTotal:F2}\n");
        }

        /// <summary>
        /// Exemplo 3: Desserializar uma coleção completa de clientes.
        /// </summary>
        public async Task Exemplo3_DesserializarColecao()
        {
            Console.WriteLine("=== Exemplo 3: Desserializar Sub-Root ===\n");

            // Preparar arquivo
            var loja = _dataGenerator.GerarLoja(2, 5, 10);
            var filePath = "loja_colecao.xml";

            // Serializar
            var parametros = new TaskParametros();
            parametros.Armazena<string>(filePath, "NomeCaminhoArquivo");

            Console.WriteLine($"Conectado ao arquivo XML:{filePath}");
            await XmlStream.ConectarAsync(parametros).ConfigureAwait(false);
            Console.WriteLine($"Serializando dados da loja.");

            var novoParametros = new TaskParametros();
            novoParametros.Armazena<Loja>(loja, "Objeto");

            await XmlStream.EscreverAsync<Loja>(novoParametros).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"Dados serializados com sucesso em {filePath}.");
            Console.WriteLine();

            Console.WriteLine("Conteúdo do arquivo XML: ");
            var xml = System.IO.File.ReadAllText(filePath);
            Console.WriteLine(xml);
            Console.WriteLine();

            Console.WriteLine($"Conectado ao arquivo XML:{filePath}");
            await XmlStream.ConectarAsync(parametros).ConfigureAwait(false);

            var parametrosLeitura = new TaskParametros();
            parametrosLeitura.ArmazenaCancellationToken(new CancellationTokenSource().Token);
            parametrosLeitura.Armazena<string>("Clientes", "NomeSubRoot");

            Console.WriteLine($"Deserializando dados dos Produtos da loja.");
            var lojaDesserializada = await XmlStream.LerAsync<Loja>(parametrosLeitura).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"Dados serializados com sucesso em {filePath}.");

            Console.WriteLine($"Clientes desserializados: {lojaDesserializada.ClientesLista.Clientes.Count()}");
            foreach (var cliente in lojaDesserializada.ClientesLista.Clientes)
            {
                Console.WriteLine($"  - {cliente.Nome} ({cliente.Email})");
            }
            Console.WriteLine();
        }

    }
}
