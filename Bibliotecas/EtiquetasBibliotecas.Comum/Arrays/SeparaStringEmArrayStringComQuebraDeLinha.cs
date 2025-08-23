using Etiquetas.Bibliotecas.Comum.Caracteres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class SeparaStringEmArrayStringComQuebraDeLinha
    {
        public static string[] SemMarcadorFinalLinhaUnixWindows(string dados, bool filtrarLinhasVazias = true)
        {
            // Split para dividir a string em linhas Unix e Windows
            var options = filtrarLinhasVazias ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            return dados.Split(new string[] { Environment.NewLine, "\n" }, options);
        }

        public static string[] ComMarcadorFinalLinhaUnixWindows(string dados, bool filtrarLinhasVazias = true)
        {
            // Usando Regex para dividir e manter os delimitadores de nova linha conforme Windows ou Unix
            var array = Regex.Split(dados, @"(?<=\r\n|\n)");
            return filtrarLinhasVazias ? FiltraLinhasVazias(array) : array;
        }
        
        public static string[] ComMarcadorFinalLinhaWindows(string dados, bool filtrarLinhasVazias = true)
        {
            // Usando Regex para substituir \n por \r\n, mas mantendo os \r\n existentes
            var texto = Regex.Replace(dados, @"(?<!\r)\n", "\r\n");
            var array = ComMarcadorFinalLinhaUnixWindows(texto, filtrarLinhasVazias);
            return filtrarLinhasVazias ? FiltraLinhasVazias(array) : array;
        }

        public static string[] ComMarcadorFinalLinhaUnix(string dados, bool filtrarLinhasVazias = true)
        {
            // Usando Replace para substituir \r\n por \n
            var texto = dados.Replace("\r\n", "\n");
            var array = ComMarcadorFinalLinhaUnixWindows(texto, filtrarLinhasVazias);
            return filtrarLinhasVazias ? FiltraLinhasVazias(array) : array;
        }

        private static string[] FiltraLinhasVazias(string[] linhas)
        {
            // Filtrando as linhas para remover vazias ou contendo apenas \r\n ou \n
            return linhas
                .Where(l => !EhStringNuloVazioComEspacosBranco
                .Execute(l) && l != "\r\n" && l != "\n")  // Remove vazias ou apenas quebras de linha
                .ToArray();
        }
    }
}
