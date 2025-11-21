using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Rede
{
    public class EnderecoRede
    {
        protected string EnderecoIPOuNomeHost { get; }
        protected int Porta { get; }
        protected IPEndPoint EnderecoRedeIpEndPoint { get; }
        protected EndPoint ParteEndPoint { get; }

        protected string IPePortaRede { get; }
        protected IPAddress EnderecoIP { get; }

        public EnderecoRede(EndPoint endPoint)
        {
            this.ParteEndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint), "Erro: EndPoint não pode ser nulo.");
            if (endPoint is IPEndPoint ipEndPoint)
            {
                this.EnderecoRedeIpEndPoint = ipEndPoint;
                this.EnderecoIPOuNomeHost = ipEndPoint.Address.ToString();
                this.Porta = ipEndPoint.Port;
                this.EnderecoIP = ipEndPoint.Address;
                this.IPePortaRede = $"{this.EnderecoIPOuNomeHost}:{this.Porta.ToString()}";
            }
            else
            {
                throw new ArgumentException("Erro: Apenas IPEndPoint é suportado atualmente.", nameof(endPoint));
            }
        }

        /// <summary>
        /// Tenta criar um IPEndPoint a partir de um endereço IP ou nome de host e uma porta.
        /// </summary>
        /// <param name="addressString">O endereço IP (ex: "127.0.0.1") ou nome de host (ex: "localhost").</param>
        /// <param name="port">A porta numérica.</param>
        public EnderecoRede(string addressString, int port)
        {
            this.EnderecoIPOuNomeHost = addressString;
            this.Porta = port;

            this.EnderecoRedeIpEndPoint = null;

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
                this.EnderecoRedeIpEndPoint = new IPEndPoint(ipAddress, port);
                this.EnderecoIP = ipAddress;
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
                        this.EnderecoRedeIpEndPoint = new IPEndPoint(ipAddress, port);
                        this.EnderecoIP = ipAddress;
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
                this.IPePortaRede = $"{this.EnderecoRedeIpEndPoint.Address.ToString()}:{port.ToString()}";

            }
        }

        public string ObtemEnderecoIPOuNomeHost()
        {
            return this.EnderecoIPOuNomeHost;
        }

        public int ObtemPorta()
        {
            return this.Porta;
        }

        public IPEndPoint ObtemEnderecoRedeIpEndPoint()
        {
            return this.EnderecoRedeIpEndPoint;
        }

        public string ObtemIPePortaRede()
        {
            return this.IPePortaRede;
        }

        public IPAddress ObtemEnderecoIP()
        {
            return this.EnderecoIP;
        }

    }
}
