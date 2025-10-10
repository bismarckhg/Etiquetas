using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.StreamsTXT.Exemplo;
using Etiquetas.Bibliotecas.Xml.Exemplo;

namespace Etiquetas.GUI.Texto
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //var examples = new ExemploStreamTxt();
            //await examples.Executar();
            //var exemplos = new ExemploGenericXmlService();
            //await exemplos.ExecutarTodosExemplos();
            var exemplos = new ExemploStreamXml();
            await exemplos.ExecutarTodosExemplos();

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
