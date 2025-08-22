using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Enums
{
    /// <summary>
    /// Define os níveis de prioridade para comandos ou mensagens.
    /// </summary>
    public enum NivelPrioridade
    {
        /// <summary>
        /// Prioridade baixa, para operações não urgentes.
        /// </summary>
        Baixa,
        /// <summary>
        /// Prioridade padrão para a maioria das operações.
        /// </summary>
        Normal,
        /// <summary>
        /// Prioridade alta, para operações importantes.
        /// </summary>
        Alta,
        /// <summary>
        /// Prioridade crítica, para operações que devem ser executadas imediatamente.
        /// </summary>
        Critica
    }
}
