using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class ArrayEmpty
    {
        public static T[] Executa<T>() => EmptyHolder<T>.Instance;

        private static class EmptyHolder<T>
        {
            internal static readonly T[] Instance = new T[0];
        }
    }
}
