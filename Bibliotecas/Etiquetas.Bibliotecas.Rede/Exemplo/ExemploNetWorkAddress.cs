using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.Rede;

namespace Etiquetas.Bibliotecas.Rede.Exemplo
{
    public static class ExemploNetWorkAddress
    {

        public static void Exemplo()
        {
            // Testes com IPs válidos
            string serverIpAdress1 = "127.0.0.1"; // Loopback IPv4
            int serverPort1 = 8080;
            if (NetworkAddress.TryCreateIpEndPoint(serverIpAdress1, serverPort1, out IPEndPoint endPoint1))
            {
                Console.WriteLine($"IPEndPoint criado com sucesso: {endPoint1}");
            }

            string serverIpAdress2 = "::1"; // Loopback IPv6
            int serverPort2 = 9000;
            if (NetworkAddress.TryCreateIpEndPoint(serverIpAdress2, serverPort2, out IPEndPoint endPoint2))
            {
                Console.WriteLine($"IPEndPoint criado com sucesso: {endPoint2}");
            }

            // Testes com nomes de host
            string serverHostName1 = "localhost";
            int serverPort3 = 5000;
            if (NetworkAddress.TryCreateIpEndPoint(serverHostName1, serverPort3, out IPEndPoint endPoint3))
            {
                Console.WriteLine($"IPEndPoint criado com sucesso: {endPoint3}");
            }

            string serverHostName2 = "google.com"; // Exemplo de um host externo
            int serverPort4 = 80;
            if (NetworkAddress.TryCreateIpEndPoint(serverHostName2, serverPort4, out IPEndPoint endPoint4))
            {
                Console.WriteLine($"IPEndPoint criado com sucesso: {endPoint4}");
            }

            // Testes com entradas inválidas
            string invalidIpAdress1 = "192.168.1.256"; // IP inválido
            int serverPort5 = 1234;
            if (!NetworkAddress.TryCreateIpEndPoint(invalidIpAdress1, serverPort5, out IPEndPoint endPoint5))
            {
                Console.WriteLine($"Falha esperada ao criar IPEndPoint para '{invalidIpAdress1}'.");
            }

            string invalidIpAdress2 = "not-an-ip"; // String inválida
            int serverPort6 = 5678;
            if (!NetworkAddress.TryCreateIpEndPoint(invalidIpAdress2, serverPort6, out IPEndPoint endPoint6))
            {
                Console.WriteLine($"Falha esperada ao criar IPEndPoint para '{invalidIpAdress2}'.");
            }

            string validIpAdress3 = "10.0.0.1";
            int invalidPort1 = 70000; // Porta fora do range
            if (!NetworkAddress.TryCreateIpEndPoint(validIpAdress3, invalidPort1, out IPEndPoint endPoint7))
            {
                Console.WriteLine($"Falha esperada ao criar IPEndPoint para porta inválida '{invalidPort1}'.");
            }

            string nonExistentHost = "this-host-does-not-exist-12345.com";
            int serverPort7 = 10000;
            if (!NetworkAddress.TryCreateIpEndPoint(nonExistentHost, serverPort7, out IPEndPoint endPoint8))
            {
                Console.WriteLine($"Falha esperada ao criar IPEndPoint para host inexistente '{nonExistentHost}'.");
            }
        }
    }
}
