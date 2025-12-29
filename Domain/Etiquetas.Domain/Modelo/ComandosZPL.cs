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
    /// Comandos ZPL (Zebra Programming Language).
    /// </summary>
    [Serializable]
    public class ComandosZPL : IComandosPadraoImpressora
    {
        [XmlIgnore]
        private string marcadorInicioTexto;

        [XmlIgnore]
        private string marcadorFimTexto;

        [XmlIgnore]
        private string comandoPosicao;

        [XmlIgnore]
        private string comandoCopias;

        [XmlIgnore]
        private string comandoBarras;

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando ZPL.
        /// </summary>
        [XmlElement("MarcadorInicioTexto")]
        public string MarcadorInicioTexto
        {
            get => MarcadoresComCaracteresEspeciais(marcadorInicioTexto);
            set => this.marcadorInicioTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando ZPL.
        /// </summary>
        [XmlElement("MarcadorFimTexto")]
        public string MarcadorFimTexto
        {
            get => MarcadoresComCaracteresEspeciais(marcadorFimTexto);
            set => this.marcadorFimTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para definir a posição de um elemento na etiqueta.
        /// </summary>
        [XmlElement("ComandoPosicao")]
        public string ComandoPosicao
        {
            get => MarcadoresComCaracteresEspeciais(comandoPosicao);
            set => this.comandoPosicao = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para especificar o número de cópias a serem impressas.
        /// </summary>
        [XmlElement("ComandoCopias")]
        public string ComandoCopias
        {
            get => MarcadoresComCaracteresEspeciais(comandoCopias);
            set => this.comandoCopias = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para configurar a impressão de códigos de barras.
        /// </summary>
        [XmlElement("ComandoBarras")]
        public string ComandoBarras
        {
            get => MarcadoresComCaracteresEspeciais(comandoBarras);
            set => this.comandoBarras = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComandosZPL"/> class.
        /// Inicializa uma nova instância da classe <see cref="ComandosZPL"/>.
        /// </summary>
        public ComandosZPL()
        {
            MarcadorInicioTexto = "^FD";
            MarcadorFimTexto = "^FS";
            ComandoPosicao = "^FO";
            ComandoCopias = "^PQ";
            ComandoBarras = "^BC";
        }
    }
}
