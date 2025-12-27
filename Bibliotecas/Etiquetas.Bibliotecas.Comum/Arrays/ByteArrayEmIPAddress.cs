using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Converte um array de bytes em um objeto IPAddress.
    /// </summary>
    public class ByteArrayEmIPAddress
    {
        /// <summary>
        /// Converte um array de bytes em um objeto IPAddress.
        /// </summary>
        /// <param name="arrayByte">array de Bytes.</param>
        /// <returns>retorna IpAddress.</returns>
        public static System.Net.IPAddress Execute(byte[] arrayByte)
        {
            var ipAddress = new System.Net.IPAddress(arrayByte);
            return ipAddress;
        }
    }
}
