using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ArrayBytesEmString
    {
        public static string Execute(byte[] bytes, int bytesLidos, Encoding encoding)
        {
            if (bytes == null) return null;
            return encoding.GetString(bytes, 0, bytesLidos);
        }

        public static string Execute(byte[] bytes, Encoding encoding)
        {
            if (bytes == null) return null;
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        public static string Execute(byte[] bytes, int posicaoInicial, int count, Encoding encoding)
        {
            if (bytes == null) return null;
            return encoding.GetString(bytes, posicaoInicial, count);
        }
    }
}
