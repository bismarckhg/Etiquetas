using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiqueta.Application.Mappers;
using Etiquetas.Application.Services;
using Etiquetas.DAL;
using Etiquetas.DAL.Data.Repositories;

namespace Etiquetas.ConsoleUI
{
    /// <summary>
    /// Ponto de entrada da aplicação console para teste com buffer pool.
    /// </summary>
    public static class TestePooled
    {
        /// <summary>
        /// Ponto de entrada principal da aplicação.
        /// </summary>
        /// <returns>Retorna uma tarefa.</returns>
        public static async Task Teste()
        {
            var conn = "Filename=etiquetas.db;Mode=Shared";

            using (var ctx = new LiteDbContext(conn))
            {
                ctx.EnsureIndices();

                var logger = new ConsoleLogger();
                var bufferPool = new BufferPool(8192);

                var repoEti = new EtiquetaImpressaoRepository(ctx);
                var repoFalta = new FaltaImprimirRepository(ctx);

                var parser = new SatoParserService();

                var pendingQueue = new PendingOperationQueue(conn, logger);
                await pendingQueue.StartAsync(System.Threading.CancellationToken.None);

                var pendingService = new PendingInsertService(conn, pendingQueue, logger);

                using (var client = TcpSimulator.CreateSimulatedClient(totalMessages: 200, bytesPerMessage: 512, intervalMs: 5))
                using (var stream = client.GetStream())
                {
                    var ms = new System.IO.MemoryStream();
                    await stream.CopyToAsync(ms).ConfigureAwait(false);
                    var data = ms.ToArray();

                    var satoList = parser.Parse(data);
                    var faltas = SatoToFaltaMapper.Map(satoList);

                    foreach (var f in faltas)
                    {
                        await repoFalta.InsertAsync(f).ConfigureAwait(false);
                        Console.WriteLine("Inserted falta: " + f.Id);
                    }
                }

                Console.WriteLine("Done.");
            }
        }
    }
}
