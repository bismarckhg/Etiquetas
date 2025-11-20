using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Numericos
{
    public static class OitoBytesEmInt64
    {
        public static Int64 Execute(byte byte1, byte byte2, byte byte3, byte byte4, byte byte5, byte byte6, byte byte7, byte byte8)
        {
            // return (Int32)((byte4 << 24) | (byte3 << 16) | (byte2 << 8) | (byte1));

            return Execute(new byte[] { byte8, byte7, byte6, byte5, byte4, byte3, byte2, byte1 });
        }

        public static Int64 Execute(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if ((bytes.Length - startIndex) < 8)
            {
                throw new ArgumentException("O array deve conter pelo menos 8 bytes a partir do índice inicial.", nameof(bytes));
            }
            return BitConverter.ToInt64(bytes, startIndex);
        }
    }
}
