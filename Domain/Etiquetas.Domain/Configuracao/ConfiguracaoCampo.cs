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
    public class ConfiguracaoCampo
    {
        /// <summary>
        /// Obtém ou define o primeiro comando de posicionamento do campo.
        /// Pode conter a posição completa (comando único) ou apenas uma coordenada (H ou V).
        /// </summary>
        public string Comando1 { get; set; }

        /// <summary>
        /// Obtém ou define o segundo comando de posicionamento do campo.
        /// Usado quando o posicionamento é dividido em dois comandos (H/V ou V/H).
        /// Deixe vazio para comando único.
        /// </summary>
        public string Comando2 { get; set; }

        /// <summary>
        /// Obtém ou define se o campo é obrigatório.
        /// Se true, gerará erro de validação quando o campo estiver vazio.
        /// </summary>
        public bool Obrigatorio { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de posicionamento utilizado por este campo.
        /// Calculado automaticamente baseado em Comando1 e Comando2.
        /// </summary>
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
