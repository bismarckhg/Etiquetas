using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.LibInt
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
                throw new Exception($"Tipo null invalido em OitoBytesInt64.");
            }

            if ((bytes.Length - startIndex) < 8)
            {
                throw new Exception($"Int64 utiliza no minimo 8 bytes. OitoBytesInt64.");
            }
            //return Execute(bytes[0], bytes[1], bytes[2], bytes[3]);
            return BitConverter.ToInt64(bytes, startIndex);
        }
    }
}
