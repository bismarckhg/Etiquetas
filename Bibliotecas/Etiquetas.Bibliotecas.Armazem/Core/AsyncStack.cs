using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.Armazem.Interfaces;

namespace Etiquetas.Bibliotecas.Armazem.Core
{
    /// <summary>
    /// Implementação thread-safe de stack assíncrono FIFO
    /// Segue o princípio da Responsabilidade Única (SRP) e Interface Segregation (ISP)
    /// </summary>
    /// <typeparam name="T">Tipo dos itens no stack</typeparam>
    public class AsyncStack<T> : IAsyncStack<T>, IDisposable
    {
        private readonly ConcurrentQueue<T> _queue;
        private readonly SemaphoreSlim _semaphore;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private volatile bool _disposed;

        public event EventHandler<T> ItemAdded;

        /// <summary>
        /// Verifica se o stack tem itens
        /// </summary>
        public bool HasItems => !_queue.IsEmpty;

        /// <summary>
        /// Quantidade de itens no stack
        /// </summary>
        public int Count => _queue.Count;

        /// <summary>
        /// Construtor do AsyncStack
        /// </summary>
        public AsyncStack()
        {
            _queue = new ConcurrentQueue<T>();
            _semaphore = new SemaphoreSlim(0);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Adiciona um item ao stack de forma assíncrona
        /// </summary>
        /// <param name="item">Item a ser adicionado</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        public async Task EnqueueAsync(T item, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            try
            {
                // Executa em uma task para manter a assincronia
                await Task.Run(() =>
                {
                    _queue.Enqueue(item);
                    _semaphore.Release(); // Sinaliza que há um item disponível
                }, cancellationToken).ConfigureAwait(false);

                // Dispara evento de forma assíncrona
                await Task.Run(() => OnItemAdded(item), cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Operação cancelada, não faz nada
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao adicionar item ao stack: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Remove e retorna um item do stack de forma assíncrona
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Item removido do stack</returns>
        public async Task<T> DequeueAsync(CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            try
            {
                // Combina o token de cancelamento interno com o externo
                using (var combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                    _cancellationTokenSource.Token, cancellationToken))
                {
                    // Aguarda até que haja um item disponível
                    await _semaphore.WaitAsync(combinedTokenSource.Token);

                    // Tenta remover um item da fila
                    if (_queue.TryDequeue(out T item))
                    {
                        return item;
                    }

                    // Se chegou aqui, algo deu errado
                    throw new InvalidOperationException("Falha ao remover item do stack");
                }
            }
            catch (OperationCanceledException)
            {
                // Operação cancelada, não faz nada
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao remover item do stack: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tenta remover um item sem aguardar (não-bloqueante)
        /// </summary>
        /// <param name="item">Item removido, se houver</param>
        /// <returns>True se conseguiu remover um item, False caso contrário</returns>
        public bool TryDequeue(out T item)
        {
            ThrowIfDisposed();

            if (_queue.TryDequeue(out item))
            {
                // Reduz o contador do semáforo se conseguiu remover
                _semaphore.Wait(0);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Limpa todos os itens do stack
        /// </summary>
        public async Task ClearAsync()
        {
            ThrowIfDisposed();

            await Task.Run(() =>
            {
                while (_queue.TryDequeue(out _))
                {
                    // Remove todos os itens
                }

                // Reseta o semáforo
                while (_semaphore.CurrentCount > 0)
                {
                    _semaphore.Wait(0);
                }
            });
        }

        /// <summary>
        /// Dispara o evento ItemAdded de forma thread-safe
        /// </summary>
        /// <param name="item">Item adicionado</param>
        protected virtual void OnItemAdded(T item)
        {
            try
            {
                ItemAdded?.Invoke(this, item);
            }
            catch (Exception ex)
            {
                // Log do erro, mas não propaga para não quebrar o fluxo
                System.Diagnostics.Debug.WriteLine($"Erro ao disparar evento ItemAdded: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica se o objeto foi descartado
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(AsyncStack<T>));
        }

        /// <summary>
        /// Libera os recursos utilizados
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera os recursos utilizados
        /// </summary>
        /// <param name="disposing">Indica se está sendo chamado pelo Dispose</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _cancellationTokenSource?.Cancel();
                _semaphore?.Dispose();
                _cancellationTokenSource?.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~AsyncStack()
        {
            Dispose(false);
        }
    }
}

