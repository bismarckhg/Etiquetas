using Etiqueta.Bibliotecas.TaskCorePipe.Comunicacao;
using Etiqueta.Bibliotecas.TaskCorePipe.Interfaces;
using Etiqueta.Bibliotecas.TaskCorePipe.Modelos;
using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Tarefa
{
    /// <summary>
    /// Implementa a lógica de alto nível para o pipe de uma tarefa individual.
    /// </summary>
    public class GerenciadorPipeTarefa : IPipeTarefa
    {
        private readonly string _nomePipe;
        private readonly ProtocoloMensagem _protocolo;
        private CancellationTokenSource _cts;
        private Task _loopEscuta;

        /// <summary>
        /// Evento acionado quando um comando é recebido do servidor.
        /// </summary>
        public event Func<IComandoPipe, Task<object>> ComandoRecebido;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="GerenciadorPipeTarefa"/>.
        /// </summary>
        /// <param name="nomePipe">O nome do pipe que este gerenciador irá escutar.</param>
        public GerenciadorPipeTarefa(string nomePipe)
        {
            _nomePipe = nomePipe;
            _protocolo = new ProtocoloMensagem(new SerializadorJson());
            _cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Inicia o loop de escuta do pipe em uma tarefa de background.
        /// </summary>
        public Task IniciarAsync()
        {
            _loopEscuta = Task.Run(() => LoopDeEscutaAsync(_cts.Token), _cts.Token);
            return Task.CompletedTask;
        }

        private async Task LoopDeEscutaAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                using (var gerenciadorConexao = new GerenciadorConexaoPipe(_nomePipe))
                {
                    try
                    {
                        var stream = await gerenciadorConexao.AguardarConexaoAsync(token);

                        while (gerenciadorConexao.EstaConectado && !token.IsCancellationRequested)
                        {
                            var mensagem = await _protocolo.LerMensagemAsync(stream);
                            if (mensagem == null) break;

                            if (mensagem.Payload is IComandoPipe comando && ComandoRecebido != null)
                            {
                                var respostaPayload = await ComandoRecebido.Invoke(comando);

                                var mensagemResposta = new MensagemPipe
                                {
                                    IdOrigem = _nomePipe,
                                    IdDestino = mensagem.IdOrigem,
                                    Payload = respostaPayload,
                                    TipoMensagem = Enums.TipoMensagem.Response
                                };
                                await _protocolo.EscreverMensagemAsync(stream, mensagemResposta);
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception)
                    {
                        await Task.Delay(5000, token);
                    }
                }
            }
        }

        /// <summary>
        /// Sinaliza para o loop de escuta parar e aguarda sua conclusão.
        /// </summary>
        public Task PararAsync()
        {
            _cts.Cancel();
            return _loopEscuta ?? Task.CompletedTask;
        }

        /// <summary>
        /// Libera os recursos, garantindo que a tarefa de escuta seja parada.
        /// </summary>
        public void Dispose()
        {
            PararAsync().Wait();
            _cts?.Dispose();
        }
    }
}
