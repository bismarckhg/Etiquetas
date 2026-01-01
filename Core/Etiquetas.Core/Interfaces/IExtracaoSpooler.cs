using Etiquetas.Bibliotecas.SATO;

namespace Etiquetas.Domain.Modelo
{
    /// <summary>
    /// Classe principal para extração de dados do spooler de impressão.
    /// </summary>
    public interface IExtracaoSpooler
    {

        /// <summary>
        /// Comandos padrão para uma linguagem de impressão.
        /// </summary>
        IComandosPadraoImpressora ComandosImpressao { get; set; }

        /// <summary>
        /// Get e set - Container para lista de comandos de campos.
        /// </summary>
        IListaComandosCampos Campos { get; set; }
    }
}
