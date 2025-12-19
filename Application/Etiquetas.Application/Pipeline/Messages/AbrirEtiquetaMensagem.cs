using System;

namespace Etiquetas.Application.Pipeline.Messages
{
    /// <summary>
    /// Mensagem para abrir uma nova etiqueta de impressão.
    /// </summary>
    public class AbrirEtiquetaMensagem : EtiquetaMensagemBase
    {
        /// <summary>
        /// Gets or sets - Código do material.
        /// </summary>
        public string CodigoMaterial { get; set; }

        /// <summary>
        /// Gets or sets - Código de barras.
        /// </summary>
        public string CodigoBarras { get; set; }

        /// <summary>
        /// Gets or sets - Descrição do medicamento.
        /// </summary>
        public string DescricaoMedicamento { get; set; }

        /// <summary>
        /// Gets or sets - Primeiro princípio ativo.
        /// </summary>
        public string PrincipioAtivo1 { get; set; }

        /// <summary>
        /// Gets or sets - Segundo princípio ativo.
        /// </summary>
        public string PrincipioAtivo2 { get; set; }

        /// <summary>
        /// Gets or sets - Número do lote.
        /// </summary>
        public string Lote { get; set; }

        /// <summary>
        /// Gets or sets - Data de validade.
        /// </summary>
        public string Validade { get; set; }

        /// <summary>
        /// Gets or sets - Matrícula do funcionário.
        /// </summary>
        public string MatriculaFuncionario { get; set; }

        /// <summary>
        /// Gets or sets - Quantidade de cópias solicitadas.
        /// </summary>
        public long QuantidadeSolicitada { get; set; }

        /// <summary>
        /// Gets or sets - Nome do JOB de impressão.
        /// </summary>
        public string JobName { get; set; }
    }

}
