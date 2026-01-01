using Etiquetas.Bibliotecas.SATO;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Interface que define a configuração de um campo específico na etiqueta.
    /// </summary>
    public interface IConfiguracaoCampo
    {
        /// <summary>
        /// Obtém ou define o primeiro comando de posicionamento do campo.
        /// Pode conter a posição completa (comando único) ou apenas uma coordenada (H ou V).
        /// </summary>
        string Comando1 { get; set; }

        /// <summary>
        /// Obtém ou define o segundo comando de posicionamento do campo.
        /// Usado quando o posicionamento é dividido em dois comandos (H/V ou V/H).
        /// Deixe vazio para comando único.
        /// </summary>
        string Comando2 { get; set; }

        /// <summary>
        /// Obtém ou define se o campo é obrigatório.
        /// Se true, gerará erro de validação quando o campo estiver vazio.
        /// </summary>
        bool Obrigatorio { get; set; }

        /// <summary>
        /// Obtém ou define o tipo de posicionamento utilizado por este campo.
        /// Calculado automaticamente baseado em Comando1 e Comando2.
        /// </summary>
        EnumTipoPosicionamento TipoPosicionamento { get; }
    }
}
