using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Etiquetas.Core;
using Etiquetas.DAL;
using Etiquetas.DAL.Data.Repositories;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Services
{
    /// <summary>
    /// Gerencia uma fila de operações pendentes e as processa de forma assíncrona com paralelismo configurável.
    /// </summary>
    /// <remarks> <see cref="PendingOperationQueue"/> permite que operações enfileiradas sejam processadas em
    /// segundo plano por um conjunto de tarefas de trabalho. As operações são processadas na ordem em que são recebidas, com suporte para
    /// capacidade de fila limitada e repetição automática com recuo exponencial em caso de falha.
    /// Chame <see cref="StartAsync(CancellationToken)"/> para iniciar o processamento das operações enfileiradas.
    /// Use <see cref="EnqueueAsync(PendingOperation, CancellationToken)"/> para adicionar novas operações à fila.
    /// Esta classe é thread-safe e destinada ao uso em cenários onde o processamento em segundo plano e o tratamento confiável de operações
    /// são necessários. Descarte a instância para interromper o processamento e liberar recursos.</remarks>
    public class PendingOperationQueue : IDisposable
    {
        private readonly BufferBlock<PendingOperation> privQueue;
        private readonly string privConnectionString;
        private readonly ConsoleLogger privLogger;

        private readonly int privDegreeOfParallelism;
        private readonly TimeSpan privInitialBackoff = TimeSpan.FromMilliseconds(100);
        private readonly TimeSpan privMaxBackoff = TimeSpan.FromSeconds(5);

        private CancellationTokenSource cancellationToken;
        private Task[] privWorkers;

        /// <summary>
        /// Initializes a new instance of the <see cref="PendingOperationQueue"/> class.
        /// Inicializa uma nova instância da classe <see cref="PendingOperationQueue"/> com a conexão especificada
        /// string, logger, capacidade da fila e grau de paralelismo.
        /// </summary>
        /// <remarks>Use este construtor para configurar a capacidade da fila e o nível de concorrência
        /// para processar operações pendentes. Se <paramref name="degreeOfParallelism"/> for menor que 1, ele será
        /// automaticamente definido como 1.</remarks>
        /// <param name="connectionString">A string de conexão usada para conectar ao recurso de destino para operações pendentes. Não pode ser <see
        /// langword="null"/> ou vazia.</param>
        /// <param name="logger">O logger usado para registrar mensagens de diagnóstico e operacionais. Não pode ser <see langword="null"/>.</param>
        /// <param name="boundedCapacity">O número máximo de operações pendentes que podem ser enfileiradas simultaneamente. Deve ser maior que zero. O
        /// padrão é 2048.</param>
        /// <param name="degreeOfParallelism">O número máximo de operações que podem ser processadas simultaneamente. Deve ser pelo menos 1. O padrão é 2.</param>
        public PendingOperationQueue(string connectionString, ConsoleLogger logger, int boundedCapacity = 2048, int degreeOfParallelism = 2)
        {
            privConnectionString = connectionString;
            privLogger = logger;
            privDegreeOfParallelism = Math.Max(1, degreeOfParallelism);
            privQueue = new BufferBlock<PendingOperation>(new DataflowBlockOptions { BoundedCapacity = boundedCapacity });
        }

        /// <summary>
        /// Enfileira uma operação pendente para ser processada de forma assíncrona.
        /// </summary>
        /// <param name="op">A <see cref="PendingOperation"/> a ser enfileirada. Não pode ser <c>null</c>.</param>
        /// <param name="ct">Um <see cref="CancellationToken"/> que pode ser usado para cancelar a operação de enfileiramento.</param>
        /// <returns>Uma tarefa que representa a operação de enfileiramento assíncrona.</returns>
        public Task EnqueueAsync(PendingOperation op, CancellationToken ct)
        {
            return privQueue.SendAsync(op, ct);
        }

        /// <summary>
        /// Inicia as tarefas de trabalho em segundo plano com o token de cancelamento especificado.
        /// </summary>
        /// <remarks>Este método inicia várias tarefas de trabalho em segundo plano de acordo com o grau de paralelismo configurado.
        /// As tarefas continuarão em execução até que o token de cancelamento fornecido seja sinalizado
        /// ou a instância seja descartada.</remarks>
        /// <param name="ct">Um <see cref="CancellationToken"/> que pode ser usado para solicitar o cancelamento das tarefas de trabalho em segundo plano.</param>
        /// <returns>Uma <see cref="Task"/> concluída que representa o início das tarefas de trabalho em segundo plano.</returns>
        public Task StartAsync(CancellationToken ct)
        {
            cancellationToken = CancellationTokenSource.CreateLinkedTokenSource(ct);
            privWorkers = new Task[privDegreeOfParallelism];
            for (int i = 0; i < privWorkers.Length; i++)
            {
                privWorkers[i] = Task.Run(() => WorkerLoop(cancellationToken.Token), cancellationToken.Token);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Libera todos os recursos usados ​​pela instância atual e tenta encerrar corretamente quaisquer tarefas em segundo plano em execução.
        /// </summary>
        /// <remarks>Chame este método quando a instância não for mais necessária para garantir que todas as operações em segundo plano sejam canceladas e os recursos sejam liberados. Após chamar <see cref="Dispose"/>, a instância não deve ser usada.</remarks>
        public void Dispose()
        {
            try
            {
                cancellationToken?.Cancel();
                if (privWorkers != null)
                {
                    Task.WaitAll(privWorkers, TimeSpan.FromSeconds(5));
                }

            }
            catch
            {
            }
        }

        /// <summary>
        /// Calcula o intervalo de espera para uma nova tentativa usando espera exponencial com jitter.
        /// </summary>
        /// <remarks>Este método aplica espera exponencial, dobrando o intervalo a cada tentativa, até
        /// o máximo especificado. Um fator de jitter aleatório entre 1,0 e 1,5 é aplicado para ajudar a reduzir a contenção
        /// e evitar novas tentativas sincronizadas em sistemas distribuídos.</remarks>
        /// <param name="initial">O intervalo de espera inicial a ser usado para a primeira tentativa de nova tentativa. Deve ser um <see
        /// cref="TimeSpan"/> não negativo.</param>
        /// <param name="max">O intervalo de espera máximo permitido. O intervalo de espera calculado não excederá este valor. Deve ser um <see cref="TimeSpan"/> não negativo.</param>
        /// <param name="attempt">O número da tentativa de nova tentativa atual. Deve ser maior ou igual a 1. Cada incremento aumenta o backoff
        /// exponencialmente.</param>
        /// <returns>Um <see cref="TimeSpan"/> representando o intervalo de backoff calculado para a tentativa especificada, incluindo um
        /// fator de jitter aleatório. O valor está sempre entre <paramref name="initial"/> e <paramref name="max"/>,
        /// inclusive.</returns>
        private static TimeSpan ComputeBackoff(TimeSpan initial, TimeSpan max, int attempt)
        {
            double mult = Math.Pow(2, attempt - 1);
            var ms = Math.Min(max.TotalMilliseconds, initial.TotalMilliseconds * mult);
            var jitter = (new Random().NextDouble() * 0.5) + 1.0;
            return TimeSpan.FromMilliseconds(ms * jitter);
        }

        /// <summary>
        /// Processa continuamente as operações pendentes da fila interna até que o cancelamento seja solicitado.
        /// </summary>
        /// <remarks>O método recupera operações da fila interna e tenta processar cada uma.
        /// Se uma operação não puder ser processada devido a um erro, ela será repetida com recuo exponencial até que
        /// seja bem-sucedida ou até que o cancelamento seja solicitado.</remarks>
        /// <param name="ct">Um <see cref="CancellationToken"/> usado para observar solicitações de cancelamento e encerrar o loop normalmente.</param>
        /// <returns>Uma tarefa que representa a execução assíncrona do loop de trabalho.</returns>
        private async Task WorkerLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                PendingOperation op = null;
                try
                {
                    op = await privQueue.ReceiveAsync(ct).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                if (op == null)
                {
                    continue;
                }

                op.Attempt++;
                try
                {
                    using (var ctx = new LiteDbContext(privConnectionString))
                    {
                        var repo = new EtiquetaImpressaoRepository(ctx);
                        var found = repo.FindByJobNameAsync(op.KeyValue).Result;
                        if (found != null)
                        {
                            var falta = new FaltaImprimir
                            {
                                Id = Guid.NewGuid().ToString("N"),
                                IdEtiquetaImpressao = found.Id.ToString(),
                                NomeDoJOB = op.KeyValue,
                                DataImpressao = DateTime.UtcNow,
                                StatusImpressora = 0,
                                FaltaImpressao = op.Registro.FaltaImpressao
                            };

                            var faltaRepo = new FaltaImprimirRepository(ctx);
                            faltaRepo.InsertAsync(falta).Wait();
                            privLogger.Info("Pending op applied: linked to existing etiqueta Id=" + found.Id);
                            continue;
                        }
                        else
                        {
                            repo.InsertAsync(op.Registro).Wait();
                            privLogger.Info("Pending op inserted new EtiquetaImpressao");
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    privLogger.Error("Pending worker error", ex);
                    var backoff = ComputeBackoff(privInitialBackoff, privMaxBackoff, op.Attempt);
                    try
                    {
                        await Task.Delay(backoff, ct).ConfigureAwait(false);
                    }
                    catch
                    {
                    }

                    try
                    {
                        await privQueue.SendAsync(op, ct).ConfigureAwait(false);
                    }
                    catch
                    { 
                    }
                }
            }
        }
    }
}
