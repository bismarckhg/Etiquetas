using Etiqueta.Bibliotecas.TaskCorePipe.Enums;
using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Interfaces
{
    /// <summary>
    /// Define a estrutura para comandos que podem ser enviados através de pipes.
    /// </summary>
    public interface IComandoPipe
    {
        /// <summary>
        /// O tipo de comando a ser executado (ex: STOP, PING).
        /// </summary>
        TipoComando Comando { get; }

        /// <summary>
        /// Parâmetros ou dados necessários para a execução do comando.
        /// </summary>
        object Parametros { get; }
    }
}
