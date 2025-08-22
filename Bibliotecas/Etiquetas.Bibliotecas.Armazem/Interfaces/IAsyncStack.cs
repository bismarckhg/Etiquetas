using System;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Armazem.Interfaces
{
    /// <summary>
    /// Interface para stack assíncrono thread-safe seguindo padrão FIFO
    /// </summary>
    /// <typeparam name="T">Tipo dos itens no stack</typeparam>
    public interface IAsyncStack<T>
    {
        /// <summary>
        /// Adiciona um item ao stack de forma assíncrona
        /// </summary>
        /// <param name="item">Item a ser adicionado</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        Task EnqueueAsync(T item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove e retorna um item do stack de forma assíncrona
        /// </summary>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Item removido do stack</returns>
        Task<T> DequeueAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica se o stack tem itens
        /// </summary>
        bool HasItems { get; }

        /// <summary>
        /// Quantidade de itens no stack
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Evento disparado quando um novo item é adicionado
        /// </summary>
        event EventHandler<T> ItemAdded;
    }
}

