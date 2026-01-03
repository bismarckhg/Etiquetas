using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Numericos
{
    /// <summary>
    /// Classe utilitária para obter os valores de um enum.
    /// </summary>
    public static class ObtemValoresEnum
    {
        /// <summary>
        /// Obtém os valores de um enum genérico.
        /// </summary>
        /// <typeparam name="T">Tipo enum a ser obtido.</typeparam>
        /// <returns>retorna Array de enum.</returns>
        public static T[] ObterValoresEnum<T>()
            where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}
