using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Etiquetas.Bibliotecas.SATO.Interfaces
{
    /// <summary>
    /// Interface para comandos padrão de linguagem Codigo Barras da impressora.
    /// </summary>
    public interface IComandosPadraoImpressora
    {
        /// <summary>
        /// Substitui sequências de caracteres especiais por seus caracteres correspondentes.
        /// </summary>
        /// <returns>string com texto ja com carcacteres especiais correspondentes.</returns>
        /// <param name="texto">string contento texto com caracteres na forma <KEY> ou [KEY].</param>
        string MarcadoresComCaracteresEspeciais(string texto);

        /// <summary>
        /// Remove sequências de caracteres especiais por seus textos na forma KEY ou [KEY] correspondentes ao caractere.
        /// </summary>
        /// <returns>string convertido.</returns>
        /// <param name="texto">string a ser convertido.</param>
        string RemoverMarcadoresComCaracteresEspeciais(string texto);
    }
}
