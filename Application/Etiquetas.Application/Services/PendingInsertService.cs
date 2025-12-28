using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Etiqueta.Application.DTOs;
using Etiquetas.Application.DTOs;
using Etiquetas.Core;
using Etiquetas.Core.Interfaces;
using Etiquetas.DAL;
using Etiquetas.DAL.Data.Repositories;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Services
{
    /// <summary>
    /// Serviço para inserção de etiquetas pendentes.
    /// </summary>
    public class PendingInsertService
    {
        /// <summary>
        /// String de conexão com o banco de dados LiteDB.
        /// </summary>
        private readonly string connString;

        /// <summary>
        /// Resolvedor de IDs de etiquetas.
        /// </summary>
        private readonly IEtiquetaIdResolver resolver;

        /// <summary>
        /// Fila de operações pendentes.
        /// </summary>
        private readonly PendingOperationQueue pendQueue;

        /// <summary>
        /// Logger de registro.
        /// </summary>
        private readonly ConsoleLogger regLogger;

        // <param name="connectionString">String de conexão do liteDB.</param>
        // <param name="pendingQueue">Fila de Operações pendentes.</param>
        // <param name="logger">Log de operação.</param>
        // <param name="resolver">Parametro com o Id do DTO de retorno da Impressora Sato.</param>

        /// <summary>
        /// Initializes a new instance of the <see cref="PendingInsertService"/> class for managing pending insert
        /// operations.
        /// Inicializa uma nova instância da classe <see cref="PendingInsertService"/> para gerenciar operações de inserção pendentes.
        /// </summary>
        /// <remarks>Use este construtor para configurar o serviço com a conexão de banco de dados necessária,
        /// fila de operações pendentes e mecanismo de registro. O parâmetro <paramref name="resolver"/> é opcional e
        /// deve ser fornecido se a resolução do ID da impressora for necessária para operações pendentes.</remarks>        /// <param name="connectionString">A Conneção String usada para conectar no LiteDB database. Não pode ser nulo ou vazio.</param>
        /// <param name="pendingQueue">A fila que armazena as operações pendentes a serem processadas.Cannot be null.</param>
        /// <param name="logger">O registrador usado para registrar eventos operacionais. Se nulo, um valor padrão.<see cref="ConsoleLogger"/> is used.</param>
        /// <param name="resolver">Um opcional que resolve o ID do DTO de retorno da impressora Sato. Se não for especificado, a resolvido o ID da impressora
        /// não estará disponível.</param>
        public PendingInsertService(string connectionString, PendingOperationQueue pendingQueue, ConsoleLogger logger, IEtiquetaIdResolver resolver = null)
        {
            connString = connectionString;
            pendQueue = pendingQueue;
            regLogger = logger ?? new ConsoleLogger();
            this.resolver = resolver;
        }

        /// <summary>
        /// Tenta inserir o rótulo especificado diretamente ou o coloca em fila para processamento posterior,
        /// caso a inserção imediata não seja possível.
        /// </summary>
        /// <remarks>Se o identificador do rótulo puder ser resolvido a partir do trabalho Sato fornecido, o 
        /// rótulo será inserido imediatamente e o método retornará <see langword="true"/>. Se o identificador
        /// não puder ser resolvido, o rótulo será enfileirado para processamento posterior e o método 
        /// retornará <see langword="false"/>.</remarks>
        /// <param name="sato">As informações do trabalho Sato usadas para resolver o identificador do rótulo e o contexto do trabalho. Não pode ser `null`.</param>
        /// <param name="toInsert">O rótulo a ser inserido ou enfileirado. A propriedade `EtiquetaImpressao.Id` pode ser definida se a inserção
        /// for bem-sucedida. Não pode ser `null`.</param>
        /// <param name="ct">Um token de cancelamento que pode ser usado para cancelar a operação assíncrona.</param>
        /// <returns><see langword="true"/> se o rótulo foi inserido diretamente; caso contrário, <see langword="false"/> se o rótulo
        /// foi enfileirado para processamento posterior.</returns>
        public async Task<bool> TryInsertOrEnqueueAsync(SatoDto sato, EtiquetaImpressao toInsert, CancellationToken ct)
        {
            long? etiquetaId = null;
            if (resolver != null)
            {
                etiquetaId = await resolver.ResolveEtiquetaIdAsync(sato).ConfigureAwait(false);
            }

            if (etiquetaId.HasValue)
            {
                toInsert.Id = etiquetaId.Value;
                await InsertDirectAsync(toInsert).ConfigureAwait(false);
                return true;
            }

            var falta = new PendingOperation(sato.JobName, sato.JobId, toInsert);
            await pendQueue.EnqueueAsync(falta, ct).ConfigureAwait(false);
            regLogger.Info("Enqueued pending operation for job=" + sato.JobName);
            return false;
        }

        private Task InsertDirectAsync(EtiquetaImpressao ent)
        {
            using (var ctx = new LiteDbContext(connString))
            {
                var repo = new EtiquetaImpressaoRepository(ctx);
                return repo.InsertAsync(ent);
            }
        }
    }
}
