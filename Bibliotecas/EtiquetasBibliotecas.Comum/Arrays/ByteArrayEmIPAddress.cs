using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public class ByteArrayEmIPAddress
    {
        public static System.Net.IPAddress Execute(byte[] arrayByte)
        {
            var ipAddress = new System.Net.IPAddress(arrayByte);
            return ipAddress;
        }
    }
}
