using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class Maiusculas
    {
        public static string Execute(this string texto)
        {
            return texto.ToUpper();
        }
    }
}
