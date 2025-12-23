using System;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Esta entidade encapsula todas as informações relevantes para o controle de Etiquetas.
    /// </summary>
    public interface IEtiquetaImpressao : IContratoEntidade
    {
        /// <summary>
        /// Gets or sets - obtém ou define o identificador único da solicitação de impressão da etiqueta.
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a primeira parte da descrição completa do medicamento para o qual a etiqueta será impressa.
        /// Não pode ser nulo ou vazio.
        /// </summary>
        string DescricaoMedicamento { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define segunda parte da descrição completa do medicamento para o qual a etiqueta será impressa.
        /// Não pode ser nulo ou vazio.
        /// </summary>
        string DescricaoMedicamento2 { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o primeira parte do texto do princípio ativo do medicamento.
        /// Pode ser nulo ou vazio se não aplicável.
        /// </summary>
        string PrincipioAtivo { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o segunda parte do texto do princípio ativo do medicamento, se houver.
        /// Pode ser nulo ou vazio se não aplicável.
        /// </summary>
        string PrincipioAtivo2 { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o código de material associado ao medicamento.
        /// </summary>
        long CodigoMaterial { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a data de validade do lote do medicamento.
        /// </summary>
        DateTime Validade { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o número do lote do medicamento.
        /// Não pode ser nulo ou vazio.
        /// </summary>
        string Lote { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a matrícula do funcionário que solicitou ou iniciou a impressão.
        /// Não pode ser nulo ou vazio.
        /// </summary>
        string MatriculaFuncionario { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o código de barras a ser impresso na etiqueta.
        /// Não pode ser nulo ou vazio.
        /// </summary>
        string CodigoBarras { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a data e hora de início do processo de impressão da etiqueta.
        /// </summary>
        DateTime DataHoraInicio { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a data e hora de término do processo de impressão da etiqueta.
        /// </summary>
        DateTime DataHoraFim { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o status atual da impressão da etiqueta.
        /// 'P' - Pendente, 'C' - Concluída.
        /// </summary>
        char StatusEtiqueta { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a quantidade de Etiquetas que foram solicitadas a serem impressas.
        /// </summary>
        long QuantidadeSolicitada { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define a quantidade de Etiquetas que ainda faltam ser impressas para esta solicitação.
        /// </summary>
        long FaltaImpressao { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o nome do trabalho de impressão associado a esta etiqueta.
        /// </summary>
        string JobName { get; set; }

        /// <summary>
        /// Sobrescreve o método Equals para comparar objetos com base no Id.
        /// </summary>
        /// <param name="obj">O objeto a ser comparado.</param>
        /// <returns>True se os objetos forem iguais; caso contrário, False.</returns>
        bool Equals(object obj);

        /// <summary>
        /// Sobrescreve o método GetHashCode baseado no Id.
        /// </summary>
        /// <returns>O código hash do objeto.</returns>
        int GetHashCode();
    }
}
