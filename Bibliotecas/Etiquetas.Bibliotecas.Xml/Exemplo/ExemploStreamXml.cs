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
        }

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
    }
}
