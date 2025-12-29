using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Modelo
{
    public class ComandosSBPL
    {
        /// <summary>
        /// Gets or sets - O marcador ESC (caractere de escape) usado em comandos SBPL.
        /// </summary>
        public string SBPL_MarcadorESC { get; set; }

        /// <summary>
        /// Gets or sets - O comando SBPL usado para definir a posição horizontal de um elemento na etiqueta.
        /// </summary>
        public string SBPL_ComandoHorizontal { get; set; }

        /// <summary>
        /// Gets or sets - O comando SBPL usado para definir a posição vertical de um elemento na etiqueta.
        /// </summary>
        public string SBPL_ComandoVertical { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando SBPL.
        /// </summary>
        public string SBPL_MarcadorInicioTexto { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando SBPL.
        /// </summary>
        public string SBPL_MarcadorFimTexto { get; set; }
    }
}
