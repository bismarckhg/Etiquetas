using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Converte um array de bytes em uma string, utilizando a codificação especificada.
    /// </summary>
    public static class ArrayBytesEmString
    {
        /// <summary>
        /// Converte um array de bytes em uma string, utilizando a codificação especificada. 
        /// </summary>
        /// <param name="bytes">array de bytes.</param>
        /// <param name="bytesLidos">quantidade de bytes lidos.</param>
        /// <param name="encoding">o encoding aplicado ao array.</param>
        /// <returns>retorna string ecoding.</returns>
        public static string Execute(byte[] bytes, int bytesLidos, Encoding encoding)
        {
            if (bytes == null)
            {
                return null;
            }

            return encoding.GetString(bytes, 0, bytesLidos);
        }

        /// <summary>
        /// Converte um array de bytes em uma string, utilizando a codificação especificada.
        /// </summary>
        /// <param name="bytes">array de bytes.</param>
        /// <param name="encoding">o encoding aplicado ao array.</param>
        /// <returns>retorna string ecoding.</returns>
        public static string Execute(byte[] bytes, Encoding encoding)
        {
            if (bytes == null)
            {
                return null;
            }

            return encoding.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Converte um array de bytes em uma string, utilizando a codificação especificada.
        /// </summary>
        /// <param name="bytes">array de bytes.</param>
        /// <param name="posicaoInicial">Posicao inicial no array.</param>
        /// <param name="count">contagem do array</param>
        /// <param name="encoding">o encoding aplicado ao array.</param>
        /// <returns>retorna string ecoding.</returns>
        public static string Execute(byte[] bytes, int posicaoInicial, int count, Encoding encoding)
        {
            if (bytes == null)
            {
                return null;
            }

            return encoding.GetString(bytes, posicaoInicial, count);
        }
    }
}
