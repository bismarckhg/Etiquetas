using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TaskCore
{
    /// <summary>
    /// Fornece informações do hardware de execução.
    /// </summary>
    public static class HardwareInfo
    {
        /// <summary>Número de processadores lógicos detectados.</summary>
        public static int ProcessorCount => Environment.ProcessorCount;
    }
}
