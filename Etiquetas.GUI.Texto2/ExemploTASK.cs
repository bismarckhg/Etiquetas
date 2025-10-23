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
            var cts01 = new CancellationTokenSource();
            var ctsBreak01 = new CancellationTokenSource();
            ITaskParametros parametros01 = new TaskParametros();
            parametros01.ArmazenaCancellationToken(cts01.Token);
            parametros01.ArmazenaCancellationTokenBreak(ctsBreak01.Token);

            // ✅ DIAGNÓSTICO 1: Verificar se o token foi armazenado
            Console.WriteLine($"Exemplo01Contador Token armazenado cts01 - CanBeCanceled: {parametros01.RetornoCancellationToken.CanBeCanceled}");
            Console.WriteLine($"Exemplo01Contador Token armazenado cts01 - HashCode: {parametros01.RetornoCancellationToken.GetHashCode()}");
            Console.WriteLine($"Exemplo01Contador Token Break01 armazenado - CanBeCanceled: {parametros01.RetornoCancellationTokenBreak.CanBeCanceled}");
            Console.WriteLine($"Exemplo01Contador Token Break01 armazenado - HashCode: {parametros01.RetornoCancellationTokenBreak.GetHashCode()}");

            var cts02 = new CancellationTokenSource();
            var ctsBreak02 = new CancellationTokenSource();
            ITaskParametros parametros02 = new TaskParametros();
            parametros02.ArmazenaCancellationToken(cts02.Token);
            parametros02.ArmazenaCancellationTokenBreak(ctsBreak02.Token);

            // ✅ DIAGNÓSTICO 1: Verificar se o token foi armazenado
            Console.WriteLine($"Exemplo02Contador Token armazenado cts02 - CanBeCanceled: {parametros02.RetornoCancellationToken.CanBeCanceled}");
            Console.WriteLine($"Exemplo02Contador Token armazenado cts02 - HashCode: {parametros02.RetornoCancellationToken.GetHashCode()}");
            Console.WriteLine($"Exemplo02Contador Token Break02 armazenado - CanBeCanceled: {parametros02.RetornoCancellationTokenBreak.CanBeCanceled}");
            Console.WriteLine($"Exemplo02Contador Token Break02 armazenado - HashCode: {parametros02.RetornoCancellationTokenBreak.GetHashCode()}");

            var grupo = new TasksGrupos("Exemplos");
            await grupo.AdicionarTask(1, Exemplo01Contador, parametros01, "Exemplo01Contador");
            await grupo.AdicionarTask(2, Exemplo02Contador, parametros02, "Exemplo02Contador");
            grupo.IniciarExecucao();

            await Task.Delay(1200);

            // ✅ DIAGNÓSTICO 2: Verificar antes de cancelar
            Console.WriteLine($"Exemplo01Contador Antes de cancelar - IsCancellationRequested: {cts01.Token.IsCancellationRequested}");
            Console.WriteLine($"Exemplo01Contador Antes de cancelar Break - IsCancellationRequested: {ctsBreak01.Token.IsCancellationRequested}");

            cts01.Cancel();
            
            // ✅ DIAGNÓSTICO 3: Verificar após cancelar
            Console.WriteLine($"Exemplo01Contador Após cancelar - IsCancellationRequested: {cts01.Token.IsCancellationRequested}");
            Console.WriteLine($"Exemplo01Contador Após cancelar - Token original HashCode: {cts01.Token.GetHashCode()}");
            Console.WriteLine($"Exemplo01Contador Após cancelar Break - IsCancellationRequested: {ctsBreak01.Token.IsCancellationRequested}");
            Console.WriteLine($"Exemplo01Contador Após cancelar Break - Token original HashCode: {ctsBreak01.Token.GetHashCode()}");

            await Task.Delay(10800);

            // ✅ DIAGNÓSTICO 2: Verificar antes de cancelar
            Console.WriteLine($"Exemplo02Contador Antes de cancelar - IsCancellationRequested: {cts02.Token.IsCancellationRequested}");
            Console.WriteLine($"Exemplo02Contador Antes de cancelar Break - IsCancellationRequested: {ctsBreak02.Token.IsCancellationRequested}");

            ctsBreak02.Cancel();

            // ✅ DIAGNÓSTICO 3: Verificar após cancelar
            Console.WriteLine($"Exemplo02Contador Após cancelar - IsCancellationRequested: {cts02.Token.IsCancellationRequested}");
            Console.WriteLine($"Exemplo02Contador Após cancelar - Token original HashCode: {cts02.Token.GetHashCode()}");
            Console.WriteLine($"Exemplo02Contador Após cancelar Break - IsCancellationRequested: {ctsBreak02.Token.IsCancellationRequested}");
            Console.WriteLine($"Exemplo02Contador Após cancelar Break - Token original HashCode: {ctsBreak02.Token.GetHashCode()}");

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
                    Console.WriteLine("Exemplo01Contador Ciclo {ciclo}");
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
                Console.WriteLine($"Exemplo01Contador Exceção: {ex.Message}");
            }

            return new TaskReturnValue();
        }

        public static async Task<ITaskReturnValue> Exemplo02Contador(ITaskParametros parametros)
        {
            var cancellationToken = parametros.RetornoCancellationToken;
            var cancellationTokenBreak = parametros.RetornoCancellationTokenBreak;

            Console.WriteLine($"Exemplo02Contador Token normal - CanBeCanceled: {cancellationToken.CanBeCanceled}");
            Console.WriteLine($"Exemplo02Contador Token break - CanBeCanceled: {cancellationTokenBreak.CanBeCanceled}");

            // 🔗 Mantém os tokens separados (não usaremos linked)
            bool cancelamentoBreakDetectado = false;

            // ⚡ Reação imediata ao cancelamento "Break"
            cancellationTokenBreak.Register(() =>
            {
                cancelamentoBreakDetectado = true;
                Console.WriteLine("⚠️ Cancelamento BREAK detectado forçando interrupção...");
            });

            try
            {
                int ciclo = 0;
                Console.WriteLine("🚀 Exemplo02Contador iniciado.");

                // 🔄 Loop principal controlado pelo token normal
                while (!cancellationToken.IsCancellationRequested)
                {
                    ciclo++;

                    // ✅ Cancelamento BREAK (forte) no início de cada loop
                    if (cancelamentoBreakDetectado || cancellationTokenBreak.IsCancellationRequested)
                    {
                        Console.WriteLine("🧨 Interrompendo imediatamente via.");
                        throw new OperationCanceledException(cancellationTokenBreak);
                    }

                    Console.WriteLine($"Exemplo02Contador Ciclo {ciclo}");
                    Console.WriteLine(" - Iniciando contador de 1 a 50...");

                    // 🔁 Loop interno com checagem leve e delays canceláveis
                    for (int i = 0; i <= 50; i++)
                    {
                        // 👀 Checa o break token a cada iteração também
                        if (cancelamentoBreakDetectado || cancellationTokenBreak.IsCancellationRequested)
                        {
                            Console.WriteLine($"🛑 BREAK detectado durante contagem (Ciclo {ciclo}, i={i})");
                            throw new OperationCanceledException(cancellationTokenBreak);
                        }

                        Console.Write($"Exemplo02Contador Ciclo: {ciclo} Contador: {i}");
                        Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
                        Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");

                        // ⏱️ O delay respeita cancelamento normal, e também o break (duas camadas)
                        await Task.Delay(100, cancellationToken);
                    }

                    Console.WriteLine($"✅ Ciclo {ciclo} concluído;");
                }

                Console.WriteLine("🟢 Exemplo02Contador finalizado normalmente (cancelamento normal ou fim natural).");
            }
            catch (OperationCanceledException ex)
            {
                // Captura cancelamento BREAK com distinção
                if (ex.CancellationToken == cancellationTokenBreak)
                {
                    Console.WriteLine("🟥 Exemplo02Contador cancelado via BREAK (parada brusca controlada).");
                }
                else
                {
                    Console.WriteLine("🟠 Exemplo02Contador cancelado via token normal.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exemplo02Contador exceção: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("🔚 Exemplo02Contador liberando recursos e encerrando.");
            }

            return new TaskReturnValue();
        }

        //public static async Task<ITaskReturnValue> Exemplo02Contador(ITaskParametros parametros)
        //{
        //    var cancellationToken = parametros.RetornoCancellationToken;
        //    var cancellationTokenBreak = parametros.RetornoCancellationTokenBreak;

        //    // ✅ DIAGNÓSTICO 4: Verificar token recebido
        //    Console.WriteLine($"Exemplo02Contador Token recebido - CanBeCanceled: {cancellationToken.CanBeCanceled}");
        //    Console.WriteLine($"Exemplo02Contador Token recebido - HashCode: {cancellationToken.GetHashCode()}");
        //    Console.WriteLine($"Exemplo02Contador Token Break recebido - CanBeCanceled: {cancellationTokenBreak.CanBeCanceled}");
        //    Console.WriteLine($"Exemplo02Contador Token Break recebido - HashCode: {cancellationTokenBreak.GetHashCode()}");

        //    try
        //    {
        //        var ciclo = 0;
        //        cancellationTokenBreak.ThrowIfCancellationRequested();
        //        while (!cancellationToken.IsCancellationRequested)
        //        {
        //            ciclo++;
        //            Console.WriteLine($"Exemplo02Contador Ciclo {ciclo}");
        //            Console.WriteLine(" - Iniciando contador de 1 a 50...");

        //            for (int i = 0; i <= 50; i++)
        //            {
        //                // ✅ DIAGNÓSTICO 5: Verificar a cada iteração
        //                Console.Write($"Exemplo02Contador Ciclo: {ciclo} Contador: {i}");
        //                Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
        //                Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");
        //                await Task.Delay(100);
        //            }
        //        }
        //        Console.Write("Exemplo02Contador Task cancelada.");
        //        Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
        //        Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Console.Write("Exemplo02Contador Task cancelada.");
        //        Console.Write($" - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");
        //        Console.WriteLine($" - IsCancellationRequestedBreak: {cancellationTokenBreak.IsCancellationRequested}");
        //    }

        //    return new TaskReturnValue();
        //}
    }
}
