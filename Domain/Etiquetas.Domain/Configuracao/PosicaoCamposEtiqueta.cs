using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Configuracao
{
    /// <summary>
    /// Implementação concreta da configuração de posições de campos em etiquetas,
    /// carregando os valores do arquivo de configuração (appsettings.xml).
    /// </summary>
    public class PosicaoCamposEtiqueta : IPosicaoCamposEtiqueta
    {
        /// <inehritdoc/>
        public IExtracaoSpooler ConfiguracaoSpooler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PosicaoCamposEtiqueta"/> class.
        /// Inicializa uma nova instância da classe <see cref="PosicaoCamposEtiqueta"/>,
        /// carregando todas as configurações do appsettings.xml.
        /// </summary>
        /// <param name="tipoLinguagem">Tipo de linguagem de impressão a ser utilizada</param>
        public PosicaoCamposEtiqueta(EnumTipoLinguagemImpressao tipoLinguagem)
        {
            this.ConfiguracaoSpooler = new ExtracaoSpooler(tipoLinguagem);
            CarregarConfiguracoes();
        }

        /// <summary>
        /// Carrega todas as configurações do arquivo appsettings.xml.
        /// </summary>
        private void CarregarConfiguracoes()
        {

            // Deserializar de arquivo
            var configCarregada = await XmlStream.LerAsync<ExtracaoSpooler>(parametrosLeitura).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"\nCarregados {configCarregada.Campos.Comandos.Count} campos do arquivo.");

            // Carrega marcadores de texto baseado na linguagem
            switch (this.ConfiguracaoSpooler.ComandosImpressao.TipoLinguagem)
            {
                case EnumTipoLinguagemImpressao.ZPL:
                    this.ConfiguracaoSpooler.ComandosImpressao.MarcadorInicioTexto = ObterConfiguracao("ZPL_MarcadorInicioTexto", "^FD");
                    this.ConfiguracaoSpooler.ComandosImpressao.MarcadorFimTexto = ObterConfiguracao("ZPL_MarcadorFimTexto", "^FS");
                    break;

                case EnumTipoLinguagemImpressao.SBPL:
                    this.ConfiguracaoSpooler.ComandosImpressao.MarcadorInicioTexto = ObterConfiguracao("SBPL_MarcadorInicioTexto", "");
                    this.ConfiguracaoSpooler.ComandosImpressao.MarcadorFimTexto = ObterConfiguracao("SBPL_MarcadorFimTexto", "");
                    break;

                case EnumTipoLinguagemImpressao.EPL:
                    this.ConfiguracaoSpooler.ComandosImpressao.MarcadorInicioTexto = ObterConfiguracao("EPL_MarcadorInicioTexto", "\"");
                    this.ConfiguracaoSpooler.ComandosImpressao.MarcadorFimTexto = ObterConfiguracao("EPL_MarcadorFimTexto", "\"");
                    break;
            }

            // Carrega configurações de cada campo

            CodigoMaterial = CarregarCampo("CodigoMaterial");
            DescricaoMedicamento = CarregarCampo("DescricaoMedicamento");
            DescricaoMedicamento2 = CarregarCampo("DescricaoMedicamento2");
            PrincipioAtivo = CarregarCampo("PrincipioAtivo");
            PrincipioAtivo2 = CarregarCampo("PrincipioAtivo2");
            Embalagem = CarregarCampo("Embalagem");
            Lote = CarregarCampo("Lote");
            Validade = CarregarCampo("Validade");
            CodigoUsuario = CarregarCampo("CodigoUsuario");
            CodigoBarras = CarregarCampo("CodigoBarras");
            Copias = CarregarCampo("Copias");
        }

        /// <summary>
        /// Carrega a configuração de um campo específico do appsettings.xml.
        /// </summary>
        /// <param name="nomeCampo">Nome do campo a ser carregado</param>
        /// <returns>Objeto ConfiguracaoCampo com os dados carregados</returns>
        private ConfiguracaoCampo CarregarCampo(string nomeCampo)
        {
            var cmd1 = $"Campo_{nomeCampo}_Cmd1";
            var cmd2 = $"Campo_{nomeCampo}_Cmd2";
            var comando1 = ObterConfiguracao(cmd1, null);
            var comando2 = ObterConfiguracao(cmd2, null);
            var obrigatorio = ObterConfiguracaoBoolean($"Campo_{nomeCampo}_Obrigatorio", false);
            return new ConfiguracaoCampo
            {
                Comando1 = comando1,
                Comando2 = comando2,
                Obrigatorio = obrigatorio,
            };
        }

        /// <summary>
        /// Obtém uma configuração string do appsettings.xml.
        /// </summary>
        /// <param name="chave">Chave da configuração</param>
        /// <param name="valorPadrao">Valor padrão se a chave não existir</param>
        /// <returns>Valor da configuração ou valor padrão</returns>
        private string ObterConfiguracao(string chave, string valorPadrao)
        {
            var valor = ConfigurationManager.AppSettings[chave];
            return string.IsNullOrWhiteSpace(valor) ? valorPadrao : valor;
        }

        /// <summary>
        /// Obtém uma configuração booleana do appsettings.xml.
        /// </summary>
        /// <param name="chave">Chave da configuração</param>
        /// <param name="valorPadrao">Valor padrão se a chave não existir.</param>
        /// <returns>Valor da configuração ou valor padrão</returns>
        private bool ObterConfiguracaoBoolean(string chave, bool valorPadrao)
        {
            var valor = ConfigurationManager.AppSettings[chave];
            if (string.IsNullOrWhiteSpace(valor))
            {
                return valorPadrao;
            }

            bool resultado;
            return bool.TryParse(valor, out resultado) ? resultado : valorPadrao;
        }
    }
}
