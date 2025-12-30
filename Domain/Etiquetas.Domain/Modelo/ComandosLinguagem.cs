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
        private string comandoPosicao1;

        [XmlIgnore]
        private string comandoPosicao2;

        [XmlIgnore]
        private string comandoTexto;

        [XmlIgnore]
        private string comandoBarras;

        [XmlIgnore]
        private string comandoCopias;

        /// <summary>
        /// Gets or sets - O marcador ESC (caractere de escape) usado em comandos SBPL.
        /// </summary>
        [XmlElement("MarcadorESC")]
        public string SBPL_MarcadorESC
        {
            get => MarcadoresComCaracteresEspeciais(marcadorComando);
            set => this.marcadorComando = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        public TipoLinguagemImpressao TipoLinguagem { get; set; }
    }
}
