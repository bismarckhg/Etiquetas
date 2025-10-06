using System;
#if NET472
using System.Security.AccessControl;
#endif
using System.IO.Pipes;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Comunicacao
{
    /// <summary>
    /// Gerencia a conexão de baixo nível para um pipe nomeado.
    /// </summary>
    public class GerenciadorConexaoPipe : IDisposable
    {
        private NamedPipeServerStream _pipeServerStream;
        private readonly string _nomePipe;
        private readonly int _maximoNumeroServidores;

        /// <summary>
        /// Indica se o pipe está atualmente conectado.
        /// </summary>
        public bool EstaConectado => _pipeServerStream?.IsConnected ?? false;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="GerenciadorConexaoPipe"/>.
        /// </summary>
        /// <param name="nomePipe">O nome do pipe para a conexão.</param>
        /// <param name="maximoServidores">O número máximo de servidores permitidos para o pipe.</param>
        public GerenciadorConexaoPipe(string nomePipe, int maximoServidores = 1)
        {
            _nomePipe = nomePipe;
            _maximoNumeroServidores = maximoServidores;
        }

        /// <summary>
        /// Inicia o servidor de pipe e aguarda por uma conexão de cliente.
        /// </summary>
        /// <param name="cancellationToken">Token para cancelar a espera por conexão.</param>
        /// <returns>O stream do pipe conectado.</returns>
        public async Task<NamedPipeServerStream> AguardarConexaoAsync(CancellationToken cancellationToken)
        {
#if NET472
            // Garante que o pipe possa ser acessado por outros usuários, se necessário.
            var pipeSecurity = new PipeSecurity();
            pipeSecurity.AddAccessRule(new PipeAccessRule(
                new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                PipeAccessRights.ReadWrite,
                System.Security.AccessControl.AccessControlType.Allow));

            _pipeServerStream = new NamedPipeServerStream(
                _nomePipe,
                PipeDirection.InOut,
                _maximoNumeroServidores,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous,
                4096, // Buffer de entrada
                4096, // Buffer de saída
                pipeSecurity);
#else
            _pipeServerStream = new NamedPipeServerStream(
                _nomePipe,
                PipeDirection.InOut,
                _maximoNumeroServidores,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);
#endif

            await _pipeServerStream.WaitForConnectionAsync(cancellationToken);
            return _pipeServerStream;
        }

        /// <summary>
        /// Desconecta o cliente atual e fecha o stream do pipe.
        /// </summary>
        public void Desconectar()
        {
            if (_pipeServerStream != null && _pipeServerStream.IsConnected)
            {
                _pipeServerStream.Disconnect();
            }
            Dispose();
        }

        /// <summary>
        /// Libera os recursos do pipe.
        /// </summary>
        public void Dispose()
        {
            _pipeServerStream?.Dispose();
            _pipeServerStream = null;
        }
    }
}
