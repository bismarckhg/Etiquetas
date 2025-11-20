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

            try
            {
                var endPoint1 = NetworkAddress.TryCreateIpEndPoint(serverIpAdress1, serverPort1);
                Console.WriteLine($"IPEndPoint1 criado com sucesso: {endPoint1}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Falha ao criar IPEndPoint para host inexistente '{serverIpAdress1}:{serverPort1}'.");
            }

            string serverIpAdress2 = "::1"; // Loopback IPv6
            int serverPort2 = 9000;
            try
            {
                var endPoint2 = NetworkAddress.TryCreateIpEndPoint(serverIpAdress2, serverPort2);
                Console.WriteLine($"IPEndPoint2 criado com sucesso: {endPoint2}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Falha ao criar IPEndPoint para host inexistente '{serverIpAdress2}:{serverPort2}'.");
            }

            // Testes com nomes de host
            string serverHostName1 = "localhost";
            int serverPort3 = 5000;
            try
            {
                var endPoint3 = NetworkAddress.TryCreateIpEndPoint(serverHostName1, serverPort3);
                Console.WriteLine($"IPEndPoint3 criado com sucesso: {endPoint3}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Falha ao criar IPEndPoint para host inexistente '{serverHostName1}:{serverPort3}'.");
            }

            string serverHostName2 = "google.com"; // Exemplo de um host externo
            int serverPort4 = 80;
            try
            {
                var endPoint4 = NetworkAddress.TryCreateIpEndPoint(serverHostName2, serverPort4);
                Console.WriteLine($"IPEndPoint4 criado com sucesso: {endPoint4}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Falha ao criar IPEndPoint para host inexistente '{serverHostName2}:{serverPort4}'.");
            }

            // Testes com entradas inválidas
            string invalidIpAdress1 = "192.168.1.256"; // IP inválido
            int serverPort5 = 1234;
            try
            {
                var endPoint5 = NetworkAddress.TryCreateIpEndPoint(invalidIpAdress1, serverPort5);
                Console.WriteLine($"IPEndPoint5 criado com sucesso: {endPoint5}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Falha ao criar IPEndPoint para host inexistente '{invalidIpAdress1}:{serverPort5}'.");
            }

            string invalidIpAdress2 = "not-an-ip"; // String inválida
            int serverPort6 = 5678;
            try
            {
                var endPoint6 = NetworkAddress.TryCreateIpEndPoint(invalidIpAdress2, serverPort6);
                Console.WriteLine($"IPEndPoint6 criado com sucesso: {endPoint6}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Falha ao criar IPEndPoint para '{invalidIpAdress2}:{serverPort6}'.");
            }

            string validIpAdress3 = "10.0.0.1";
            int invalidPort1 = 70000; // Porta fora do range
            try
            {
                var endpoint7 = NetworkAddress.TryCreateIpEndPoint(validIpAdress3, invalidPort1);
                Console.WriteLine($"IPEndPoint6 criado com sucesso: {endpoint7}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Falha esperada ao criar IPEndPoint para porta inválida '{validIpAdress3}:{invalidPort1}'.");
            }

            string nonExistentHost = "this-host-does-not-exist-12345.com";
            int serverPort8 = 10000;
            try
            {
                var endPoint8 = NetworkAddress.TryCreateIpEndPoint(nonExistentHost, serverPort8);
                Console.WriteLine($"IPEndPoint6 criado com sucesso: {endPoint8}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Falha esperada ao criar IPEndPoint para host inexistente '{nonExistentHost}:{serverPort8}'.");
            }
        }
    }
}
