using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.LibInt
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
                throw new Exception($"Tipo null invalido em QuatroBytesInt32.");
            }

            if ((bytes.Length - startIndex) < 4)
            {
                throw new Exception($"Int32 utiliza no minimo 4 bytes. QuatroBytesInt32.");
            }
            //return Execute(bytes[0], bytes[1], bytes[2], bytes[3]);
            return BitConverter.ToInt32(bytes, startIndex);
        }

    }
}