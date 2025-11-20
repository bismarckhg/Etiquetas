using Etiquetas.Bibliotecas.TaskCore;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.GUI.Texto2
{
    public class ExemploTASK
    {
        public static async Task ExecutaTodosExemplos()
        {
            ITaskParametros parametros01 = new TaskParametros();
            ITaskParametros parametros02 = new TaskParametros();

            var grupo = new TasksGrupos("Exemplos");
            await grupo.AdicionarTask(1, Exemplo01Contador, parametros01, "Exemplo01Contador");
            await grupo.AdicionarTask(2, Exemplo02Contador, parametros02, "Exemplo02Contador");
            grupo.IniciarExecucao();

            await Task.Delay(1200);

            //cts01.Cancel();
            grupo.CancelamentoTaskPorNome("Exemplo01Contador");

            await Task.Delay(1200);

            //ctsBreak02.Cancel();
            grupo.CancelamentoBrutoTaskPorNome("Exemplo02Contador");

            await grupo.AguardaTodasTasksAsync();
        }

        public static async Task<ITaskReturnValue> Exemplo01Contador(ITaskParametros parametros)
        {
            var cancellationToken = parametros.RetornoCancellationToken;
            var cancellationTokenBreak = parametros.RetornoCancellationTokenBreak;

            // ✅ DIAGNÓSTICO 4: Verificar token recebido
            Console.WriteLine($"Exemplo01Contador Token recebido - CanBeCanceled: {cancellationToken.CanBeCanceled}");
            Console.WriteLine($"Exemplo01Contador Token recebido - HashCode: {cancellationToken.GetHashCode()}");
            Console.WriteLine($"Exemplo01Contador Token Break recebido - CanBeCanceled: {cancellationTokenBreak.CanBeCanceled}");
            Console.WriteLine($"Exemplo01Contador Token Break recebido - HashCode: {cancellationTokenBreak.GetHashCode()}");

            try
            {
                cancellationTokenBreak.ThrowIfCancellationRequested();
                var ciclo = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    ciclo++;
                    Console.WriteLine($"Exemplo01Contador Ciclo {ciclo}");
                    Console.WriteLine(" - Iniciando contador de 1 a 50...");

                    for (int i = 0; i <= 50; i++)
                    {
                        // ✅ DIAGNÓSTICO 5: Verificar a cada iteração
                        Console.Write($"Exemplo01Contador Ciclo: {ciclo} Contador: {i}");
                        Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
                        Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");
                        await Task.Delay(100);
                    }
                }
                Console.Write("Exemplo01Contador Task cancelada.");
                Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
                Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Exemplo01Contador Task cancelada(BREAK).");
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Exemplo01Contador Exceção: {ex.Message}");
                throw;
            }

            return new TaskReturnValue();
        }

        //public static async Task<ITaskReturnValue> Exemplo02Contador(ITaskParametros parametros)
        //{
        //    var cancellationToken = parametros.RetornoCancellationToken;
        //    var cancellationTokenBreak = parametros.RetornoCancellationTokenBreak;

        //    Console.WriteLine($"Exemplo02Contador Token normal - CanBeCanceled: {cancellationToken.CanBeCanceled}");
        //    Console.WriteLine($"Exemplo02Contador Token break - CanBeCanceled: {cancellationTokenBreak.CanBeCanceled}");

        //    // 🔗 Mantém os tokens separados (não usaremos linked)
        //    bool cancelamentoBreakDetectado = false;

        //    // ⚡ Reação imediata ao cancelamento "Break"
        //    cancellationTokenBreak.Register(() =>
        //    {
        //        cancelamentoBreakDetectado = true;
        //        Console.WriteLine("⚠️ Cancelamento BREAK detectado forçando interrupção...");
        //    });

        //    try
        //    {
        //        int ciclo = 0;
        //        Console.WriteLine("🚀 Exemplo02Contador iniciado.");

        //        // 🔄 Loop principal controlado pelo token normal
        //        while (!cancellationToken.IsCancellationRequested)
        //        {
        //            ciclo++;

        //            // ✅ Cancelamento BREAK (forte) no início de cada loop
        //            if (cancelamentoBreakDetectado || cancellationTokenBreak.IsCancellationRequested)
        //            {
        //                Console.WriteLine("🧨 Interrompendo imediatamente via.");
        //                throw new OperationCanceledException(cancellationTokenBreak);
        //            }

        //            Console.WriteLine($"Exemplo02Contador Ciclo {ciclo}");
        //            Console.WriteLine(" - Iniciando contador de 1 a 50...");

        //            // 🔁 Loop interno com checagem leve e delays canceláveis
        //            for (int i = 0; i <= 50; i++)
        //            {
        //                // 👀 Checa o break token a cada iteração também
        //                if (cancelamentoBreakDetectado || cancellationTokenBreak.IsCancellationRequested)
        //                {
        //                    Console.WriteLine($"🛑 BREAK detectado durante contagem (Ciclo {ciclo}, i={i})");
        //                    throw new OperationCanceledException(cancellationTokenBreak);
        //                }

        //                Console.Write($"Exemplo02Contador Ciclo: {ciclo} Contador: {i}");
        //                Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
        //                Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");

        //                // ⏱️ O delay respeita cancelamento normal, e também o break (duas camadas)
        //                await Task.Delay(100, cancellationToken);
        //            }

        //            Console.WriteLine($"✅ Ciclo {ciclo} concluído;");
        //        }

        //        Console.WriteLine("🟢 Exemplo02Contador finalizado normalmente (cancelamento normal ou fim natural).");
        //    }
        //    catch (OperationCanceledException ex)
        //    {
        //        // Captura cancelamento BREAK com distinção
        //        if (ex.CancellationToken == cancellationTokenBreak)
        //        {
        //            Console.WriteLine("🟥 Exemplo02Contador cancelado via BREAK (parada brusca controlada).");
        //        }
        //        else
        //        {
        //            Console.WriteLine("🟠 Exemplo02Contador cancelado via token normal.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"💥 Exemplo02Contador exceção: {ex.Message}");
        //    }
        //    finally
        //    {
        //        Console.WriteLine("🔚 Exemplo02Contador liberando recursos e encerrando.");
        //    }

        //    return new TaskReturnValue();
        //}

        public static async Task<ITaskReturnValue> Exemplo02Contador(ITaskParametros parametros)
        {
            var cancellationToken = parametros.RetornoCancellationToken;
            var cancellationTokenBreak = parametros.RetornoCancellationTokenBreak;

            // ✅ DIAGNÓSTICO 4: Verificar token recebido
            Console.WriteLine($"Exemplo02Contador Token recebido - CanBeCanceled: {cancellationToken.CanBeCanceled}");
            Console.WriteLine($"Exemplo02Contador Token recebido - HashCode: {cancellationToken.GetHashCode()}");
            Console.WriteLine($"Exemplo02Contador Token Break recebido - CanBeCanceled: {cancellationTokenBreak.CanBeCanceled}");
            Console.WriteLine($"Exemplo02Contador Token Break recebido - HashCode: {cancellationTokenBreak.GetHashCode()}");

            try
            {
                var ciclo = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    ciclo++;
                    Console.WriteLine($"Exemplo02Contador Ciclo {ciclo}");
                    Console.WriteLine(" - Iniciando contador de 1 a 50...");
                    cancellationTokenBreak.ThrowIfCancellationRequested();

                    for (int i = 0; i <= 50; i++)
                    {
                        cancellationTokenBreak.ThrowIfCancellationRequested();
                        // ✅ DIAGNÓSTICO 5: Verificar a cada iteração
                        Console.Write($"Exemplo02Contador Ciclo: {ciclo} Contador: {i}");
                        Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
                        Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");
                        await Task.Delay(100, cancellationTokenBreak);
                    }
                }
                Console.Write("Exemplo02Contador Task cancelada.");
                Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
                Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");
            }
            catch (OperationCanceledException)
            {
                Console.Write("Exemplo02Contador Task cancelada.");
                Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
                Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");
            }

            return new TaskReturnValue();
        }
    }
}
