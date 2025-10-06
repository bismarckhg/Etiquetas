using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Interfaces
{
    /// <summary>
    /// Define o contrato para um gerenciador de Mutex global.
    /// </summary>
    public interface IGerenciadorMutex : IDisposable
    {
        /// <summary>
        /// Tenta adquirir o lock do Mutex.
        /// </summary>
        /// <param name="timeout">O tempo máximo de espera para adquirir o lock.</param>
        /// <returns>True se o lock foi adquirido, False caso contrário.</returns>
        bool Adquirir(TimeSpan timeout);

        /// <summary>
        /// Libera o lock do Mutex.
        /// </summary>
        void Liberar();
    }
}
