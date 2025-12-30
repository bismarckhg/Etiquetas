using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Etiquetas.Bibliotecas.SATO;

namespace Etiquetas.Domain.Modelo
{
    public class ComandosLinguagem : IComandosPadraoImpressora
    {

        [XmlIgnore]
        private string marcadorComando;

        [XmlIgnore]
        private string marcadorInicioTexto;

        [XmlIgnore]
        private string marcadorFimTexto;

        [XmlIgnore]
        private string posicaoComando1;

        [XmlIgnore]
        private string posicaoComando2;

        [XmlIgnore]
        private string comandoTexto;

        [XmlIgnore]
        private string comandoBarras;

        [XmlIgnore]
        private string comandoCopias;

        /// <summary>
        /// Gets or sets - Enum que indica o tipo de linguagem de impressão.
        /// </summary>
        public TipoLinguagemImpressao TipoLinguagem { get; set; }

        /// <summary>
        /// Gets or sets - O caractere marcador (caractere especial ou não) usado no início do comando.
        /// </summary>
        [XmlElement("MarcadorComando")]
        public string MarcadorComando
        {
            get => MarcadoresComCaracteresEspeciais(marcadorComando);
            set => this.marcadorComando = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando.
        /// </summary>
        [XmlElement("MarcadorInicioTexto")]
        public string MarcadorInicioTexto
        {
            get => MarcadoresComCaracteresEspeciais(marcadorInicioTexto);
            set => this.marcadorInicioTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando.
        /// </summary>
        [XmlElement("MarcadorFimTexto")]
        public string MarcadorFimTexto
        {
            get => MarcadoresComCaracteresEspeciais(marcadorFimTexto);
            set => this.marcadorFimTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - linha e coluna (separada por virgula), ou linha ou coluna utilizada na posicao do campo.
        /// </summary>
        [XmlElement("ComandoPosicao1")]
        public string ComandoPosicao1
        {
            get => MarcadoresComCaracteresEspeciais(posicaoComando1);
            set => this.posicaoComando1 = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - ou linha ou coluna utilizada na posicao do campo.
        /// </summary>
        [XmlElement("ComandoPosicao2")]
        public string ComandoPosicao2
        {
            get => MarcadoresComCaracteresEspeciais(posicaoComando2);
            set => this.posicaoComando2 = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando usado para imprimir texto na etiqueta.
        /// </summary>
        [XmlElement("ComandoTexto")]
        public string ComandoTexto
        {
            get => MarcadoresComCaracteresEspeciais(comandoTexto);
            set => this.comandoTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando usado para imprimir códigos de barras na etiqueta.
        /// </summary>
        [XmlElement("ComandoBarras")]
        public string ComandoBarras
        {
            get => MarcadoresComCaracteresEspeciais(comandoBarras);
            set => this.comandoBarras = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando usado para especificar o número de cópias a serem impressas.
        /// </summary>
        [XmlElement("ComandoCopias")]
        public string ComandoCopias
        {
            get => MarcadoresComCaracteresEspeciais(comandoCopias);
            set => this.comandoCopias = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComandosLinguagem"/> class.
        /// Inicializa uma nova instância da classe <see cref="ComandosLinguagem"/>.
        /// </summary>
        public ComandosLinguagem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComandosLinguagem"/> class.
        /// Inicializa uma nova instância da classe <see cref="ComandosLinguagem"/>.
        /// </summary>
        /// <param name="tipoLinguagem">Define o tipo de linguagem da impressora.</param>
        public ComandosLinguagem(TipoLinguagemImpressao tipoLinguagem)
        {
            switch (tipoLinguagem)
            {
                case TipoLinguagemImpressao.ZPL:
                    InicializarComandosZPL(tipoLinguagem);
                    break;
                case TipoLinguagemImpressao.SBPL:
                    InicializarComandosSBPL(tipoLinguagem);
                    break;
                case TipoLinguagemImpressao.EPL:
                    InicializarComandosEPL(tipoLinguagem);
                    break;
                default:
                    break;
            }
        }

        private void InicializarComandosZPL(TipoLinguagemImpressao tipoLinguagem)
        {
            this.TipoLinguagem = tipoLinguagem;
            this.MarcadorComando = "^";
            this.MarcadorInicioTexto = "FD";
            this.MarcadorFimTexto = "FS";
            this.ComandoPosicao1 = "FO"; // Field Origin
            this.ComandoPosicao2 = string.Empty; // Field Data
            this.ComandoTexto = "A";
            this.ComandoBarras = "B";
            this.ComandoCopias = "PQ";
        }

        private void InicializarComandosSBPL(TipoLinguagemImpressao tipoLinguagem)
        {
            this.TipoLinguagem = tipoLinguagem;
            this.MarcadorComando = "<ESC>"; // Chr(27)
            this.ComandoPosicao1 = "H";
            this.ComandoPosicao2 = "V";
            this.MarcadorInicioTexto = string.Empty;
            this.MarcadorFimTexto = string.Empty;
            this.ComandoTexto = string.Empty;
            this.ComandoBarras = string.Empty;
            this.ComandoCopias = string.Empty;
        }

        private void InicializarComandosEPL(TipoLinguagemImpressao tipoLinguagem)
        {
            this.TipoLinguagem = tipoLinguagem;
            this.MarcadorComando = string.Empty;
            this.MarcadorInicioTexto = "\"";
            this.MarcadorFimTexto = "\"";
            this.ComandoPosicao1 = string.Empty;
            this.ComandoPosicao2 = string.Empty;
            this.ComandoTexto = "A";
            this.ComandoBarras = "B";
            this.ComandoCopias = string.Empty;
        }
    }
}
