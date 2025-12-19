using System;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Representa uma solicitação de Impressão de uma copia da etiqueta e o seu status.
    /// </summary>
    public interface IFaltaImprimir : IContratoEntidade
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
        DateTime DataImpressao { get; set; }

        /// <summary>
        /// Gets or sets - obtém ou define o status da impressão da Etiqueta.
        /// </summary>
        byte StatusImpressora { get; set; }

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
