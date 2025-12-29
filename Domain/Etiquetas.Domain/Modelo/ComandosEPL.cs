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
    /// Comandos EPL (Eltron Programming Language).
    /// </summary>
    [Serializable]
    public class ComandosEPL : IComandosPadraoImpressora
    {
        [XmlIgnore]
        private string comandoTexto;

        [XmlIgnore]
        private string comandoBarras;

        [XmlIgnore]
        private string marcadorInicioTexto;

        [XmlIgnore]
        private string marcadorFimTexto;

        /// <summary>
        /// Gets or sets - O comando EPL usado para imprimir texto na etiqueta.
        /// </summary>
        [XmlElement("ComandoTexto")]
        public string EPL_ComandoTexto
        {
            get => MarcadoresComCaracteresEspeciais(comandoTexto);
            set => this.comandoTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O comando EPL usado para imprimir códigos de barras na etiqueta.
        /// </summary>
        [XmlElement("ComandoBarras")]
        public string EPL_ComandoBarras
        {
            get => MarcadoresComCaracteresEspeciais(comandoBarras);
            set => this.comandoBarras = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando EPL.
        /// </summary>
        [XmlElement("MarcadorInicioTexto")]
        public string EPL_MarcadorInicioTexto
        {
            get => MarcadoresComCaracteresEspeciais(marcadorInicioTexto);
            set => this.marcadorInicioTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando EPL.
        /// </summary>
        [XmlElement("MarcadorFimTexto")]
        public string EPL_MarcadorFimTexto
        {
            get => MarcadoresComCaracteresEspeciais(marcadorFimTexto);
            set => this.marcadorFimTexto = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComandosEPL"/> class.
        /// Inicializa uma nova instância da classe <see cref="ComandosEPL"/>.
        /// </summary>
        public ComandosEPL()
        {
            EPL_ComandoTexto = "A";
            EPL_ComandoBarras = "B";
            EPL_MarcadorInicioTexto = "\"";
            EPL_MarcadorFimTexto = "\"";
        }
    }
}
