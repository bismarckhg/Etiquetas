using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TaskCore
{
    public class TaskParametros : TaskReturnValue
    {
        public TaskParametros(
            int quantidadeParametros,
            ITaskParametros parametros,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
            : base(quantidadeParametros, parametros, sourceFilePath)
        {
        }

        public TaskParametros(
            int quantidadeParametros,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
            : base(quantidadeParametros, sourceFilePath, memberName)
        {
        }

        public TaskParametros(
            ITaskParametros parametros,
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
            : base(1, parametros, sourceFilePath, memberName)
        {
        }

        public TaskParametros(
            [CallerFilePath] string sourceFilePath = "",
            [CallerMemberName] string memberName = "")
            : base(1, sourceFilePath, memberName)
        {
        }
    }
}
