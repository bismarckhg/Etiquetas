using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Application.Services
{
    /// <summary>
    /// Representa uma operação pendente para processamento.
    /// </summary>
    public class PendingOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PendingOperation"/> class.
        /// Inicializa uma nova instancia da classe <see cref="PendingOperation"/>.
        /// </summary>
        /// <param name="key">Chave EtiquetaImpressao.</param>
        /// <param name="key2">Segunda Chave EtiquetaImpressao.</param>
        /// <param name="etiqueta">Entidade de EtiquetaImpressão.</param>
        public PendingOperation(string key, string key2, IEtiquetaImpressao etiqueta)
        {
            TableName = "EtiquetaImpressao";
            KeyValue = key;
            Registro = etiqueta;
            Attempt = 0;
            EnqueuedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Gets the key value.
        /// </summary>
        public string KeyValue { get; }

        /// <summary>
        /// Gets the record.
        /// </summary>
        public IEtiquetaImpressao Registro { get; }

        /// <summary>
        /// Gets or sets the attempt count.
        /// </summary>
        public int Attempt { get; set; }

        /// <summary>
        /// Gets the time the operation was enqueued.
        /// </summary>
        public DateTime EnqueuedAt { get; }
    }
}
