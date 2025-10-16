using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.Comum.Caracteres;
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
            Exemplo1();
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
            var dados = "TESTE ENVIO DE DADOS";
            parametrosEnvio.Armazena<string>(dados, "Dados");
            parametrosEnvio.Armazena<int>(5000, "Timeout");
            parametrosEnvio.Armazena<int>(4096, "BufferSize");
            parametrosEnvio.Armazena<Encoding>(ConversaoEncoding.UTF8BOM, "Encoding");
            parametrosEnvio.Armazena<CancellationToken>(this.CancellationBruto, "CancellationBruto");
            await this.TcpCliente.EscreverAsync<string>(parametrosEnvio).ConfigureAwait(false);
            Console.WriteLine($"Mensagem {dados} enviada ao servidor.");
            // Aguarda um pouco para garantir que o servidor tenha tempo de responder

            Console.WriteLine("Aguardar 5 segundos.");
            await Task.Delay(1000).ConfigureAwait(false);

            Console.WriteLine("Aguardar 4 segundos.");
            await Task.Delay(1000).ConfigureAwait(false);

            Console.WriteLine("Aguardar 3 segundos.");
            await Task.Delay(1000).ConfigureAwait(false);

            Console.WriteLine("Aguardar 2 segundos.");
            await Task.Delay(1000).ConfigureAwait(false);

            Console.WriteLine("Aguardar 1 segundos.");
            await Task.Delay(1000).ConfigureAwait(false);

            Console.WriteLine("Possui dados?");
            if (this.TcpCliente.PossuiDados())
            {
                var parametrosRecepcao = new TaskParametros(4);
                parametrosRecepcao.Armazena<int>(5000, "Timeout");
                parametrosRecepcao.Armazena<int>(4096, "BufferSize");
                parametrosRecepcao.Armazena<Encoding>(ConversaoEncoding.UTF8BOM, "Encoding");
                parametrosRecepcao.Armazena<CancellationToken>(this.CancellationBruto, "CancellationBruto");

                var resposta = await this.TcpCliente.LerAsync<string>(parametrosRecepcao).ConfigureAwait(false);
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
