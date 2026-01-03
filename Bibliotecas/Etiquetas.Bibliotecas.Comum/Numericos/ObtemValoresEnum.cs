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
    public static class ObtemValoresEnum<T>
            where T : Enum
    {
        /// <summary>
        /// Obtém os valores de um enum genérico.
        /// </summary>
        /// <typeparam name="T">Tipo enum a ser obtido.</typeparam>
        /// <returns>retorna Array de enum.</returns>
        public static T[] ObtemEnum()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Obtém o nome do valor do enum.
        /// </summary>
        /// <param name="valor">Valore Enum.</param>
        /// <returns>string do nome.</returns>
        public static string ObtemNome(T valor)
        {
            return Enum.GetName(typeof(T), valor);
        }
    }
}
