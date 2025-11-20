using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Numericos
{
    public static class StringParaInt64
    {
        public static Int64 Execute(string value)
        {
            var valido = Int64.TryParse(value, out Int64 result);
            if (valido)
            {
                return result;
            }
            return default;
        }
    }
}
