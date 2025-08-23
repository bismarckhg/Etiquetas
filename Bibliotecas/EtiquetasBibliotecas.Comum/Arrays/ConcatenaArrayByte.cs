using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class ConcatenaArrayBytes
    {
        public static byte[] Execute(byte[] array1, byte[] array2)
        {
            // Cria um array de bytes para armazenar o resultado
            byte[] resultado = new byte[array1.Length + array2.Length];

            // Copia o primeiro array para o resultado
            Buffer.BlockCopy(array1, 0, resultado, 0, array1.Length);

            // Copia o segundo array para o resultado após o primeiro array
            Buffer.BlockCopy(array2, 0, resultado, array1.Length, array2.Length);

            return resultado;
        }
    }
}
