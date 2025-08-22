using Etiqueta.Bibliotecas.TaskCorePipe.Enums;
using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Modelos
{
    /// <summary>
    /// Representa um comando específico a ser executado, enviado como payload de uma MensagemPipe.
    /// </summary>
    public class ComandoPipe : Interfaces.IComandoPipe
    {
        /// <summary>
        /// O tipo de comando a ser executado.
        /// </summary>
        public TipoComando Comando { get; set; }

        /// <summary>
        /// Parâmetros específicos para o comando. Pode ser um objeto, um dicionário, etc.
        /// </summary>
        public object Parametros { get; set; }
    }
}
