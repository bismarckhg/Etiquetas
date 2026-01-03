using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Etiquetas.Bibliotecas.SATO;

namespace Etiquetas.Domain.Modelo
{
    /// <summary>
    /// Comandos padrão para uma linguagem de impressão.
    /// </summary>
    public class ComandosLinguagem : ComandosPadraoImpressora
    {
        /// <summary>
        /// Campo privado para armazenar o valor do marcador de comando.
        /// </summary>
        [XmlIgnore]
        private string privateMarcadorComando;

        /// <summary>
        /// Campo privado para armazenar o valor do marcador de início de texto.
        /// </summary>
        [XmlIgnore]
        private string privateMarcadorInicioTexto;

        /// <summary>
        /// Campo privado para armazenar o valor do marcador de fim de texto.
        /// </summary>
        [XmlIgnore]
        private string privateMarcadorFimTexto;

        /// <summary>
        /// Campo privado para armazenar o valor da posição do comando 1.
        /// </summary>
        [XmlIgnore]
        private string privatePosicaoComando1;

        /// <summary>
        /// Campo privado para armazenar o valor da posição do comando 2.
        /// </summary>
        [XmlIgnore]
        private string privatePosicaoComando2;

        /// <summary>
        /// Campo privado para armazenar o valor do comando de texto.
        /// </summary>
        [XmlIgnore]
        private string privateComandoTexto;

        /// <summary>
        /// Campo privado para armazenar o valor do comando de barras.
        /// </summary>
        [XmlIgnore]
        private string privateComandoBarras;

        /// <summary>
        /// Campo privado para armazenar o valor do comando de cópias.
        /// </summary>
        [XmlIgnore]
        private string privateComandoCopias;

        /// <summary>
        /// Gets or sets - Enum que indica o tipo de linguagem de impressão.
        /// </summary>
        public EnumTipoLinguagemImpressao TipoLinguagem { get; set; }

        /// <summary>
        /// Gets or sets - O caractere marcador (caractere especial ou não) usado no início do comando.
        /// </summary>
        [XmlElement("MarcadorComando")]
        public string MarcadorComando
        {
            get => MarcadoresComCaracteresEspeciais(privateMarcadorComando);
            set => this.privateMarcadorComando = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando.
        /// </summary>
        [XmlElement("MarcadorInicioTexto")]
        public string MarcadorInicioTexto
        {
            get => MarcadoresComCaracteresEspeciais(privateMarcadorInicioTexto);
            set => this.privateMarcadorInicioTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando.
        /// </summary>
        [XmlElement("MarcadorFimTexto")]
        public string MarcadorFimTexto
        {
            get => MarcadoresComCaracteresEspeciais(privateMarcadorFimTexto);
            set => this.privateMarcadorFimTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - linha e coluna (separada por virgula), ou linha ou coluna utilizada na posicao do campo.
        /// </summary>
        [XmlElement("ComandoPosicao1")]
        public string ComandoPosicao1
        {
            get => MarcadoresComCaracteresEspeciais(privatePosicaoComando1);
            set => this.privatePosicaoComando1 = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - ou linha ou coluna utilizada na posicao do campo.
        /// </summary>
        [XmlElement("ComandoPosicao2")]
        public string ComandoPosicao2
        {
            get => MarcadoresComCaracteresEspeciais(privatePosicaoComando2);
            set => this.privatePosicaoComando2 = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando usado para imprimir texto na etiqueta.
        /// </summary>
        [XmlElement("ComandoTexto")]
        public string ComandoTexto
        {
            get => MarcadoresComCaracteresEspeciais(privateComandoTexto);
            set => this.privateComandoTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando usado para imprimir códigos de barras na etiqueta.
        /// </summary>
        [XmlElement("ComandoBarras")]
        public string ComandoBarras
        {
            get => MarcadoresComCaracteresEspeciais(privateComandoBarras);
            set => this.privateComandoBarras = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando usado para especificar o número de cópias a serem impressas.
        /// </summary>
        [XmlElement("ComandoCopias")]
        public string ComandoCopias
        {
            get => MarcadoresComCaracteresEspeciais(privateComandoCopias);
            set => this.privateComandoCopias = RemoverMarcadoresComCaracteresEspeciais(value);
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
        public ComandosLinguagem(EnumTipoLinguagemImpressao tipoLinguagem)
        {
            switch (tipoLinguagem)
            {
                case EnumTipoLinguagemImpressao.ZPL:
                    InicializarComandosZPL(tipoLinguagem);
                    break;
                case EnumTipoLinguagemImpressao.SBPL:
                    InicializarComandosSBPL(tipoLinguagem);
                    break;
                case EnumTipoLinguagemImpressao.EPL:
                    InicializarComandosEPL(tipoLinguagem);
                    break;
                default:
                    break;
            }
        }

        private void InicializarComandosZPL(EnumTipoLinguagemImpressao tipoLinguagem)
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

        private void InicializarComandosSBPL(EnumTipoLinguagemImpressao tipoLinguagem)
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

        private void InicializarComandosEPL(EnumTipoLinguagemImpressao tipoLinguagem)
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
