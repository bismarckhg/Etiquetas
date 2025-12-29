using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Modelo
{
    public class ComandosEPL
    {

        /// <summary>
        /// Gets or sets - O comando EPL usado para imprimir texto na etiqueta.
        /// </summary>
        public string EPL_ComandoTexto { get; set; }

        /// <summary>
        /// Gets or sets - O comando EPL usado para imprimir códigos de barras na etiqueta.
        /// </summary>
        public string EPL_ComandoBarras { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando EPL.
        /// </summary>
        public string EPL_MarcadorInicioTexto { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando EPL.
        /// </summary>
        public string EPL_MarcadorFimTexto { get; set; }

    }
}
