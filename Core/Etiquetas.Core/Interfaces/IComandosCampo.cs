using Etiquetas.Bibliotecas.SATO.Interfaces;

namespace Etiquetas.Domain.Modelo
{
    /// <summary>
    /// Interface que define os comandos específicos para cada campo na etiqueta.
    /// </summary>
    public interface IComandosCampo : IComandosPadraoImpressora
    {
        /// <summary>
        /// Gets or sets - Comando usado para definir o campo específico na etiqueta.
        /// </summary>
        string ComandoEspecifico { get; set; }

        /// <summary>
        /// Gets or sets - Nome do campo.
        /// </summary>
        string NomeCampo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets - Indica se o campo é obrigatório ou não.
        /// </summary>
        bool Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Comando para posição 1 ou única.
        /// </summary>
        string PosicaoComando1 { get; set; }

        /// <summary>
        /// Gets or sets - Comando para posição 2 caso a posição 1 não seja única ou suficiente.
        /// </summary>
        string PosicaoComando2 { get; set; }
    }
}
