using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.GUI.Texto2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(2000);
            //var cancel = new CancellationTokenSource().Token;
            //CancellationToken cancel2 = default;

            //if (cancel == default)
            //{
            //    Console.WriteLine("cancel é default");
            //}

            //if (cancel2 == default)
            //{
            //    Console.WriteLine("cancel2 é default");
            //}

            //cancel2 = new CancellationTokenSource().Token;


            //if (cancel2 != default)
            //{
            //    Console.WriteLine("cancel2 não é default");
            //}

            //await TCPListernerSimulador.Execute();
            await ExemploTASK.ExecutaTodosExemplos();

            //var cancel1 = new CancellationTokenSource();
            //var cancel2 = new CancellationTokenSource();
            //var ctsGrupo = new CancellationTokenSource();

            //ctsGrupo.Token.Register(() =>
            //{
            //    cancel1.Cancel();
            //});

            //ctsGrupo.Token.Register(() =>
            //{
            //    cancel2.Cancel();
            //});

            //Console.WriteLine($"Cancel1 {cancel1.Token.IsCancellationRequested}");
            //Console.WriteLine($"Cancel2 {cancel2.Token.IsCancellationRequested}");
            //Console.WriteLine($"ctsGrupo {ctsGrupo.Token.IsCancellationRequested}");

            ////ctsGrupo.Cancel();
            //cancel1.Cancel();

            //Console.WriteLine($"Cancel1 {cancel1.Token.IsCancellationRequested}");
            //Console.WriteLine($"Cancel2 {cancel2.Token.IsCancellationRequested}");
            //Console.WriteLine($"ctsGrupo {ctsGrupo.Token.IsCancellationRequested}");

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}