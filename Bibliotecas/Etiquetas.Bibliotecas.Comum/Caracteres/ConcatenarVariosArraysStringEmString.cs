using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConcatenarVariosArraysStringEmString
    {
        public static string ExecuteLinq(params string[][] arrays)
        {
            if (arrays == null || arrays.Length == 0)
                return string.Empty;

            // Usando LINQ para mesclar todos os arrays e concatenar em uma única string
            return string.Concat(arrays.Where(a => a != null).SelectMany(array => array));
        }

        public static string ExecuteStringBuilder(params string[][] arrays)
        {
            // Verifica se o argumento é nulo ou vazio
            if (arrays == null || arrays.Length == 0)
                return string.Empty;

            // Inicializa o StringBuilder com uma capacidade estimada para evitar realocações frequentes
            StringBuilder sb = new StringBuilder();

            foreach (var array in arrays)
            {
                if (array == null || array.Length == 0)
                    continue;

                foreach (var str in array)
                {
                    // Adiciona cada string ao StringBuilder
                    sb.Append(str);
                }
            }

            return sb.ToString();
        }
    }
}
