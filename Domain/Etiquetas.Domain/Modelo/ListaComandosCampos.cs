using Etiquetas.Bibliotecas.SATO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Etiquetas.Domain.Modelo
{
    /// <summary>
    /// Container para lista de comandos de campos.
    /// </summary>
    [Serializable]
    [XmlRoot("ListaComandosCampos")]
    public class ListaComandosCampos : IListaComandosCampos
    {
        /// <summary>
        /// Gets or sets - Lista de comandos de campos.
        /// </summary>
        [XmlArray("Comandos")]
        [XmlArrayItem("Comando")]
        public List<ComandosPadraoImpressora> Comandos { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListaComandosCampos"/> class.
        /// Inicializa uma nova instância da classe <see cref="ListaComandosCampos"/>.
        /// </summary>
        public ListaComandosCampos()
        {
            Comandos = new List<ComandosCampo>();
        }
    }

    //{
    //    /// <summary>
    //    /// Gets or sets - Código Material - Posição em ^FO.
    //    /// </summary>
    //    public string CodigoMaterial { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Código Material é obrigatório.
    //    /// </summary>
    //    public bool CodigoMaterial_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Descrição 1 - Posição única.
    //    /// </summary>
    //    public string DescricaoMedicamento { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Descrição Medicamento é obrigatório.
    //    /// </summary>
    //    public bool DescricaoMedicamento_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Descrição 2 - Posição única.
    //    /// </summary>
    //    public string DescricaoMedicamento2 { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Descrição Medicamento 2 é obrigatório. 
    //    /// </summary>
    //    public bool DescricaoMedicamento2_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Princípio Ativo posicao Unica.
    //    /// </summary>
    //    public string PrincipioAtivo { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Princípio Ativo é obrigatório. 
    //    /// </summary>
    //    public bool PrincipioAtivo_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Princípio Ativo 2, posicao única. 
    //    /// </summary>
    //    public string PrincipioAtivo2 { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Princípio Ativo 2 posição 2.
    //    /// </summary>
    //    public string PrincipioAtivo2 { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Princípio Ativo 2 é obrigatório. 
    //    /// </summary>
    //    public bool PrincipioAtivo2_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Embalagem posição única.
    //    /// </summary>
    //    public string Embalagem { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Embalagem posicao 2.
    //    /// </summary>
    //    public string Embalagem { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Embalagem é obrigatório. 
    //    /// </summary>
    //    public bool Embalagem_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Lote posicao única.
    //    /// </summary>
    //    public string Lote { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Lote posição 2.
    //    /// </summary>
    //    public string Lote { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Lote é obrigatório. 
    //    /// </summary>
    //    public bool Lote_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Validade posição única.
    //    /// </summary>
    //    public string Validade { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Validade posição 2.
    //    /// </summary>
    //    public string Validade { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Validade é obrigatório. 
    //    /// </summary>
    //    public bool Validade_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Usuário posição única.
    //    /// </summary>
    //    public string CodigoUsuario { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Usuário posição 2.
    //    /// </summary>
    //    public string CodigoUsuario { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Código Usuário é obrigatório. 
    //    /// </summary>
    //    public bool CodigoUsuario_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Código de Barras posição única.
    //    /// </summary>
    //    public string CodigoBarras { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Código de Barras posição 2.
    //    /// </summary>
    //    public string CodigoBarras { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Código de Barras é obrigatório. 
    //    /// </summary>
    //    public bool CodigoBarras_Obrigatorio { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Cópias posição única.
    //    /// </summary>
    //    public string Copias { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Cópias.
    //    /// </summary>
    //    public string Copias { get; set; }

    //    /// <summary>
    //    /// Gets or sets - Indica se o campo Cópias é obrigatório. 
    //    /// </summary>
    //    public bool Copias_Obrigatorio { get; set; }
    //}
}
