namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Objeto de transferência de dados (DTO) para a entidade EtiquetaImpressao.
    /// Utilizado para transferir dados entre a camada de aplicação e a interface de usuário ou API.
    /// </summary>
    /// <remarks>Esta classe destina-se ao uso interno e facilita a transferência de dados relacionados aos dados da etiqueta.</remarks>
    public interface IEtiquetaImpressaoDto
    {
        /// <summary>
        /// Gets or sets - obtém ou define o identificador único da etiqueta.
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a descrição do medicamento.
        /// </summary>
        string DescricaoMedicamento { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a descrição do medicamento.
        /// </summary>
        string DescricaoMedicamento2 { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o primeiro princípio ativo.
        /// </summary>
        string PrincipioAtivo { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o segundo princípio ativo.
        /// </summary>
        string PrincipioAtivo2 { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o código do material.
        /// </summary>
        string CodigoMaterial { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a data de validade.
        /// </summary>
        string Validade { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o lote do medicamento.
        /// </summary>
        string Lote { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a Embalagem do medicamento.
        /// </summary>
        string Embalagem { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a matrícula do funcionário responsável.
        /// </summary>
        string CodigoUsuario { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o código de barras.
        /// </summary>
        string CodigoBarras { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a data e hora de início da impressão.
        /// </summary>
        string DataHoraInicio { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a data e hora de fim da impressão.
        /// </summary>
        string DataHoraFim { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o status da impressão.
        /// </summary>
        char StatusEtiqueta { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a descrição do status.
        /// </summary>
        string DescricaoStatus { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a quantidade solicitada para impressão.
        /// </summary>
        long QuantidadeSolicitada { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a quantidade restante a ser impressa.
        /// </summary>
        long FaltaImpressao { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o nome do trabalho de impressão associado a esta etiqueta.
        /// </summary>
        string JobName { get; set; }

    }
}
