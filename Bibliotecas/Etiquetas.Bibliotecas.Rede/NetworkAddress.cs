using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Rede
{
    public static class NetworkAddress
    {
        /// <summary>
        /// Tenta criar um IPEndPoint a partir de um endereço IP ou nome de host e uma porta.
        /// </summary>
        /// <param name="addressString">O endereço IP (ex: "127.0.0.1") ou nome de host (ex: "localhost").</param>
        /// <param name="port">A porta numérica.</param>
        /// <param name="ipEndPoint">O IPEndPoint criado, se bem-sucedido.</param>
        /// <returns>True se o IPEndPoint foi criado com sucesso, False caso contrário.</returns>
        public static IPEndPoint TryCreateIpEndPoint(string addressString, int port)
        {
            IPEndPoint ipEndPoint = null;

            // 1. Validar a porta
            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentException($"Erro: A porta de rede {port} está fora do intervalo válido ({IPEndPoint.MinPort}-{IPEndPoint.MaxPort}).");
            }

            IPAddress ipAddress = null;

            // 2. Tentar parsear como IP numérico primeiro
            if (IPAddress.TryParse(addressString, out ipAddress))
            {
                // Se for um IP numérico válido, podemos criar o IPEndPoint diretamente
                ipEndPoint = new IPEndPoint(ipAddress, port);
                return ipEndPoint;
            }
            else
            {
                // Se não for um IP numérico, tentar resolver como nome de host
                try
                {
                    // Dns.GetHostEntry pode retornar múltiplos IPs para um nome de host.
                    // Geralmente, pegamos o primeiro IPv4 ou IPv6 disponível.
                    IPHostEntry hostEntry = Dns.GetHostEntry(addressString);

                    // Preferimos IPv4, mas IPv6 também é uma opção.
                    foreach (IPAddress address in hostEntry.AddressList)
                    {
                        if (address.AddressFamily == AddressFamily.InterNetwork || address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            ipAddress = address;
                            break; // Pegamos o primeiro IP válido encontrado
                        }
                    }

                    if (ipAddress != null)
                    {
                        ipEndPoint = new IPEndPoint(ipAddress, port);
                        return ipEndPoint;
                    }
                    else
                    {
                        throw new ArgumentException($"Erro: IP ou Nome Host '{addressString}' inválido.");
                    }
                }
                catch (SocketException ex)
                {
                    throw new Exception($"Erro de DNS ao tentar resolver '{addressString}' SocketException: {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro inesperado ao tentar resolver '{addressString}': {ex.Message}", ex);
                }
            }
        }

    }
}
