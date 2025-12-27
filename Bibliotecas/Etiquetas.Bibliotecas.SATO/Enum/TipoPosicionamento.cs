using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Enumeração que define o tipo de posicionamento dos campos na etiqueta.
    /// </summary>
    public enum TipoPosicionamento
    {
        /// <summary>Posição definida em um único comando (ex: ZPL ^FO10,20)</summary>
        ComandoUnico,
        /// <summary>Posição dividida em dois comandos: Horizontal primeiro, Vertical depois</summary>
        HorizontalVertical,
        /// <summary>Posição dividida em dois comandos: Vertical primeiro, Horizontal depois</summary>
        VerticalHorizontal
    }
}
