namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Objeto de transferência de dados (DTO) para a entidade FaltaImprimir.
    /// </summary>
    /// <remarks>Esta classe destina-se ao uso interno e facilita a transferência de dados relacionados a itens
    /// de copias impressas com status e copias faltantes. Não se destina ao uso direto em APIs externas.</remarks>
    public interface IFaltaImprimirDto
    {
        /// <summary>
        /// Gets or sets - obtém ou define o identificador único da solicitação de impressão.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o identificador da etiqueta de impressão associado.
        /// </summary>
        string IdEtiquetaImpressao { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o nome do JOB de impressão.
        /// </summary>
        string NomeDoJOB { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a data da impressão de uma Etiqueta.
        /// </summary>
        string DataImpressao { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o status da impressão da Etiqueta.
        /// </summary>
        byte StatusImpressora { get; set; }

        /// <summary>
        /// Gets or sets - A descrição do status da impressão da etiqueta.
        /// </summary>
        string DescricaoStatusImpressora { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a quantidade de etiquetas que faltam ser impressas.
        /// </summary>
        long FaltaImpressao { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// Gets or sets - obtém ou define se a impressora está online.
        /// </summary>
        bool IsOnline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// Gets or sets - obtém ou define se houve um erro na impressão.
        /// </summary>
        bool IsError { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o estado atual da impressão.
        /// </summary>
        string State { get; set; }
    }
}
