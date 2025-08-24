using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Numericos
{
    public static class QuatroBytesEmInt32
    {
        public static Int32 Execute(byte byte1, byte byte2, byte byte3, byte byte4)
        {
            // return (Int32)((byte4 << 24) | (byte3 << 16) | (byte2 << 8) | (byte1));

            return Execute(new byte[] { byte4, byte3, byte2, byte1 });

        }

        public static Int32 Execute(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if ((bytes.Length - startIndex) < 4)
            {
                throw new ArgumentException("O array deve conter pelo menos 4 bytes a partir do índice inicial.", nameof(bytes));
            }
            return BitConverter.ToInt32(bytes, startIndex);
        }

    }
}