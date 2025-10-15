using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.TaskCore;
using Etiquetas.Bibliotecas.TCPCliente;

namespace Etiquetas.Bibliotecas.TCPCliente.Exemplo
{
    public class ExemploStreamTCPClient
    {
        private readonly StreamTCPCliente TcpCliente;

        public ExemploStreamTCPClient()
        {
                TcpCliente = new StreamTCPCliente();
        }

        public async Task ExecutarTodosExemplos()
        {

        }

        public async Task Exemplo1()
        {
            var parametros = new TaskParametros(4);
            parametros.Armazena<string>("127.0.0.1", "ServerIpAdress");
            parametros.Armazena<int>(9100, "ServerPort");
        }
    }
}
