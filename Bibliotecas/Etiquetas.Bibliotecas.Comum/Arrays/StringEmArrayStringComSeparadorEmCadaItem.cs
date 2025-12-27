using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Divide uma string em um array de strings, mantendo o separador no início de cada item
    /// (exceto o primeiro, se houver texto antes do primeiro separador).
    /// </summary>
    public static class StringEmArrayStringComSeparadorEmCadaItem
    {
        // ------------------ Facade (4 cenários) ------------------

        /// <summary>
        /// Divide uma string em um array de strings, mantendo o separador no início de cada item.
        /// </summary>
        /// <param name="texto">texto string a ser dividido em array string.</param>
        /// <param name="separador">separador(split) de divisão do texto em array string.</param>
        /// <param name="removeEmptyEntries">se true remove os arrays string vazios.</param>
        /// <returns>retorna o array string dividido.</returns>
        public static string[] Execute(string texto, char separador, bool removeEmptyEntries = true)
        {
            if (texto == null)
            {
                throw new ArgumentNullException(nameof(texto));
            }

            var sepChars = new[] { separador };
            return Core(texto, sepChars, null, removeEmptyEntries);
        }

        /// <summary>
        /// Divide uma string em um array de strings, mantendo o separador no início de cada item. 
        /// </summary>
        /// <param name="texto">texto string a ser dividido em array string.</param>
        /// <param name="separador">separador(split) de divisão do texto em array string.</param>
        /// <param name="removeEmptyEntries">se true remove os arrays string vazios.</param>
        /// <returns>retorna o array string dividido.</returns>
        public static string[] Execute(string texto, string separador, bool removeEmptyEntries = true)
        {
            if (texto == null)
            {
                throw new ArgumentNullException(nameof(texto));
            }

            if (separador == null)
            {
                throw new ArgumentNullException(nameof(separador));
            }

            if (separador.Length == 0)
            {
                return FiltrarVazios(new[] { texto }, removeEmptyEntries);
            }

            if (separador.Length == 1)
            {
                return Execute(texto, separador[0], removeEmptyEntries);
            }

            var sepsStr = new[] { separador };
            return Core(texto, null, sepsStr, removeEmptyEntries);
        }

        /// <summary>
        /// Divide uma string em um array de strings, mantendo o separador no início de cada item.
        /// </summary>
        /// <param name="texto">texto string a ser dividido em array string.</param>
        /// <param name="separador">separador(split) de divisão do texto em array string.</param>
        /// <param name="removeEmptyEntries">se true remove os arrays string vazios.</param>
        /// <returns>retorna o array string dividido.</returns>
        public static string[] Execute(string texto, char[] separadoresChar, bool removeEmptyEntries = true)
        {
            if (texto == null)
            {
                throw new ArgumentNullException(nameof(texto));
            }

            if (separadoresChar == null || separadoresChar.Length == 0)
            {
                return FiltrarVazios(new[] { texto }, removeEmptyEntries);
            }

            // Dedup com tabela booleana (sem List/HashSet)
            var mapa = new bool[ushort.MaxValue + 1];
            int unicos = 0;
            for (int i = 0; i < separadoresChar.Length; i++)
            {
                char c = separadoresChar[i];
                if (!mapa[c])
                {
                    mapa[c] = true;
                    unicos++;
                }
            }

            var seps = new char[unicos];
            for (int c = 0, k = 0; c <= ushort.MaxValue; c++)
            {
                if (mapa[c])
                {
                    seps[k++] = (char)c;
                }
            }

            return Core(texto, seps, null, removeEmptyEntries);
        }

        /// <summary>
        /// Divide uma string em um array de strings, mantendo o separador no início de cada item.
        /// </summary>
        /// <param name="texto">texto string a ser dividido em array string.</param>
        /// <param name="separador">separador(split) de divisão do texto em array string.</param>
        /// <param name="removeEmptyEntries">se true remove os arrays string vazios.</param>
        /// <returns>retorna o array string dividido.</returns>
        public static string[] Execute(string texto, string[] separadoresString, bool removeEmptyEntries = true)
        {
            if (texto == null)
            {
                throw new ArgumentNullException(nameof(texto));
            }

            if (separadoresString == null || separadoresString.Length == 0)
            {
                return FiltrarVazios(new[] { texto }, removeEmptyEntries);
            }

            // Normaliza: remove nulos/vazios, dedup, separa 1-char de multi, ordena multi por tamanho desc
            // Passo 1: filtra nulos/vazios
            int validos = 0;
            for (int i = 0; i < separadoresString.Length; i++)
            {
                if (!string.IsNullOrEmpty(separadoresString[i]))
                {
                    validos++;
                }
            }

            if (validos == 0)
            {
                return FiltrarVazios(new[] { texto }, removeEmptyEntries);
            }

            var tmp = new string[validos];

            for (int i = 0, k = 0; i < separadoresString.Length; i++)
            {
                if (!string.IsNullOrEmpty(separadoresString[i]))
                {
                    tmp[k++] = separadoresString[i];
                }
            }

            // Passo 2: ordena ordinal para deduplicar contíguos
            Array.Sort(tmp, StringComparer.Ordinal);

            // Passo 3: remove duplicatas (in-place)
            int u = 1;
            for (int i = 1; i < tmp.Length; i++)
            {
                if (!string.Equals(tmp[i], tmp[u - 1], StringComparison.Ordinal))
                {
                    tmp[u++] = tmp[i];
                }
            }

            // Passo 4: particiona em chars e multi
            int qtdChar = 0;
            for (int i = 0; i < u; i++)
            {
                if (tmp[i].Length == 1)
                {
                    qtdChar++;
                }
            }

            var sepsChar = qtdChar > 0 ? new char[qtdChar] : null;
            var sepsMulti = (u - qtdChar) > 0 ? new string[u - qtdChar] : null;

            for (int i = 0, ci = 0, mi = 0; i < u; i++)
            {
                var s = tmp[i];
                if (s.Length == 1)
                {
                    sepsChar[ci++] = s[0];
                }
                else
                {
                    sepsMulti[mi++] = s;
                }
            }

            // Passo 5: ordena multi por tamanho desc (depois ordinal p/ determinismo)
            if (sepsMulti != null)
            {
                Array.Sort(sepsMulti, (a, b) =>
                {
                    int cmp = b.Length.CompareTo(a.Length);
                    return cmp != 0 ? cmp : string.CompareOrdinal(a, b);
                });
            }

            return Core(texto, sepsChar, sepsMulti, removeEmptyEntries);
        }

        // ------------------ Núcleo sem List/HashSet ------------------

        /// <summary>
        /// Realiza a divisão mantendo separadores no INÍCIO, preferindo sempre o separador MAIS LONGO.
        /// </summary>
        private static string[] Core(string texto, char[] sepChars, string[] sepMulti, bool removeEmptyEntries)
        {
            int n = texto.Length;

            // Tabela de 1-char (O(1)) — evita List/HashSet
            var isCharSep = new bool[ushort.MaxValue + 1];
            if (sepChars != null)
            {
                for (int i = 0; i < sepChars.Length; i++)
                {
                    isCharSep[sepChars[i]] = true;
                }
            }

            // Pré-computa dados de multi
            char[] multiFirst = null;
            int[] multiLen = null;
            if (sepMulti != null && sepMulti.Length > 0)
            {
                multiFirst = new char[sepMulti.Length];
                multiLen = new int[sepMulti.Length];
                for (int i = 0; i < sepMulti.Length; i++)
                {
                    multiFirst[i] = sepMulti[i][0];
                    multiLen[i] = sepMulti[i].Length;
                }
            }

            // ---------- Passo 1: contar segmentos ----------
            int firstIdx = -1;
            int ocorr = 0;

            int iPos = 0;
            while (iPos < n)
            {
                int len;
                if (TentaMatch(texto, n, iPos, isCharSep, sepMulti, multiFirst, multiLen, out len))
                {
                    if (firstIdx < 0)
                    {
                        firstIdx = iPos;
                    }

                    ocorr++;
                    iPos += len > 0 ? len : 1;
                }
                else
                {
                    iPos++;
                }
            }

            if (firstIdx < 0)
            {
                // Sem separadores
                if (removeEmptyEntries && n == 0)
                {
                    return new string[0];
                }

                return new[] { texto };
            }

            int totalSegmentos = (firstIdx > 0 ? 1 : 0) + ocorr;
            if (totalSegmentos == 0)
            {
                return new string[0];
            }

            // ---------- Passo 2: preencher array ----------
            var resultado = new string[totalSegmentos];
            int k = 0;

            if (firstIdx > 0)
            {
                resultado[k++] = texto.Substring(0, firstIdx);
            }

            int inicioSegmento = firstIdx;
            iPos = firstIdx;
            while (iPos < n)
            {
                // pos atual iPos DEVE estar num separador
                int lenAtual;
                if (!TentaMatch(texto, n, iPos, isCharSep, sepMulti, multiFirst, multiLen, out lenAtual))
                {
                    // segurança (não deve acontecer): avança
                    iPos++;
                    continue;
                }

                // encontra início do PRÓXIMO separador (ou fim do texto)
                int scan = iPos + (lenAtual > 0 ? lenAtual : 1);
                while (scan < n)
                {
                    int lenNext;
                    if (TentaMatch(texto, n, scan, isCharSep, sepMulti, multiFirst, multiLen, out lenNext))
                    {
                        break;
                    }

                    scan++;
                }

                // segmento atual: [inicioSegmento .. scan)
                resultado[k++] = texto.Substring(inicioSegmento, scan - inicioSegmento);
                inicioSegmento = scan;
                iPos = scan;
                if (scan >= n)
                {
                    break;
                }
            }

            // removeEmptyEntries (apenas ""), sem List
            return FiltrarVazios(resultado, removeEmptyEntries);
        }

        /// <summary>
        /// Tenta casar qualquer separador em txt[idx], preferindo multi (mais longos) antes de 1 char.
        /// </summary>
        private static bool TentaMatch(
            string txt,
            int n,
            int idx,
            bool[] isCharSep,
            string[] sepMulti,
            char[] multiFirst,
            int[] multiLen,
            out int len)
        {
            len = 0;
            if (idx >= n)
            {
                return false;
            }

            // 1) multi (ordenados por tamanho desc) — checa só os que têm o mesmo primeiro char
            if (sepMulti != null)
            {
                char c = txt[idx];
                for (int i = 0; i < sepMulti.Length; i++)
                {
                    if (multiFirst[i] != c)
                    {
                        continue;
                    }

                    int m = multiLen[i];
                    if (idx + m <= n && string.Compare(txt, idx, sepMulti[i], 0, m, StringComparison.Ordinal) == 0)
                    {
                        len = m;
                        return true;
                    }
                }
            }

            // 2) 1-char
            if (isCharSep[txt[idx]])
            {
                len = 1;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Filtra strings vazias do array, se solicitado.
        /// </summary>
        /// <param name="partes">array de string a ser filtrado.</param>
        /// <param name="removeEmptyEntries">se true remove os elementos vazios.</param>
        /// <returns>retorna o array string dividido.</returns>
        private static string[] FiltrarVazios(string[] partes, bool removeEmptyEntries)
        {
            if (!removeEmptyEntries)
            {
                return partes;
            }

            int n = partes.Length;
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                if (!string.IsNullOrEmpty(partes[i]))
                {
                    count++;
                }
            }

            if (count == n)
            {
                return partes;
            }

            var arr = new string[count];
            for (int i = 0, k = 0; i < n; i++)
            {
                if (!string.IsNullOrEmpty(partes[i]))
                {
                    arr[k++] = partes[i];
                }
            }

            return arr;
        }
    }
}
