using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Separa uma string em um array de strings com base em quebras de linha (Unix e Windows).
    /// </summary>
    public static class SeparaStringEmArrayStringComQuebraDeLinha
    {
        /// <summary>
        /// Separa a string em um array de strings, removendo os marcadores de final de linha (Unix e Windows).
        /// </summary>
        /// <param name="dados">string de dados.</param>
        /// <param name="filtrarLinhasVazias">true se for filtrar linhas vazias.</param>
        /// <returns>array de string.</returns>
        public static string[] SemMarcadorFinalLinhaUnixWindows(string dados, bool filtrarLinhasVazias = true)
        {
            // Split para dividir a string em linhas Unix e Windows
            var options = filtrarLinhasVazias ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            return dados.Split(new string[] { "\r\n", "\n" }, options);
        }

        /// <summary>
        /// Separa a string em um array de strings, mantendo os marcadores de final de linha (Unix e Windows).
        /// </summary>
        /// <param name="dados">string de dados.</param>
        /// <param name="filtrarLinhasVazias">true se filtrar linhas vazias.</param>
        /// <returns>array de string filtrado.</returns>
        public static string[] ComMarcadorFinalLinhaUnixWindows(string dados, bool filtrarLinhasVazias = true)
        {
            // Usando Regex para dividir e manter os delimitadores de nova linha conforme Windows ou Unix
            var array = Regex.Split(dados, @"(?<=\r\n|\n)");
            return filtrarLinhasVazias ? FiltraLinhasVazias(array) : array;
        }

        /// <summary>
        /// Separa a string em um array de strings, mantendo os marcadores de final de linha no formato Windows (\r\n).
        /// </summary>
        /// <param name="dados">string de dados.</param>
        /// <param name="filtrarLinhasVazias">true se filtrar linhas vazias.</param>
        /// <returns>array de string filtrado.</returns>
        public static string[] ComMarcadorFinalLinhaWindows(string dados, bool filtrarLinhasVazias = true)
        {
            // Usando Regex para substituir \n por \r\n, mas mantendo os \r\n existentes
            var texto = Regex.Replace(dados, @"(?<!\r)\n", "\r\n");
            var array = ComMarcadorFinalLinhaUnixWindows(texto, filtrarLinhasVazias);
            return filtrarLinhasVazias ? FiltraLinhasVazias(array) : array;
        }

        /// <summary>
        /// Separa a string em um array de strings, mantendo os marcadores de final de linha no formato Unix (\n).
        /// </summary>
        /// <param name="dados">string de dados.</param>
        /// <param name="filtrarLinhasVazias">true se filtrar linhas vazias.</param>
        /// <returns>array de string filtrado.</returns>
        public static string[] ComMarcadorFinalLinhaUnix(string dados, bool filtrarLinhasVazias = true)
        {
            // Usando Replace para substituir \r\n por \n
            var texto = dados.Replace("\r\n", "\n");
            var array = ComMarcadorFinalLinhaUnixWindows(texto, filtrarLinhasVazias);
            return filtrarLinhasVazias ? FiltraLinhasVazias(array) : array;
        }

        /// <summary>
        /// Filtra linhas vazias ou contendo apenas quebras de linha.
        /// </summary>
        /// <param name="linhas">array de string com linhas.</param>
        /// <returns>array de string em linhas.</returns>
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
