using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Etiqueta.Application.DTOs;
using Etiqueta.Application.Mappers;
using Etiquetas.Application.Mappers;
using Etiquetas.Application.Pipeline.Messages;
using Etiquetas.Application.Services;
using Etiquetas.Core;
using Etiquetas.Core.Constants;
using Etiquetas.Core.Interfaces;
using Etiquetas.DAL;
using Etiquetas.DAL.Data.Repositories;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Pipeline
{
    /// <summary>
    /// Pipeline TPL Dataflow completo para processamento de etiquetas Sato.
    /// Processa pacotes quebrados, separa por ETX, transforma em itens individuais.
    /// </summary>
    public class EtiquetaPipeline : IDisposable
    {
        private readonly string _connectionString;
        private readonly PacoteSatoBufferService _bufferService;
        private readonly SatoParserService _parserService;

        // Controle de etiquetas abertas em memória
        private readonly ConcurrentDictionary<long, IEtiquetaImpressao> _etiquetasAbertas;
        private readonly ConcurrentDictionary<string, long> _jobNameParaId;

        // ═══════════════════════════════════════════════════════════════
        // BLOCOS DO PIPELINE - ABERTURA DE ETIQUETA
        // ═══════════════════════════════════════════════════════════════

        private readonly ActionBlock<AbrirEtiquetaMensagem> _abrirEtiquetaBlock;

        // ═══════════════════════════════════════════════════════════════
        // BLOCOS DO PIPELINE - PROCESSAMENTO DE PACOTES SATO
        // ═══════════════════════════════════════════════════════════════

        /// <summary>
        /// BLOCO 1: Recebe pacote de bytes bruto e monta pacotes completos usando buffer.
        /// </summary>
        private readonly TransformManyBlock<PacoteSatoMensagem, byte[]> _montarPacotesBlock;

        /// <summary>
        /// BLOCO 2: Faz parsing de cada pacote completo em SatoDto.
        /// </summary>
        private readonly TransformBlock<byte[], PacoteProcessadoMensagem> _parsePacoteBlock;

        /// <summary>
        /// BLOCO 3: Valida pacotes Sato.
        /// </summary>
        private readonly TransformBlock<PacoteProcessadoMensagem, PacoteProcessadoMensagem> _validarPacoteBlock;

        /// <summary>
        /// BLOCO 4: Broadcast para múltiplos destinos (FaltaImprimir + Atualização Etiqueta).
        /// </summary>
        private readonly BroadcastBlock<PacoteProcessadoMensagem> _broadcastPacoteBlock;

        /// <summary>
        /// BLOCO 5: Grava registro de FaltaImprimir no LiteDB.
        /// </summary>
        private readonly ActionBlock<PacoteProcessadoMensagem> _gravarFaltaImprimirBlock;

        /// <summary>
        /// BLOCO 6: Atualiza campo FaltaImpressao da etiqueta.
        /// </summary>
        private readonly ActionBlock<PacoteProcessadoMensagem> _atualizarEtiquetaBlock;

        // ═══════════════════════════════════════════════════════════════
        // BLOCOS DO PIPELINE - FECHAMENTO
        // ═══════════════════════════════════════════════════════════════

        private readonly ActionBlock<FecharEtiquetaMensagem> _fecharEtiquetaBlock;

        /// <summary>
        /// Initializes a new instance of the <see cref="EtiquetaPipeline"/> class.
        /// </summary>
        /// <param name="connectionString">String de conexão do LiteDB.</param>
        public EtiquetaPipeline(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _bufferService = new PacoteSatoBufferService();
            _parserService = new SatoParserService();

            _etiquetasAbertas = new ConcurrentDictionary<long, IEtiquetaImpressao>();
            _jobNameParaId = new ConcurrentDictionary<string, long>();

            InicializarIndices();

            // ═══════════════════════════════════════════════════════════════
            // CONFIGURAÇÃO DOS BLOCOS
            // ═══════════════════════════════════════════════════════════════

            // BLOCO: ABERTURA DE ETIQUETA
            _abrirEtiquetaBlock = new ActionBlock<AbrirEtiquetaMensagem>(
                msg => AbrirEtiquetaAsync(msg),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1, // Serializado
                    BoundedCapacity = 50
                }
            );

            // BLOCO 1: MONTAR PACOTES COMPLETOS (usando buffer para pacotes quebrados)
            _montarPacotesBlock = new TransformManyBlock<PacoteSatoMensagem, byte[]>(
                msg => MontarPacotesCompletos(msg),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1, // Serializado (buffer não é thread-safe por origem)
                    BoundedCapacity = 100
                }
            );

            // BLOCO 2: PARSE DE PACOTE COMPLETO
            _parsePacoteBlock = new TransformBlock<byte[], PacoteProcessadoMensagem>(
                pacote => ParsePacoteSato(pacote),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 4, // Paralelizado (parsing é CPU-bound)
                    BoundedCapacity = 200
                }
            );

            // BLOCO 3: VALIDAÇÃO
            _validarPacoteBlock = new TransformBlock<PacoteProcessadoMensagem, PacoteProcessadoMensagem>(
                msg => ValidarPacote(msg),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 4,
                    BoundedCapacity = 200
                }
            );

            // BLOCO 4: BROADCAST
            _broadcastPacoteBlock = new BroadcastBlock<PacoteProcessadoMensagem>(
                msg => msg,
                new DataflowBlockOptions
                {
                    BoundedCapacity = 100
                }
            );

            // BLOCO 5: GRAVAR FALTA IMPRIMIR
            _gravarFaltaImprimirBlock = new ActionBlock<PacoteProcessadoMensagem>(
                msg => GravarFaltaImprimirAsync(msg),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1, // Serializado (I/O)
                    BoundedCapacity = 100
                }
            );

            // BLOCO 6: ATUALIZAR ETIQUETA
            _atualizarEtiquetaBlock = new ActionBlock<PacoteProcessadoMensagem>(
                msg => AtualizarEtiquetaAsync(msg),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1,
                    BoundedCapacity = 100
                }
            );

            // BLOCO: FECHAMENTO
            _fecharEtiquetaBlock = new ActionBlock<FecharEtiquetaMensagem>(
                msg => FecharEtiquetaAsync(msg),
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 1
                }
            );

            // ═══════════════════════════════════════════════════════════════
            // LINKAGEM DOS BLOCOS
            // ═══════════════════════════════════════════════════════════════

            // Pipeline de pacotes Sato
            _montarPacotesBlock.LinkTo(_parsePacoteBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            _parsePacoteBlock.LinkTo(_validarPacoteBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            _validarPacoteBlock.LinkTo(_broadcastPacoteBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            // Broadcast para dois destinos
            _broadcastPacoteBlock.LinkTo(_gravarFaltaImprimirBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            _broadcastPacoteBlock.LinkTo(_atualizarEtiquetaBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });
        }

        /// <summary>
        /// Inicializa índices no LiteDB.
        /// </summary>
        private void InicializarIndices()
        {
            using (var ctx = new LiteDbContext(_connectionString))
            {
                ctx.EnsureIndices();
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // MÉTODOS DE PROCESSAMENTO - ABERTURA
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Abre uma nova etiqueta de impressão e grava no LiteDB.
        /// </summary>
        private async Task AbrirEtiquetaAsync(AbrirEtiquetaMensagem msg)
        {
            if (string.IsNullOrWhiteSpace(msg.CodigoBarras))
            {
                throw new ArgumentException("Código de barras é obrigatório", nameof(msg.CodigoBarras));
            }

            long.TryParse(msg.CodigoMaterial, out long codigoMaterial);
            DateTime.TryParse(msg.Validade, out DateTime validade);

            var etiqueta = new EtiquetaImpressao
            {
                CodigoMaterial = codigoMaterial,
                CodigoBarras = msg.CodigoBarras,
                DescricaoMedicamento = msg.DescricaoMedicamento ?? string.Empty,
                PrincipioAtivo1 = msg.PrincipioAtivo1 ?? string.Empty,
                PrincipioAtivo2 = msg.PrincipioAtivo2 ?? string.Empty,
                Lote = msg.Lote ?? string.Empty,
                Validade = validade,
                MatriculaFuncionario = msg.MatriculaFuncionario ?? string.Empty,
                DataHoraInicio = DateTime.Now,
                DataHoraFim = DateTime.MinValue,
                StatusEtiqueta = 'P', // Pendente
                QuantidadeSolicitada = msg.QuantidadeSolicitada,
                FaltaImpressao = msg.QuantidadeSolicitada,
                JobName = msg.JobName ?? $"{codigoMaterial}_{DateTime.Now:ddHHmmss}"
            };

            using (var ctx = new LiteDbContext(_connectionString))
            {
                var repo = new EtiquetaImpressaoRepository(ctx);
                await repo.InsertAsync(etiqueta).ConfigureAwait(false);
            }

            _etiquetasAbertas[etiqueta.Id] = etiqueta;
            _jobNameParaId[etiqueta.JobName] = etiqueta.Id;

            Console.WriteLine($"[ABERTA] Etiqueta {etiqueta.Id} | JobName: {etiqueta.JobName}");
            Console.WriteLine($"         Medicamento: {etiqueta.DescricaoMedicamento}");
            Console.WriteLine($"         Lote: {etiqueta.Lote} | Validade: {etiqueta.Validade:dd/MM/yyyy}");
            Console.WriteLine($"         Quantidade: {etiqueta.QuantidadeSolicitada} cópias");
        }

        // ═══════════════════════════════════════════════════════════════════
        // MÉTODOS DE PROCESSAMENTO - PACOTES SATO
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Monta pacotes completos a partir de bytes quebrados usando buffer.
        /// </summary>
        private IEnumerable<byte[]> MontarPacotesCompletos(PacoteSatoMensagem msg)
        {
            var origem = msg.Origem ?? "default";
            var pacotesCompletos = _bufferService.ProcessarBytes(origem, msg.PacoteBytes);

            foreach (var pacote in pacotesCompletos)
            {
                Console.WriteLine($"[MONTADO] Pacote completo de {origem}: {pacote.Length} bytes");
                yield return pacote;
            }
        }

        /// <summary>
        /// Faz parsing de um pacote completo em SatoDto.
        /// </summary>
        private async Task<PacoteProcessadoMensagem> ParsePacoteSato(byte[] pacoteCompleto)
        {
            var sato = new SatoDto();
            Array.Copy(pacoteCompleto, 0, sato.RetornoImpressao, 0, Math.Min(pacoteCompleto.Length, PadraoConstantes.ConstTamanhoProtocolo));

            Console.WriteLine($"[PARSED] JobName: {sato.JobName} | JobId: {sato.JobId} | Falta: {sato.NumeroFaltaImprimir} | Status: {(char)sato.Status}");

            return await Task.FromResult(new PacoteProcessadoMensagem
            {
                SatoDto = sato,
                EhValido = false // Será validado no próximo bloco
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Valida pacote Sato.
        /// </summary>
        private async Task<PacoteProcessadoMensagem> ValidarPacote(PacoteProcessadoMensagem msg)
        {
            msg.EhValido = msg.SatoDto.EhValido();

            if (!msg.EhValido)
            {
                Console.WriteLine($"[INVÁLIDO] Pacote descartado - ENQ:{msg.SatoDto.ENQ} STX:{msg.SatoDto.STX} ETX:{msg.SatoDto.ETX}");
            }

            return await Task.FromResult(msg).ConfigureAwait(false);
        }

        /// <summary>
        /// Grava registro de FaltaImprimir no LiteDB (cada cópia impressa).
        /// </summary>
        private async Task GravarFaltaImprimirAsync(PacoteProcessadoMensagem msg)
        {
            if (!msg.EhValido)
            {
                return;
            }

            var sato = msg.SatoDto;

            // Tentar resolver ID da etiqueta
            long? idEtiqueta = null;
            if (_jobNameParaId.TryGetValue(sato.JobName, out long id))
            {
                idEtiqueta = id;
            }

            var faltaImprimir = new FaltaImprimir
            {
                Id = Guid.NewGuid().ToString("N"),
                IdEtiquetaImpressao = idEtiqueta?.ToString() ?? sato.JobId,
                NomeDoJOB = sato.JobName,
                DataImpressao = DateTime.Now,
                StatusImpressora = sato.Status,
                FaltaImpressao = sato.NumeroFaltaImprimir
            };

            using (var ctx = new LiteDbContext(_connectionString))
            {
                var repo = new FaltaImprimirRepository(ctx);
                await repo.InsertAsync(faltaImprimir).ConfigureAwait(false);
            }

            Console.WriteLine($"[GRAVADO] FaltaImprimir | Job: {sato.JobName} | Falta: {sato.NumeroFaltaImprimir}");
        }

        /// <summary>
        /// Atualiza campo FaltaImpressao da etiqueta.
        /// </summary>
        private async Task AtualizarEtiquetaAsync(PacoteProcessadoMensagem msg)
        {
            if (!msg.EhValido)
            {
                return;
            }

            var sato = msg.SatoDto;

            if (!_jobNameParaId.TryGetValue(sato.JobName, out long idEtiqueta))
            {
                Console.WriteLine($"[AVISO] Etiqueta não encontrada para JobName: {sato.JobName}");
                return;
            }

            if (!_etiquetasAbertas.TryGetValue(idEtiqueta, out var etiqueta))
            {
                Console.WriteLine($"[AVISO] Etiqueta {idEtiqueta} não está em memória");
                return;
            }

            etiqueta.FaltaImpressao = sato.NumeroFaltaImprimir;

            if (sato.NumeroFaltaImprimir == 0)
            {
                etiqueta.StatusEtiqueta = 'C'; // Concluída
                etiqueta.DataHoraFim = DateTime.Now;
            }

            using (var ctx = new LiteDbContext(_connectionString))
            {
                var repo = new EtiquetaImpressaoRepository(ctx);
                await repo.UpdateAsync(etiqueta).ConfigureAwait(false);
            }

            Console.WriteLine($"[ATUALIZADO] Etiqueta {idEtiqueta} | Falta: {sato.NumeroFaltaImprimir}");

            await Task.CompletedTask;
        }

        // ═══════════════════════════════════════════════════════════════════
        // MÉTODOS DE PROCESSAMENTO - FECHAMENTO
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Fecha uma etiqueta manualmente.
        /// </summary>
        private async Task FecharEtiquetaAsync(FecharEtiquetaMensagem msg)
        {
            if (!_etiquetasAbertas.TryRemove(msg.IdEtiqueta, out var etiqueta))
            {
                Console.WriteLine($"[AVISO] Etiqueta {msg.IdEtiqueta} não encontrada para fechamento");
                return;
            }

            etiqueta.StatusEtiqueta = 'F'; // Fechada manualmente
            etiqueta.DataHoraFim = DateTime.Now;

            using (var ctx = new LiteDbContext(_connectionString))
            {
                var repo = new EtiquetaImpressaoRepository(ctx);
                await repo.UpdateAsync(etiqueta).ConfigureAwait(false);
            }

            _jobNameParaId.TryRemove(etiqueta.JobName, out _);

            Console.WriteLine($"[FECHADA] Etiqueta {msg.IdEtiqueta} | JobName: {etiqueta.JobName}");

            await Task.CompletedTask;
        }

        // ═══════════════════════════════════════════════════════════════════
        // MÉTODOS PÚBLICOS - INTERFACE DO PIPELINE
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Envia mensagem para abrir uma nova etiqueta.
        /// </summary>
        public async Task AbrirEtiquetaAsync(
            string codigoMaterial,
            string codigoBarras,
            string descricaoMedicamento,
            string principioAtivo1,
            string principioAtivo2,
            string lote,
            string validade,
            string matriculaFuncionario,
            long quantidadeSolicitada,
            string jobName = null)
        {
            var msg = new AbrirEtiquetaMensagem
            {
                CodigoMaterial = codigoMaterial,
                CodigoBarras = codigoBarras,
                DescricaoMedicamento = descricaoMedicamento,
                PrincipioAtivo1 = principioAtivo1,
                PrincipioAtivo2 = principioAtivo2,
                Lote = lote,
                Validade = validade,
                MatriculaFuncionario = matriculaFuncionario,
                QuantidadeSolicitada = quantidadeSolicitada,
                JobName = jobName
            };

            await _abrirEtiquetaBlock.SendAsync(msg).ConfigureAwait(false);
        }


        /// <summary>
        /// Envia mensagem para abrir uma nova etiqueta.
        /// </summary>
        public async Task AbrirEtiquetaAsync(ISatoDto satoDto)
        {

        }

        /// <summary>
        /// Envia pacote de bytes recebido da impressora Sato.
        /// </summary>
        public async Task ProcessarPacoteSatoAsync(byte[] pacoteBytes, string origem = "default")
        {
            var msg = new PacoteSatoMensagem
            {
                PacoteBytes = pacoteBytes,
                TamanhoRecebido = pacoteBytes?.Length ?? 0,
                Origem = origem
            };

            await _montarPacotesBlock.SendAsync(msg).ConfigureAwait(false);
        }

        /// <summary>
        /// Envia mensagem para fechar etiqueta.
        /// </summary>
        public async Task FecharEtiquetaAsync(long idEtiqueta, string jobName = null)
        {
            var msg = new FecharEtiquetaMensagem
            {
                IdEtiqueta = idEtiqueta,
                JobName = jobName
            };

            await _fecharEtiquetaBlock.SendAsync(msg).ConfigureAwait(false);
        }

        /// <summary>
        /// Aguarda conclusão de todos os blocos de pacotes Sato.
        /// </summary>
        public async Task AguardarProcessamentoPacotesAsync()
        {
            _montarPacotesBlock.Complete();
            await Task.WhenAll(
                _montarPacotesBlock.Completion,
                _parsePacoteBlock.Completion,
                _validarPacoteBlock.Completion,
                _broadcastPacoteBlock.Completion,
                _gravarFaltaImprimirBlock.Completion,
                _atualizarEtiquetaBlock.Completion
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Completa todo o pipeline.
        /// </summary>
        public async Task CompletarAsync()
        {
            _abrirEtiquetaBlock.Complete();
            _montarPacotesBlock.Complete();
            _fecharEtiquetaBlock.Complete();

            await Task.WhenAll(
                _abrirEtiquetaBlock.Completion,
                _montarPacotesBlock.Completion,
                _parsePacoteBlock.Completion,
                _validarPacoteBlock.Completion,
                _broadcastPacoteBlock.Completion,
                _gravarFaltaImprimirBlock.Completion,
                _atualizarEtiquetaBlock.Completion,
                _fecharEtiquetaBlock.Completion
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Retorna status dos buffers de pacotes.
        /// </summary>
        public Dictionary<string, int> ObterStatusBuffers()
        {
            return _bufferService.ObterStatusBuffers();
        }

        /// <summary>
        /// Limpa buffer de uma origem específica.
        /// </summary>
        public void LimparBuffer(string origem)
        {
            _bufferService.LimparBuffer(origem);
        }

        /// <summary>
        /// Libera recursos do pipeline.
        /// </summary>
        public void Dispose()
        {
            _abrirEtiquetaBlock?.Complete();
            _montarPacotesBlock?.Complete();
            _fecharEtiquetaBlock?.Complete();
        }
    }
}
