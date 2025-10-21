using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TaskCore
{
    public enum TaskState
    {
        AguardandoInicio = 0,
        EmProcessamento,
        Finalizada,
        Cancelada,
        ParadaBrusca,
        Timeout,
        ComErro
    }
}
