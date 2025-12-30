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
    /// Comandos SBPL (SATO Barcode Programming Language).
    /// </summary>
    [Serializable]
    public class ComandosSBPL : IComandosPadraoImpressora
    {
        [XmlIgnore]
        private string marcadorComando;

        [XmlIgnore]
        private string comandoHorizontal;

        [XmlIgnore]
        private string comandoVertical;

        [XmlIgnore]
        private string marcadorInicioTexto;

        [XmlIgnore]
        private string marcadorFimTexto;

        /// <summary>
        /// Gets or sets - O marcador ESC (caractere de escape) usado em comandos SBPL.
        /// </summary>
        [XmlElement("MarcadorESC")]
        public string MarcadorComando
        {
            get => MarcadoresComCaracteresEspeciais(marcadorComando);
            set => this.marcadorComando = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando SBPL usado para definir a posição horizontal de um elemento na etiqueta.
        /// </summary>
        [XmlElement("ComandoHorizontal")]
        public string ComandoHorizontal
        {
            get => MarcadoresComCaracteresEspeciais(comandoHorizontal);
            set => this.comandoHorizontal = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando SBPL usado para definir a posição vertical de um elemento na etiqueta.
        /// </summary>
        [XmlElement("ComandoVertical")]
        public string SBPL_ComandoVertical
        {
            get => MarcadoresComCaracteresEspeciais(comandoVertical);
            set => this.comandoVertical = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando SBPL.
        /// </summary>
        [XmlElement("MarcadorInicioTexto")]
        public string SBPL_MarcadorInicioTexto
        {
            get => MarcadoresComCaracteresEspeciais(marcadorInicioTexto);
            set => this.marcadorInicioTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando SBPL.
        /// </summary>
        [XmlElement("MarcadorFimTexto")]
        public string SBPL_MarcadorFimTexto
        {
            get => MarcadoresComCaracteresEspeciais(marcadorFimTexto);
            set => this.marcadorFimTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComandosSBPL"/> class.
        /// Inicializa uma nova instância da classe <see cref="ComandosSBPL"/>.
        /// </summary>
        public ComandosSBPL()
        {
            SBPL_MarcadorESC = "<ESC>"; // Chr(27)
            SBPL_ComandoHorizontal = "H";
            SBPL_ComandoVertical = "V";
            SBPL_MarcadorInicioTexto = string.Empty;
            SBPL_MarcadorFimTexto = string.Empty;
        }
    }
}
