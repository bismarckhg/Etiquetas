using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public class StringIP
    {
        public static byte[] EmByteArray(string ip)
        {
            var arrayByte = (ip.Split('.').Select(a => Convert.ToByte(a)).ToArray());
            return arrayByte;
        }
    }
}
