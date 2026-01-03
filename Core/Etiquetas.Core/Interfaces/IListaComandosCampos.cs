using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Etiquetas.Domain.Modelo
{
    /// <summary>
    /// Container para lista de comandos de campos.
    /// </summary>
    public interface IListaComandosCampos
    {
        /// <summary>
        /// Gets or sets - Índice de comandos para acesso rápido.
        /// </summary>
        ConcurrentDictionary<string, int> IndiceComandos { get; set; }

        /// <summary>
        /// Gets or sets - Lista de comandos de campos.
        /// </summary>
        List<IComandosCampo> Comandos { get; set; }
    }
}
