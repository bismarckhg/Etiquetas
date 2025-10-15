using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.Comum.Geral;
using Etiquetas.Bibliotecas.TaskCore;
using Etiquetas.Bibliotecas.TCPCliente;

namespace Etiquetas.Bibliotecas.TCPCliente.Exemplo
{
    public class ExemploStreamTCPClient
    {
        private StreamTCPCliente TcpCliente;

        private CancellationToken CancellationBruto;
        private CancellationToken CancellationStop;

        public ExemploStreamTCPClient()
        {
        }

        public async Task ExecutarTodosExemplos()
        {

        }

        public async Task Exemplo1()
        {
            this.CancellationBruto = new CancellationTokenSource().Token;
            this.CancellationStop = new CancellationTokenSource().Token;

            this.TcpCliente = new StreamTCPCliente();

            var parametros = new TaskParametros(4);
            parametros.Armazena<string>("127.0.0.1", "ServerIpAdress");
            parametros.Armazena<int>(9100, "ServerPort");
            parametros.Armazena<int>(5000, "Timeout");
            parametros.Armazena<CancellationToken>(this.CancellationBruto, "CancellationBruto");

            await this.TcpCliente.ConectarAsync(parametros).ConfigureAwait(false);

            if (!this.TcpCliente.EstaAberto())
            {
                throw new Exception("Não foi possível abrir a conexão TCP.");
            }

            Console.WriteLine("Conectado com sucesso ao servidor TCP.");

            var parametrosEnvio = new TaskParametros(5);
            parametrosEnvio.Armazena<string>("Olá, servidor TCPListerner!", "Dados");
            parametrosEnvio.Armazena<int>(5000, "Timeout");
            parametrosEnvio.Armazena<int>(4096, "BufferSize");
            parametrosEnvio.Armazena<Encoding>(ConversaoEncoding.UTF8BOM, "Encoding");
            parametrosEnvio.Armazena<CancellationToken>(this.CancellationBruto, "CancellationBruto");
            await this.TcpCliente.EscreverAsync<string>(parametrosEnvio).ConfigureAwait(false);

            Console.WriteLine("Mensagem enviada ao servidor.");
            // Aguarda um pouco para garantir que o servidor tenha tempo de responder
            await Task.Delay(1000).ConfigureAwait(false);
            if (this.TcpCliente.PossuiDados())
            {
                var parametrosRecepcao = new TaskParametros(4);
                parametrosRecepcao.Armazena<int>(5000, "Timeout");
                parametrosRecepcao.Armazena<int>(4096, "BufferSize");
                parametrosRecepcao.Armazena<Encoding>(ConversaoEncoding.UTF8BOM, "Encoding");
                parametrosRecepcao.Armazena<CancellationToken>(this.CancellationBruto, "CancellationBruto");


                int bytesLidos = await this.TcpCliente.ReceberAsync(bufferRecepcao, 0, bufferRecepcao.Length, this.CancellationBruto).ConfigureAwait(false);
                string resposta = Encoding.UTF8.GetString(bufferRecepcao, 0, bytesLidos);
                Console.WriteLine($"Resposta do servidor: {resposta}");
            }
            else
            {
                Console.WriteLine("Nenhum dado disponível para leitura.");
            }
            await this.TcpCliente.FecharAsync().ConfigureAwait(false);
            Console.WriteLine("Conexão fechada.");
        }
    }
}
