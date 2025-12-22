using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Etiquetas.Bibliotecas.ControleFilaDados
{
    /// <summary>
    /// Extensões essenciais para fila de lotes atômicos + snapshot com flatten.
    /// compatíveis com C# 7.3 / .NET Framework 4.7.2.
    /// </summary>
    public static class FilaConcorrenteExtensoes
    {
        /// <summary>
        /// Enfileira um LOTE como um único item (atomicidade de lote garantida no snapshot).
        /// Materializa a sequência se necessário para garantir imutabilidade durante o enfileiramento.
        /// </summary>
        public static void EnqueueBatch<T>(this ConcurrentQueue<IReadOnlyList<T>> fila, IEnumerable<T> itens)
        {
            if (fila == null) throw new ArgumentNullException(nameof(fila));
            if (itens == null) throw new ArgumentNullException(nameof(itens));

            // Se já for uma coleção indexada/estável, reaproveita; caso contrário, materializa como array.
            var lote = itens as IReadOnlyList<T>;
            if (lote == null)
                lote = (itens as T[]) ?? itens.ToArray();

            fila.Enqueue(lote);
        }

        /// <summary>
        /// Tira snapshot FIFO de lotes e ACHATA (flatten) para um único array contínuo,
        /// preservando a ordem entre lotes e a ordem interna de cada lote.
        /// </summary>
        public static T[] ToFlattenedArraySnapshot<T>(this ConcurrentQueue<IReadOnlyList<T>> fila)
        {
            if (fila == null) throw new ArgumentNullException(nameof(fila));

            // 1) Snapshot de LOTES (cada elemento é um lote completo)
            var lotes = fila.ToArray();

            // 2) Soma total para alocar uma única vez
            var total = 0;
            for (int i = 0; i < lotes.Length; i++)
                total += lotes[i].Count;

            // 3) Flatten preservando ordem
            var final = new T[total];
            var off = 0;
            for (int i = 0; i < lotes.Length; i++)
            {
                var l = lotes[i];

                // Otimização: se for array, usa Array.Copy
                var arr = l as T[];
                if (arr != null)
                {
                    Array.Copy(arr, 0, final, off, arr.Length);
                    off += arr.Length;
                }
                else
                {
                    for (int j = 0; j < l.Count; j++)
                        final[off++] = l[j];
                }
            }
            return final;
        }
    }
}