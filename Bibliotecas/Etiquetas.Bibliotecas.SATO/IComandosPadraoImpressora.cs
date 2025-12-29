using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Interface para comandos padrão de linguagem Codigo Barras da impressora.
    /// </summary>
    public abstract class IComandosPadraoImpressora
    {
        /// <summary>
        /// Substitui sequências de caracteres especiais por seus caracteres correspondentes.
        /// </summary>
        /// <param name="texto">string contento texto com caracteres na forma <KEY> ou [KEY].</param>
        /// <returns>string com texto ja com carcacteres especiais correspondentes.</returns>
        public virtual string MarcadoresComCaracteresEspeciais(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }

            var bibliotecaSATO = Etiquetas.Bibliotecas.SATO.ControlCharListSATO.CriaDicionarioOpcao();

            var resultado = Etiquetas.Bibliotecas.SATO.ControlCharReplace.Execute(texto, bibliotecaSATO);

            return resultado;
        }

        /// <summary>
        /// Remove sequências de caracteres especiais por seus textos na forma <KEY> ou [KEY] correspondentes ao caractere.
        /// </summary>
        /// <param name="texto">string a ser convertido.</param>
        /// <returns>string convertido.</returns>
        public virtual string RemoverMarcadoresComCaracteresEspeciais(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }

            var bibliotecaSATO = Etiquetas.Bibliotecas.SATO.ControlCharListSATO.CriaDicionarioOpcao();
            var resultado = Etiquetas.Bibliotecas.SATO.ControlCharConvert.Execute(texto, bibliotecaSATO);
            return resultado;
        }
    }
}
