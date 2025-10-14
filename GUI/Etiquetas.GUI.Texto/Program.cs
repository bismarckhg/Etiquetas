using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
            //var exemplos = new ExemploStreamXml();
            //await exemplos.ExecutarTodosExemplos();

            TcpClient tcp = default;


            if (tcp == null)
            {
                Console.WriteLine("TcpClient(tcp) está vazio");
            }

            var tcpClient = new TcpClient("127.0.0.1", 9101);

            if (tcpClient == default)
            {
                Console.WriteLine("TcpClient(tcpClient) está vazio");
            }


            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}
