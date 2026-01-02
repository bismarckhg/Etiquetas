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
    /// Comandos para campo específico na etiqueta.
    /// </summary>
    public class ComandosCampo : ComandosPadraoImpressora, IComandosCampo
    {
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
            get => MarcadoresComCaracteresEspeciais(ProtectedComandoEspecifico);
            set => this.ProtectedComandoEspecifico = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - Comando para posição 1 ou única.
        /// </summary>
        [XmlElement("PosicaoComando1")]
        public string PosicaoComando1
        {
            get => MarcadoresComCaracteresEspeciais(ProtectedPosicaoComando1);
            set => this.ProtectedPosicaoComando1 = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets - Comando para posição 2 caso a posição 1 não seja única ou suficiente.
        /// </summary>
        [XmlElement("PosicaoComando2")]
        public string PosicaoComando2
        {
            get => MarcadoresComCaracteresEspeciais(ProtectedPosicaoComando2);
            set => this.ProtectedPosicaoComando2 = RemoverMarcadoresComCaracteresEspeciais(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets - Indica se o campo é obrigatório ou não.
        /// </summary>
        [XmlElement("Obrigatorio")]
        public bool Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Comando usado para definir o campo específico na etiqueta.
        /// </summary>
        [XmlIgnore]
        protected string ProtectedComandoEspecifico { get; set; }

        /// <summary>
        /// Gets or sets - Comando para posição 1 ou única.
        /// </summary>
        [XmlIgnore]
        protected string ProtectedPosicaoComando1 { get; set; }

        /// <summary>
        /// Gets or sets - Comando para posição 2 caso a posição 1 não seja única ou suficiente.
        /// </summary>
        [XmlIgnore]
        protected string ProtectedPosicaoComando2 { get; set; }
    }
}
