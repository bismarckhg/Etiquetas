using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Numericos
{
    public static class ConverteInt64EmOitoBytes
    {
        public static byte[] Execute(long inteiro64)
        {
            return BitConverter.GetBytes(inteiro64);
        }
    }
}
