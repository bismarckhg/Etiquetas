using System;
using System.Collections.Generic;
using System.Linq;
using Etiquetas.Bibliotecas.Xml.Exemplo.Modelos;

namespace Etiquetas.Bibliotecas.Xml.Exemplo
{
    /// <summary>
    /// Gerador de dados de teste para XML.
    /// </summary>
    public class XmlDataGenerator
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Gera uma lista de fornecedores de teste.
        /// </summary>
        public ListaFornecedores GerarFornecedores(int quantidade)
        {
            var fornecedores = new ListaFornecedores();

            for (int i = 1; i <= quantidade; i++)
            {
                fornecedores.Fornecedores.Add(new Fornecedor
                {
                    Id = i,
                    Nome = $"Fornecedor {i}",
                    Contato = $"contato{i}@fornecedor.com.br"
                });
            }

            return fornecedores;
        }

        /// <summary>
        /// Gera uma lista de produtos de teste.
        /// </summary>
        public List<Produto> GerarProdutos(int quantidade)
        {
            var produtos = new List<Produto>();
            var categorias = new[] { "Eletrônicos", "Móveis", "Roupas", "Alimentos", "Livros" };

            for (int i = 1; i <= quantidade; i++)
            {
                var categoria = categorias[_random.Next(categorias.Length)];
                produtos.Add(new Produto
                {
                    Codigo = $"PROD{i:D5}",
                    Descricao = $"{categoria} - Produto {i}",
                    Preco = Math.Round(_random.NextDouble() * 1000 + 10, 2)
                });
            }

            return produtos;
        }

        /// <summary>
        /// Gera uma lista de clientes de teste.
        /// </summary>
        public ListaClientes GerarClientes(int quantidade)
        {
            var clientes = new ListaClientes();
            var nomes = new[] { "João", "Maria", "Pedro", "Ana", "Carlos", "Juliana", "Ricardo", "Fernanda", "Lucas", "Beatriz" };
            var sobrenomes = new[] { "Silva", "Santos", "Oliveira", "Souza", "Pereira", "Costa", "Rodrigues", "Almeida", "Nascimento", "Lima" };

            for (int i = 1; i <= quantidade; i++)
            {
                var nome = nomes[_random.Next(nomes.Length)];
                var sobrenome = sobrenomes[_random.Next(sobrenomes.Length)];
                var nomeCompleto = $"{nome} {sobrenome}";

                clientes.Clientes.Add(new Cliente
                {
                    Id = i,
                    Nome = nomeCompleto,
                    Email = $"{nome.ToLower()}.{sobrenome.ToLower()}{i}@email.com.br"
                });
            }

            return clientes;
        }

        /// <summary>
        /// Gera uma lista de produtos completa com preço total.
        /// </summary>
        public ListaProdutos GerarListaProdutos(int quantidade)
        {
            var produtos = GerarProdutos(quantidade);
            var precoTotal = produtos.Sum(p => p.Preco);

            return new ListaProdutos
            {
                Produtos = produtos,
                PrecoTotal = Math.Round(precoTotal, 2)
            };
        }

        /// <summary>
        /// Gera uma loja completa com fornecedores, produtos e clientes.
        /// </summary>
        public Loja GerarLoja(int qtdFornecedores = 5, int qtdProdutos = 10, int qtdClientes = 15)
        {
            return new Loja
            {
                FornecedoresLista = GerarFornecedores(qtdFornecedores),
                ProdutosLista = GerarListaProdutos(qtdProdutos),
                ClientesLista = GerarClientes(qtdClientes)
            };
        }

        /// <summary>
        /// Gera uma loja grande para testes de performance.
        /// </summary>
        public Loja GerarLojaGrande()
        {
            return GerarLoja(100, 500, 1000);
        }

        /// <summary>
        /// Gera uma loja pequena para testes rápidos.
        /// </summary>
        public Loja GerarLojaPequena()
        {
            return GerarLoja(2, 5, 3);
        }
    }
}
