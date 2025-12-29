using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Modelo
{
    public class ComandosZPL
    {
        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando ZPL.
        /// </summary>
        public string MarcadorInicioTexto { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando ZPL.
        /// </summary>
        public string MarcadorFimTexto { get; set; }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para definir a posição de um elemento na etiqueta.
        /// </summary>
        public string ComandoPosicao { get; set; }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para especificar o número de cópias a serem impressas.
        /// </summary>
        public string ComandoCopias { get; set; }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para configurar a impressão de códigos de barras.
        /// </summary>
        public string ComandoBarras { get; set; }
    }
}
