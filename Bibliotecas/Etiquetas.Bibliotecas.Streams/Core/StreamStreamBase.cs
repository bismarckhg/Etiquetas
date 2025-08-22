using System;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.Streams.Interfaces;

namespace Etiquetas.Bibliotecas.Streams.Core
{
    /// <summary>
    /// Fornece uma implementação base para as classes de stream, cuidando do gerenciamento de descarte (Dispose).
    /// </summary>
    public abstract class StreamStreamBase : IStreamConexao
    {
        private bool stDisposed = false;

        /// <summary>
        /// Implementação virtual do método EstaAberto. As classes derivadas devem sobrescrevê-lo.
        /// </summary>
        public virtual bool EstaAberto() => false;

        /// <summary>
        /// Implementação virtual do método PossuiDados. As classes derivadas devem sobrescrevê-lo.
        /// </summary>
        public virtual bool PossuiDados() => false;

        /// <summary>
        /// Implementação virtual do método ConectarAsync. As classes derivadas podem sobrescrevê-lo.
        /// </summary>
        /// <param name="parametros">Parâmetros necessários para a conexão, como endereço IP e porta.</param>
        /// <returns>Uma tarefa que representa a operação de conexão.</returns>
        public virtual Task ConectarAsync(params object[] parametros)
        {
            // No-op by default
            return Task.FromResult(0);
        }

        /// <summary>
        /// Implementação virtual do método FecharAsync. As classes derivadas podem sobrescrevê-lo.
        /// </summary>
        public virtual Task FecharAsync()
        {
            // No-op by default
            return Task.FromResult(0);
        }

        /// <summary>
        /// Realiza a liberação de recursos.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Método protegido para que as classes derivadas possam implementar a lógica de descarte.
        /// </summary>
        /// <param name="disposing">Indica se a chamada vem do método Dispose().</param>
        protected virtual void Dispose(bool disposing)
        {
            if (stDisposed) return;

            if (disposing)
            {
                // Libera recursos gerenciados aqui
            }

            // Libera recursos não gerenciados aqui

            stDisposed = true;
        }

        /// <summary>
        /// Finalizador (Destrutor) para garantir a liberação de recursos não gerenciados.
        /// </summary>
        ~StreamStreamBase()
        {
            Dispose(false);
        }
    }
}
