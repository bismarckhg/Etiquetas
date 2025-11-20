using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Evento
{
    /// <summary>
    /// Classe de Eventos de Log de Erros
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Evento abstrato que deve ser implementado pelas classes derivadas para notificar quando um erro ocorre.
        /// </summary>
        //public abstract event EventHandler<ErrorEventArgs> ErrorOccurred;

        /// <summary>
        /// Implementacao do método OnErrorOccurred. As classes derivadas devem sobrescrevê-lo.
        /// </summary>
        //public abstract void OnErrorOccurred(Exception exception);

        public Exception Exception { get; }
        public DateTime Timestamp { get; }
        public string Message => Exception?.Message;

        public ErrorEventArgs(Exception exception)
        {
            Exception = exception;
            Timestamp = DateTime.Now;
        }
    }
}
