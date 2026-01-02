using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Bibliotecas.SATO.Interfaces;

namespace Etiquetas.Domain.Modelo
{
    public interface IComandosLinguagem : IComandosPadraoImpressora
    {
        /// <summary>
        /// Gets or sets - O comando usado para imprimir códigos de barras.
        /// </summary>
        string ComandoBarras { get; set; }

        /// <summary>
        /// Gets or sets - O comando usado para definir o número de cópias a serem impressas.
        /// </summary>
        string ComandoCopias { get; set; }

        /// <summary>
        /// Gets or sets - Comando para posição 1 ou única.
        /// </summary>
        string ComandoPosicao1 { get; set; }

        /// <summary>
        /// Gets or sets - Comando para posição 2 caso a posição 1 não seja única ou suficiente.
        /// </summary>
        string ComandoPosicao2 { get; set; }

        /// <summary>
        /// Gets or sets - O comando usado para imprimir texto na etiqueta.
        /// </summary>
        string ComandoTexto { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando.
        /// </summary>
        string MarcadorComando { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando.
        /// </summary>
        string MarcadorFimTexto { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando.
        /// </summary>
        string MarcadorInicioTexto { get; set; }

        /// <summary>
        /// Gets or sets - Tipo de linguagem de impressão.
        /// </summary>
        EnumTipoLinguagemImpressao TipoLinguagem { get; set; }
    }
}
