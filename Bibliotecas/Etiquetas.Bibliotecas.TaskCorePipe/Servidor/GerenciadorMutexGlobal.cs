using Etiqueta.Bibliotecas.TaskCorePipe.Interfaces;
using System;
#if NET472
using System.Security.AccessControl;
#endif
using System.Security.Principal;
using System.Threading;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Servidor
{
    /// <summary>
    /// Implementa um Mutex global para garantir uma única instância do servidor.
    /// </summary>
    public class GerenciadorMutexGlobal : IGerenciadorMutex
    {
        private Mutex _mutex;
        private bool _mutexAdquirido;

        /// <summary>
        /// Cria uma instância do gerenciador de mutex com um nome global único.
        /// </summary>
        /// <param name="nomeMutex">O nome do mutex. Deve ser único no sistema.</param>
        public GerenciadorMutexGlobal(string nomeMutex)
        {
#if NET472
            // O prefixo "Global\" é necessário para que o mutex seja visível em todas as sessões de usuário.
            var mutexSecurity = new MutexSecurity();
            mutexSecurity.AddAccessRule(new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow));
            _mutex = new Mutex(false, $"Global\\{nomeMutex}", out _, mutexSecurity);
#else
            _mutex = new Mutex(false, $"Global\\{nomeMutex}");
#endif
        }

        /// <summary>
        /// Tenta adquirir o lock do Mutex.
        /// </summary>
        /// <param name="timeout">O tempo máximo de espera para adquirir o lock.</param>
        /// <returns>True se o lock foi adquirido, False caso contrário.</returns>
        public bool Adquirir(TimeSpan timeout)
        {
            try
            {
                _mutexAdquirido = _mutex.WaitOne(timeout, false);
                return _mutexAdquirido;
            }
            catch (AbandonedMutexException)
            {
                // O mutex foi abandonado por outro processo. Consideramos adquirido.
                _mutexAdquirido = true;
                return true;
            }
        }

        /// <summary>
        /// Libera o lock do Mutex se foi adquirido.
        /// </summary>
        public void Liberar()
        {
            if (_mutexAdquirido)
            {
                try
                {
                    _mutex.ReleaseMutex();
                }
                catch (ApplicationException)
                {
                    // Ignorar erro em cenários de teste async onde a thread pode ser diferente.
                }
                _mutexAdquirido = false;
            }
        }

        /// <summary>
        /// Libera os recursos do Mutex.
        /// </summary>
        public void Dispose()
        {
            Liberar();
            _mutex?.Dispose();
            _mutex = null;
        }
    }
}
