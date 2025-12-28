using System;
using System.Buffers;

namespace Etiquetas.Application.Services
{
    /// <summary>
    /// Representa um buffer de bytes alugado de um ArrayPool.
    /// </summary>
    public sealed class PooledBuffer : IDisposable
    {
        /// <summary>
        /// Referência ao pool de arrays do qual o buffer foi alugado.
        /// </summary>
        private readonly ArrayPool<byte> privPool;

        /// <summary>
        /// Indica se o buffer já foi devolvido ao pool.
        /// </summary>
        private bool returned;

        /// <summary>
        /// Initializes a new instance of the <see cref="PooledBuffer"/> class.
        /// Inicializa uma nova instancia da classe <see cref="PooledBuffer"/>.
        /// </summary>
        /// <param name="size">tamanho do Pool.</param>
        /// <param name="pool">Pool.</param>
        public PooledBuffer(int size, ArrayPool<byte> pool)
        {
            privPool = pool;
            Buffer = privPool.Rent(size);
            Length = 0;
            returned = false;
        }

        /// <summary>
        /// Gets - O buffer de bytes alugado.
        /// </summary>
        public byte[] Buffer { get; }

        /// <summary>
        /// Gets e Sets - O comprimento atual dos dados válidos no buffer.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets - Obtém um segmento do array representando os dados válidos no buffer.
        /// </summary>
        public ArraySegment<byte> Segment => new ArraySegment<byte>(Buffer, 0, Length);

        /// <summary>
        /// Define o comprimento dos dados válidos no buffer.
        /// </summary>
        /// <param name="len">Tamanho.</param>
        public void SetLength(int len)
        {
            if (len < 0 || len > Buffer.Length)
            {
                throw new ArgumentOutOfRangeException("len");
            }

            Length = len;
        }

        /// <summary>
        /// Devolve o buffer ao pool.
        /// </summary>
        public void Return()
        {
            if (returned)
            {
                return;
            }

            returned = true;
            privPool.Return(Buffer);
        }

        /// <summary>
        /// Libera todos os recursos usados ​​pela instância atual da classe.
        /// </summary>
        /// <remarks>Chame este método quando terminar de usar o objeto para liberar recursos não gerenciados
        /// e realizar outras operações de limpeza. Após chamar <see cref="Dispose"/>, o objeto não deve ser
        /// usado.</remarks>
        public void Dispose()
        {
            Return();
            GC.SuppressFinalize(this);
        }
    }
}
