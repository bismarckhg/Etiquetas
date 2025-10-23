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

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
