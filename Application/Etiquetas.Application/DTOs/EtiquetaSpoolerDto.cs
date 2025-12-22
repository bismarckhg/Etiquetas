using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Application.DTOs
{
    /// <summary>
    /// DTO para representar uma etiqueta pronta para impressão no spooler.
    /// </summary>
    public class EtiquetaSpoolerDto
    {
        /// <summary>
        /// Codificação utilizada para interpretar o conteúdo da etiqueta.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Conteúdo da impressão da etiqueta em formato binário.
        /// </summary>
        public byte[] ImpressaoBytes { set; get; }

        /// <summary>
        /// Conteúdo da impressão da etiqueta em formato string.
        /// </summary>
        public string ImpressaoString => GetImpressaoString();

        /// <summary>
        /// Nome da impressora onde a etiqueta será enviada para impressão.
        /// </summary>
        public string NomeImpressora { get; set; }

        /// <summary>
        /// Obtém o conteúdo da impressão da etiqueta como uma string, utilizando a codificação especificada.
        /// </summary>
        /// <returns></returns>
        public string GetImpressaoString()
        {
            if (ImpressaoBytes == null || ImpressaoBytes.Length == 0)
            {
                return string.Empty;
            }

            if (Encoding == null)
            {
                Encoding = Encoding.UTF8;
            }

            return Encoding.GetString(ImpressaoBytes);
        }
    }
}
