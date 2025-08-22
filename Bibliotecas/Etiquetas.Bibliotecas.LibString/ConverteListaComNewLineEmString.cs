using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.LibString
{
    public static class ConverteListaComNewLineEmString
    {
        public static string Execute(this List<string> lista)
        {
            var delimitador = Environment.NewLine;
            return string.Join("", lista.Where(line => !Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(line.TrimEnd()) && line.TrimEnd() != delimitador));
        }
    }
}
