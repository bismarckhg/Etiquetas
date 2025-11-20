using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class QuantidadeCaracteresNaStringBuilder
    {
        public static int Execute(StringBuilder builder, char caractere)
        {
            if (builder == null)
            {
                return 0;
            }
            int count = 0;
            for (int i = 0; i < builder.Length; i++)
            {
                if (builder[i] == caractere)
                {
                    count++;
                }
            }
            return count;
        }

        public static int Execute(StringBuilder builder, string texto)
        {
            if (builder == null || string.IsNullOrEmpty(texto))
            {
                return 0;
            }

            string source = builder.ToString();
            int count = 0;
            int i = 0;
            while ((i = source.IndexOf(texto, i)) != -1)
            {
                count++;
                i += texto.Length;
            }
            return count;
        }

    }
}