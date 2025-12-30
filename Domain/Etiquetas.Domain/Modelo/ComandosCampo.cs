using Etiquetas.Bibliotecas.SATO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Etiquetas.Domain.Modelo
{
    /// <summary>
    /// Comandos para campo específico na etiqueta.
    /// </summary>
    public class ComandosCampo : IComandosPadraoImpressora
    {

        [XmlIgnore]
        private string comandoEspecifico;

        [XmlIgnore]
        private string posicaoComando1;

        [XmlIgnore]
        private string posicaoComando2;

        /// <summary>
        /// Gets or sets - Nome do campo.
        /// </summary>
        [XmlElement("NomeCampo")]
        public string NomeCampo { get; set; }

        /// <summary>
        /// Gets or sets - O comando usado para definir a posição de um elemento na etiqueta.
        /// </summary>
        [XmlElement("ComandoEspecifico")]
        public string ComandoEspecifico
        {
            get => MarcadoresComCaracteresEspeciais(comandoEspecifico);
            set => this.comandoEspecifico = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - Comando para posição 1 ou única.
        /// </summary>
        [XmlElement("PosicaoComando1")]
        public string PosicaoComando1
        {
            get => MarcadoresComCaracteresEspeciais(posicaoComando1);
            set => this.posicaoComando1 = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - Comando para posição 2 caso a posição 1 não seja única ou suficiente.
        /// </summary>
        [XmlElement("PosicaoComando2")]
        public string PosicaoComando2
        {
            get => MarcadoresComCaracteresEspeciais(posicaoComando2);
            set => this.posicaoComando2 = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets - Indica se o campo é obrigatório ou não.
        /// </summary>
        [XmlElement("Obrigatorio")]
        public bool Obrigatorio { get; set; }
    }
}
