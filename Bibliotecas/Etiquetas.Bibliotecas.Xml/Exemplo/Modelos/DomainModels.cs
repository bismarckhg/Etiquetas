using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Etiquetas.Bibliotecas.Xml.Exemplo.Modelos
{
    [Serializable]
    public class Fornecedor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Contato { get; set; }
    }

    [Serializable]
    [XmlType("Fornecedores")]
    public class ListaFornecedores
    {
        [XmlElement("Fornecedor")]
        public List<Fornecedor> Fornecedores { get; set; } = new List<Fornecedor>();
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

    [Serializable]
    [XmlType("Clientes")]
    public class ListaClientes
    {
        [XmlElement("Cliente")]
        public List<Cliente> Clientes { get; set; } = new List<Cliente>();
    }

    [Serializable]
    [XmlType("Produtos")]
    public class ListaProdutos
    {
        [XmlElement("Produto")]
        public List<Produto> Produtos { get; set; } = new List<Produto>();

        public double PrecoTotal { get; set; }
    }

    [Serializable]
    [XmlRoot("Loja")]
    public class Loja
    {
        [XmlElement("Fornecedores")]
        public ListaFornecedores FornecedoresLista { get; set; }

        [XmlElement("Produtos")]
        public ListaProdutos ProdutosLista { get; set; }

        [XmlElement("Clientes")]
        public ListaClientes ClientesLista { get; set; }
    }
}
