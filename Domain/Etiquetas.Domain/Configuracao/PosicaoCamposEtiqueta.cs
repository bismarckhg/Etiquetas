using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Core.Enum;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Domain.Configuracao
{
    /// <summary>
    /// Implementação concreta da configuração de posições de campos em etiquetas,
    /// carregando os valores do arquivo de configuração (appsettings.xml).
    /// </summary>
    public class PosicaoCamposEtiqueta : IPosicaoCamposEtiqueta
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="PosicaoCamposEtiqueta"/>,
        /// carregando todas as configurações do appsettings.xml.
        /// </summary>
        /// <param name="tipoLinguagem">Tipo de linguagem de impressão a ser utilizada</param>
        public PosicaoCamposEtiqueta(TipoLinguagemImpressao tipoLinguagem)
        {
            TipoLinguagem = tipoLinguagem;
            CarregarConfiguracoes();
        }

        /// <inheritdoc/>
        public TipoLinguagemImpressao TipoLinguagem { get; private set; }

        /// <inheritdoc/>
        public string MarcadorInicialTexto { get; private set; }

        /// <inheritdoc/>
        public string MarcadorFinalTexto { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo CodigoMaterial { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo DescricaoMedicamento { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo DescricaoMedicamento2 { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo PrincipioAtivo1 { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo PrincipioAtivo2 { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo Embalagem { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo Lote { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo Validade { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo CodigoUsuario { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo CodigoBarras { get; private set; }

        /// <inheritdoc/>
        public IConfiguracaoCampo Copias { get; private set; }

        /// <summary>
        /// Carrega todas as configurações do arquivo appsettings.xml.
        /// </summary>
        private void CarregarConfiguracoes()
        {
            // Carrega marcadores de texto baseado na linguagem
            switch (TipoLinguagem)
            {
                case TipoLinguagemImpressao.ZPL:
                    MarcadorInicialTexto = ObterConfiguracao("ZPL_MarcadorInicioTexto", "^FD");
                    MarcadorFinalTexto = ObterConfiguracao("ZPL_MarcadorFimTexto", "^FS");
                    break;

                case TipoLinguagemImpressao.SBPL:
                    MarcadorInicialTexto = ObterConfiguracao("SBPL_MarcadorInicioTexto", "");
                    MarcadorFinalTexto = ObterConfiguracao("SBPL_MarcadorFimTexto", "");
                    break;

                case TipoLinguagemImpressao.EPL:
                    MarcadorInicialTexto = ObterConfiguracao("EPL_MarcadorInicioTexto", "\"");
                    MarcadorFinalTexto = ObterConfiguracao("EPL_MarcadorFimTexto", "\"");
                    break;
            }

            // Carrega configurações de cada campo
            CodigoMaterial = CarregarCampo("CodigoMaterial");
            DescricaoMedicamento = CarregarCampo("DescricaoMedicamento");
            DescricaoMedicamento2 = CarregarCampo("DescricaoMedicamento2");
            PrincipioAtivo1 = CarregarCampo("PrincipioAtivo1");
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
            return new ConfiguracaoCampo
            {
                Comando1 = ObterConfiguracao($"Campo_{nomeCampo}_Cmd1", ""),
                Comando2 = ObterConfiguracao($"Campo_{nomeCampo}_Cmd2", ""),
                Obrigatorio = ObterConfiguracaoBoolean($"Campo_{nomeCampo}_Obrigatorio", false)
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
        /// <param name="valorPadrao">Valor padrão se a chave não existir</param>
        /// <returns>Valor da configuração ou valor padrão</returns>
        private bool ObterConfiguracaoBoolean(string chave, bool valorPadrao)
        {
            var valor = ConfigurationManager.AppSettings[chave];
            if (string.IsNullOrWhiteSpace(valor))
                return valorPadrao;

            bool resultado;
            return bool.TryParse(valor, out resultado) ? resultado : valorPadrao;
        }
    }
}
