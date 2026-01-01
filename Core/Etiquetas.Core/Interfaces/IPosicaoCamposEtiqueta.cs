using System.Net;
using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Domain.Modelo;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Interface que define o posicionamento dos campos na etiqueta.
    /// </summary>
    public interface IPosicaoCamposEtiqueta
    {
        /// <summary>
        /// Gets or sets the spooler configuration used for extraction operations.
        /// Gets ou sets - A configuração da etiqueta e do spooler usada para extração de dados.
        /// </summary>
        IExtracaoSpooler ConfiguracaoSpooler { get; set; }
    }
}
