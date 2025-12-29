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
    public class ComandosCampo
    {
        /// <summary>
        /// Gets or sets - Nome do campo.
        /// </summary>
        [XmlElement("NomeCampo")]
        public string NomeCampo { get; set; }

        /// <summary>
        /// Gets or sets - Comando para posição 1 ou única.
        /// </summary>
        [XmlElement("ComandoPosicao1")]
        public string ComandoPosicao1 { get; set; }

        /// <summary>
        /// Gets or sets - Comando para posição 2 caso a posição 1 não seja única ou suficiente.
        /// </summary>
        [XmlElement("ComandoPosicao2")]
        public string ComandoPosicao2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets - Indica se o campo é obrigatório ou não.
        /// </summary>
        [XmlElement("Obrigatorio")]
        public bool Obrigatorio { get; set; }
    }
}
