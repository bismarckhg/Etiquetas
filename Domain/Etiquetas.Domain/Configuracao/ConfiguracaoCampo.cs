using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Core.Interfaces;

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
        public EnumTipoPosicionamento TipoPosicionamento
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Comando2))
                {
                    return EnumTipoPosicionamento.ComandoUnico;
                }

                // Detecta se é H/V ou V/H baseado nos comandos
                if (Comando1.Contains("H") || Comando1.Contains("h"))
                {
                    return EnumTipoPosicionamento.HorizontalVertical;
                }

                if (Comando1.Contains("V") || Comando1.Contains("v"))
                {
                    return EnumTipoPosicionamento.VerticalHorizontal;
                }

                return EnumTipoPosicionamento.ComandoUnico;
            }
        }
    }
}
