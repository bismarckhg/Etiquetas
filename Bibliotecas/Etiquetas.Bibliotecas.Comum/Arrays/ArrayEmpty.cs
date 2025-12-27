using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Fornece uma instância de array vazio para qualquer tipo especificado.
    /// </summary>
    public static class ArrayEmpty
    {
        /// <summary>
        /// Retorna uma instância de array vazio do tipo especificado T.
        /// </summary>
        /// <typeparam name="T">Tipo Variavel.</typeparam>
        /// <returns>retorna o tipo variavel.</returns>
        public static T[] Executa<T>() => EmptyHolder<T>.Instance;

        private static class EmptyHolder<T>
        {
            internal static readonly T[] Instance = new T[0];
        }
    }
}
