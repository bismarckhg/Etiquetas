using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TCPCliente.Exemplo
{
    public static class ExemploTCPConnector
    {

        // Exemplo de uso
        public static async Task Exemplo()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1"); // Ou IPAddress.Loopback
            int port = 12345; // Uma porta que provavelmente não terá um servidor escutando
            //string ipString = "127.0.0.1";

            var connector = new TcpConnector(ip, port);

            Console.WriteLine($"Tentando conectar a {ip.ToString()}:{port} com timeout de 2000ms...");
            try
            {
                // Para testar o timeout, use uma porta onde não há servidor.
                // Para testar sucesso, inicie um servidor simples na porta 12345.
                using (TcpClient client = await connector.ConnectWithTimeoutAsync(new CancellationTokenSource().Token, 2000))
                {
                    Console.WriteLine($"Conectado com sucesso a {client.Client.RemoteEndPoint}");
                    // Faça algo com o cliente
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Erro de Timeout: {ex.Message}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Erro de Socket: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }

            // Exemplo de conexão rápida (para não dar timeout)
            Console.WriteLine($"Tentando conectar a {ip.ToString()}:80 (HTTP) com timeout de 5000ms...");
            try
            {
                // Tentar conectar a uma porta comum que pode estar aberta (ex: HTTP)
                // Isso pode falhar se não houver um servidor HTTP na máquina local
                // ou se o firewall bloquear.
                var connector2 = new TcpConnector(ip, 80);
                using (TcpClient client = await connector2.ConnectWithTimeoutAsync(new CancellationTokenSource().Token, 5000))
                {
                    Console.WriteLine($"Conectado com sucesso a {client.Client.RemoteEndPoint}");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Erro de Timeout: {ex.Message}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Erro de Socket: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }
    }
}
