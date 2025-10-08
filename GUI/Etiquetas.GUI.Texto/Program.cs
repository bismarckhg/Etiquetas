using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.Xml.Exemplo;

namespace Etiquetas.GUI.Texto
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var examples = new ExemploGenericXmlService();
            await examples.ExecutarTodosExemplos();

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
