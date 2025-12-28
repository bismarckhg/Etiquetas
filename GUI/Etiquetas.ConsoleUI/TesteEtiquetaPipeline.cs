using Etiquetas.Application.Pipeline;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.ConsoleUI
{
    /// <summary>
    /// Classe de teste para o pipeline de etiquetas.
    /// </summary>
    public class TesteEtiquetaPipeline
    {
        /// <summary>
        /// Método de teste do pipeline de etiquetas.
        /// </summary>
        /// <returns>Retorna uma tarefa.</returns>
        public static async Task Teste()
        {
            const string connectionString = @"Filename=Data/Etiquetas.db;Connection=shared";

            using (var pipeline = new EtiquetaPipeline(connectionString))
            {
                Console.WriteLine("═══════════════════════════════════════════════");
                Console.WriteLine("   PIPELINE DE ETIQUETAS SATO - TPL DATAFLOW  ");
                Console.WriteLine("═══════════════════════════════════════════════\n");

                foreach (var item in SimuladorEtiquetasImpressao.RetornoDto())
                {
                    // ═══════════════════════════════════════════════════════════
                    // ETAPA 1: ABRIR ETIQUETA
                    // ═══════════════════════════════════════════════════════════
                    Console.WriteLine("▶ ABRINDO ETIQUETA\n");

                    //await pipeline.AbrirEtiquetaAsync();

                    await pipeline.AbrirEtiquetaAsync(
                        codigoMaterial: "123456",
                        codigoBarras: "7891234567890",
                        descricaoMedicamento: "DIPIRONA SODICA 500MG",
                        principioAtivo1: "DIPIRONA SODICA",
                        principioAtivo2: "",
                        lote: "L2025001",
                        validade: "2026-12-31",
                        matriculaFuncionario: "MAT001",
                        quantidadeSolicitada: 5,
                        jobName: "ETQ_20251215_001"
                    );

                    await Task.Delay(500);

                    // ═══════════════════════════════════════════════════════════
                    // ETAPA 2: SIMULAR PACOTES QUEBRADOS DA IMPRESSORA
                    // ═══════════════════════════════════════════════════════════
                    Console.WriteLine("\n▶ SIMULANDO PACOTES QUEBRADOS DA IMPRESSORA\n");

                    // Pacote completo para simulação
                    var pacoteCompleto = CriarPacoteSatoSimulado("ETQ_20251215_001", "00", 'A', 5);

                    // Simular pacote quebrado em 3 partes

                    var parte0 = Enumerable.Repeat((byte)'0', 8).ToArray();
                    var parte1 = new byte[10];
                    var parte2 = new byte[10];
                    var parte3 = new byte[8];

                    Array.Copy(pacoteCompleto, 0, parte1, 0, 10);
                    Array.Copy(pacoteCompleto, 10, parte2, 0, 10);
                    Array.Copy(pacoteCompleto, 20, parte3, 0, 8);

                    Console.WriteLine("→ Enviando PARTE 0 (8 bytes)");
                    await pipeline.ProcessarPacoteSatoAsync(parte0, "Porta2");
                    await Task.Delay(100);

                    Console.WriteLine("→ Enviando PARTE 1 (10 bytes)");
                    await pipeline.ProcessarPacoteSatoAsync(parte1, "Porta2");
                    await Task.Delay(100);

                    Console.WriteLine("→ Enviando PARTE 2 (10 bytes)");
                    await pipeline.ProcessarPacoteSatoAsync(parte2, "Porta2");
                    await Task.Delay(100);

                    Console.WriteLine("→ Enviando PARTE 3 (8 bytes) - COMPLETA PACOTE");
                    await pipeline.ProcessarPacoteSatoAsync(parte3, "Porta2");
                    await Task.Delay(500);

                    // ═══════════════════════════════════════════════════════════
                    // ETAPA 3: SIMULAR IMPRESSÃO DE MAIS CÓPIAS
                    // ═══════════════════════════════════════════════════════════
                    Console.WriteLine("\n▶ SIMULANDO IMPRESSÃO DE CÓPIAS\n");

                    for (int i = 4; i >= 0; i--)
                    {
                        pacoteCompleto = new byte[36];

                        var pacote = CriarPacoteSatoSimulado("ETQ_20251215_001", "00", 'G', i);

                        Array.Copy(parte0, 0, pacoteCompleto, 0, 8);
                        Array.Copy(pacote, 0, pacoteCompleto, 8, pacote.Length);

                        await pipeline.ProcessarPacoteSatoAsync(pacoteCompleto, "Porta2");
                        await Task.Delay(300);
                    }

                    // ═══════════════════════════════════════════════════════════
                    // ETAPA 4: AGUARDAR PROCESSAMENTO
                    // ═══════════════════════════════════════════════════════════
                    Console.WriteLine("\n▶ AGUARDANDO PROCESSAMENTO...\n");
                    await pipeline.AguardarProcessamentoPacotesAsync();

                    // ═══════════════════════════════════════════════════════════
                    // ETAPA 5: STATUS DOS BUFFERS
                    // ═══════════════════════════════════════════════════════════
                    Console.WriteLine("\n▶ STATUS DOS BUFFERS\n");
                    var statusBuffers = pipeline.ObterStatusBuffers();
                    foreach (var kvp in statusBuffers)
                    {
                        Console.WriteLine($"   {kvp.Key}: {kvp.Value} bytes pendentes");
                    }
                }

                await pipeline.CompletarAsync();

                Console.WriteLine("\n═══════════════════════════════════════════════");
                Console.WriteLine("   PIPELINE CONCLUÍDO COM SUCESSO");
                Console.WriteLine("═══════════════════════════════════════════════\n");
            }
        }

        /// <summary>
        /// Cria um pacote Sato simulado para testes.
        /// </summary>
        static byte[] CriarPacoteSatoSimulado(string jobName, string jobId, char status, long faltaImprimir)
        {
            var pacote = new byte[28];

            pacote[0] = 0x05; // ENQ
            pacote[1] = 0x02; // STX

            // JobId (2 bytes)
            var jobIdBytes = Encoding.ASCII.GetBytes(jobId.PadRight(2));
            Array.Copy(jobIdBytes, 0, pacote, 2, 2);

            // Status (1 byte)
            pacote[4] = (byte)status;

            // Numero falta imprimir (6 bytes)
            var faltaStr = faltaImprimir.ToString("000000");
            var faltaBytes = Encoding.ASCII.GetBytes(faltaStr);
            Array.Copy(faltaBytes, 0, pacote, 5, 6);

            // JobName (16 bytes)
            var jobNameBytes = Encoding.ASCII.GetBytes(jobName.PadRight(16));
            Array.Copy(jobNameBytes, 0, pacote, 11, 16);

            pacote[27] = 0x03; // ETX

            return pacote;
        }
    }
}
