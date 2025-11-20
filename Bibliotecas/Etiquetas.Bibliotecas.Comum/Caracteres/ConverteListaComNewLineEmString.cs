using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConverteListaComNewLineEmString
    {
        public static string Execute(this List<string> lista)
        {
            if (lista == null)
            {
                return string.Empty;
            }
            var delimitador = Environment.NewLine;
            return string.Join("", lista.Where(line => line != null && !EhStringNuloVazioComEspacosBranco.Execute(line.TrimEnd()) && line.TrimEnd() != delimitador));
        }
    }
}
