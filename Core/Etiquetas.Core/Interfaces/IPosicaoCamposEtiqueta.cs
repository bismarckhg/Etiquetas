using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Bibliotecas.Xml;
using Etiquetas.Domain.Modelo;
using System.Net;
using System.Threading.Tasks;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Interface que define o posicionamento dos campos na etiqueta.
    /// </summary>
    public interface IPosicaoCamposEtiqueta
    {
        /// <summary>
        /// Gets or Sets - the spooler configuration used for extraction operations.
        /// Gets ou sets - A configuração da etiqueta e do spooler usada para extração de dados.
        /// </summary>
        IExtracaoSpooler ConfiguracaoSpooler { get; set; }

        /// <summary>
        /// Gets or sets - Stream XML do arquivo de configuração dos campos da etiqueta.
        /// </summary>
        StreamXml XmlStream { get; set; }

        /// <summary>
        /// Carrega todas as configurações do arquivo appsettings.xml.
        /// </summary>
        /// <returns>Task.</returns>
        Task CarregarConfiguracoes();

        /// <summary>
        /// Obtém o comando do campo pelo nome.
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        Task<IComandosCampo> ObterComandoCampoPeloNome(string nome);
    }
}
