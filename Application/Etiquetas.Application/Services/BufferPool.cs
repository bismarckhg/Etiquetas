using System;
using System.Buffers;

namespace Etiquetas.Application.Services
{
    /// <summary>
    /// A pool for renting and returning byte buffers.
    /// </summary>
    public class BufferPool
    {
        /// <summary>
        /// O conjunto de arrays subjacente usado para alugar buffers.
        /// </summary>
        private readonly ArrayPool<byte> privPool;

        /// <summary>
        /// O tamanho padrão dos buffers alugados do pool.
        /// </summary>
        private readonly int privSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferPool"/> class.
        /// Inicializa uma nova instância da classe <see cref="BufferPool"/>.
        /// </summary>
        /// <param name="defaultSize">parameto com o tamanho default do buffer.</param>
        public BufferPool(int defaultSize)
        {
            privPool = ArrayPool<byte>.Shared;
            privSize = defaultSize;
        }

        /// <summary>
        /// Recupera um buffer do pool compartilhado com pelo menos o comprimento mínimo especificado.
        /// </summary>
        /// <remarks>O buffer retornado é obtido de um pool compartilhado e deve ser devolvido ou descartado
        /// adequadamente quando não for mais necessário para evitar vazamentos de recursos.</remarks>
        /// <param name="minimumLength">O comprimento mínimo necessário, em bytes, do buffer a ser alugado. Deve ser não negativo.</param>
        /// <returns>Uma instância de <see cref="PooledBuffer"/> contendo um buffer cujo comprimento é de pelo menos <paramref
        /// name="minimumLength"/> bytes.</returns>
        public PooledBuffer Rent(int minimumLength)
        {
            return new PooledBuffer(Math.Max(minimumLength, privSize), ArrayPool<byte>.Shared);
        }
    }
}
