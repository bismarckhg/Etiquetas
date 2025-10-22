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
            var cts = new CancellationTokenSource();
            ITaskParametros parametros = new TaskParametros();
            parametros.ArmazenaCancellationToken(cts.Token);

            // ✅ DIAGNÓSTICO 1: Verificar se o token foi armazenado
            Console.WriteLine($"Token armazenado - CanBeCanceled: {parametros.RetornoCancellationToken.CanBeCanceled}");
            Console.WriteLine($"Token armazenado - HashCode: {parametros.RetornoCancellationToken.GetHashCode()}");

            var grupo = new TasksGrupos("Exemplos");
            await grupo.AdicionarTask(1, Exemplo01Contador, parametros, "Exemplo01Contador");
            grupo.IniciarExecucao();

            await Task.Delay(1200);

            // ✅ DIAGNÓSTICO 2: Verificar antes de cancelar
            Console.WriteLine($"Antes de cancelar - IsCancellationRequested: {cts.Token.IsCancellationRequested}");

            cts.Cancel();

            // ✅ DIAGNÓSTICO 3: Verificar após cancelar
            Console.WriteLine($"Após cancelar - IsCancellationRequested: {cts.Token.IsCancellationRequested}");
            Console.WriteLine($"Após cancelar - Token original HashCode: {cts.Token.GetHashCode()}");

            await grupo.AguardaTodasTasksAsync();
        }

        public static async Task<ITaskReturnValue> Exemplo01Contador(ITaskParametros parametros)
        {
            var cancellationToken = parametros.RetornoCancellationToken;

            // ✅ DIAGNÓSTICO 4: Verificar token recebido
            Console.WriteLine($"Token recebido - CanBeCanceled: {cancellationToken.CanBeCanceled}");
            Console.WriteLine($"Token recebido - HashCode: {cancellationToken.GetHashCode()}");

            Console.WriteLine("Iniciando contador de 1 a 50...");

            for (int i = 0; i <= 50; i++)
            {
                // ✅ DIAGNÓSTICO 5: Verificar a cada iteração
                Console.WriteLine($"Contador: {i} - IsCancellationRequested: {cancellationToken.IsCancellationRequested}");

                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Contador cancelado.");
                    break;
                }

                await Task.Delay(100);
            }

            return new TaskReturnValue();
        }
    }
}
