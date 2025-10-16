using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.GUI.Texto2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Task.Delay(2000);
            await TCPListernerSimulador.Execute();

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
