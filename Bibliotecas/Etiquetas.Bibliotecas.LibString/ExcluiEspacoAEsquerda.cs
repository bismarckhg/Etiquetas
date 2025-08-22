using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.LibString
{
    public static class ExcluiEspacoAEsquerda
    {
        public static string Execute(string texto)
        {
            return texto.TrimStart();
        }
    }
}
