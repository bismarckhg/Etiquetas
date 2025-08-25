using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ZeroAEsquerdaInt
    {
        public static string Execute(this int numero, int tamanho)
        {
            if (tamanho < 0)
                tamanho = 0;

            if (tamanho > 9)
                tamanho = 9;

            return numero.ToString($"D{tamanho}");
        }
    }
}
