using Etiquetas.Bibliotecas.SATO;

namespace Etiquetas.Domain.Modelo
{
    /// <summary>
    /// Classe principal para extração de dados do spooler de impressão.
    /// </summary>
    public interface IExtracaoSpooler
    {
        /// <summary>
        /// Gets or Sets - Comandos padrão para uma linguagem de impressão.
        /// </summary>
        IComandosLinguagem ComandosImpressao { get; set; }

        /// <summary>
        /// Gets or Sets - Container para lista de comandos de campos.
        /// </summary>
        IListaComandosCampos Campos { get; set; }
    }
}
