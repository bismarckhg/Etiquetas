using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ExcluiEspacoAEsquerda
    {
        public static string Execute(string texto)
        {
            return texto.TrimStart();
        }
    }
}
