using Etiquetas.Core.Enum;
using Etiquetas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Configuracao
{
    /// <summary>
    /// Representa a configuração completa de um campo específico na etiqueta.
    /// </summary>
    public class ConfiguracaoCampo : IConfiguracaoCampo
    {
        /// <inheritdoc/>
        public string Comando1 { get; set; }

        /// <inheritdoc/>
        public string Comando2 { get; set; }

        /// <inheritdoc/>
        public bool Obrigatorio { get; set; }

        /// <inheritdoc/>
        public TipoPosicionamento TipoPosicionamento
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Comando2))
                    return TipoPosicionamento.ComandoUnico;

                // Detecta se é H/V ou V/H baseado nos comandos
                if (Comando1.Contains("H") || Comando1.Contains("h"))
                    return TipoPosicionamento.HorizontalVertical;

                if (Comando1.Contains("V") || Comando1.Contains("v"))
                    return TipoPosicionamento.VerticalHorizontal;

                return TipoPosicionamento.ComandoUnico;
            }
        }
    }
}
