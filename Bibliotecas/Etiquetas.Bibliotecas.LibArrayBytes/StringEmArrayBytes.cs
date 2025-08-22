using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.LibArrayBytes
{
    public static class StringEmArrayBytes
    {
        public static byte[] ExecuteASCII(string texto)
        {
            return Encoding.ASCII.GetBytes(texto);
        }

        public static byte[] ExecuteUTF8(string texto)
        {
            return Encoding.UTF8.GetBytes(texto);
        }

        public static byte[] Execute(Encoding encoder, string texto)
        {
           return encoder.GetBytes(texto);
        }
    }
}
