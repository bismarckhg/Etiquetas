using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.LibInt
{
    public static class StringParaInt16Nulo
    {
        public static Int16? Execute(this string value)
        {
            var valido = Int16.TryParse(value, out Int16 result);
            if (valido)
            {
                return result;
            }
            return null;
        }
    }
}
