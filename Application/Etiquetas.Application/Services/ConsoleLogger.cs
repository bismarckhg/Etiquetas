using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Application.Services
{
    /// <summary>
    /// Logger simples para console.
    /// </summary>
    public class ConsoleLogger
    {
        /// <summary>
        /// Informa uma mensagem de informação.
        /// </summary>
        /// <param name="message">string com mensage de informação.</param>
        public void Info(string message)
        {
            Console.WriteLine("[I] " + message);
        }

        /// <summary>
        /// Informa uma mensagem de aviso.
        /// </summary>
        /// <param name="message">string com mensagem de Aviso.</param>
        public void Warn(string message)
        {
            Console.WriteLine("[W] " + message);
        }

        /// <summary>
        /// Informa uma mensagem de erro.
        /// </summary>
        /// <param name="message">string com mensagem de erro.</param>
        /// <param name="ex">exception do erro.</param>
        public void Error(string message, Exception ex = null)
        {
            Console.WriteLine("[E] " + message + (ex != null ? " -> " + ex.Message : string.Empty));
        }
    }
}
