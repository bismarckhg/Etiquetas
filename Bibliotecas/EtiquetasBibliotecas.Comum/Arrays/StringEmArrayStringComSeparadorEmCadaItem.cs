using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringEmArrayStringComSeparadorEmCadaItem
    {
        // ------------------------------------------------------------
        // 1) UM separador de 1 char (char/string com length==1)
        // ------------------------------------------------------------
        /// <summary>
        /// Divide por um separador de 1 caractere, mantendo-o no início.
        /// Ex.: "A^B^C" -> ["A", "^B", "^C"]
        /// </summary>
        public static string[] Execute(string texto, char separador)
        {
            if (texto == null) throw new ArgumentNullException(nameof(texto));
            var partes = new List<string>(8);

            int n = texto.Length, i = 0;

            // prólogo (antes do primeiro separador)
            while (i < n && texto[i] != separador) i++;
            if (i > 0) partes.Add(texto.Substring(0, i));
            if (i >= n) return partes.ToArray();

            int inicioSegmento = i; // aponta para o separador
            i++; // avança

            while (i < n)
            {
                if (texto[i] == separador)
                {
                    partes.Add(texto.Substring(inicioSegmento, i - inicioSegmento));
                    inicioSegmento = i;
                }
                i++;
            }

            partes.Add(texto.Substring(inicioSegmento));
            return partes.ToArray();
        }

        // ------------------------------------------------------------
        // 2) UM separador string (>= 2 caracteres)
        // ------------------------------------------------------------
        /// <summary>
        /// Divide por um separador string (>=2), mantendo-o no início.
        /// Usa comparação ordinal por padrão; habilite ignorarCase se precisar.
        /// </summary>
        public static string[] Execute(string texto, string separador, bool ignorarCase = false)
        {
            if (texto == null) throw new ArgumentNullException(nameof(texto));
            if (string.IsNullOrEmpty(separador)) return new[] { texto };

            var comp = ignorarCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var partes = new List<string>(8);

            int n = texto.Length, m = separador.Length, i = 0;
            int idx = texto.IndexOf(separador, i, comp);
            if (idx < 0) return new[] { texto };

            if (idx > 0) partes.Add(texto.Substring(0, idx));
            int inicioSegmento = idx;

            i = idx + m;
            while (i <= n - m)
            {
                int prox = texto.IndexOf(separador, i, comp);
                if (prox < 0) break;
                partes.Add(texto.Substring(inicioSegmento, prox - inicioSegmento));
                inicioSegmento = prox;
                i = prox + m;
            }

            partes.Add(texto.Substring(inicioSegmento));
            return partes.ToArray();
        }

        // ------------------------------------------------------------
        // 3) VÁRIOS separadores de 1 char (char[] / string[] de 1 char)
        // ------------------------------------------------------------
        /// <summary>
        /// Divide por qualquer separador de 1 char, mantendo-o no início.
        /// Ex.: '^','|','#' -> "A^B|C#D" => ["A","^B","|C","#D"]
        /// </summary>
        public static string[] Execute(string texto, params char[] separadores)
        {
            if (texto == null) throw new ArgumentNullException(nameof(texto));
            if (separadores == null || separadores.Length == 0) return new[] { texto };

            var set = new HashSet<char>(separadores);
            var partes = new List<string>(8);

            int n = texto.Length, i = 0;

            // prólogo
            while (i < n && !set.Contains(texto[i])) i++;
            if (i > 0) partes.Add(texto.Substring(0, i));
            if (i >= n) return partes.ToArray();

            int inicioSegmento = i;
            i++;

            while (i < n)
            {
                if (set.Contains(texto[i]))
                {
                    partes.Add(texto.Substring(inicioSegmento, i - inicioSegmento));
                    inicioSegmento = i;
                }
                i++;
            }

            partes.Add(texto.Substring(inicioSegmento));
            return partes.ToArray();
        }

        /// <summary>
        /// Conforto: passa separadores como strings de 1 char cada.
        /// </summary>
        public static string[] Èxecute(string texto, params string[] separadores1Char)
        {
            if (separadores1Char == null || separadores1Char.Length == 0)
                return new[] { texto };

            var lista = new List<char>(separadores1Char.Length);
            for (int k = 0; k < separadores1Char.Length; k++)
                if (!string.IsNullOrEmpty(separadores1Char[k]))
                    lista.Add(separadores1Char[k][0]);

            // remove duplicados
            var set = new HashSet<char>(lista);
            var arr = new char[set.Count];
            set.CopyTo(arr);

            return DividirVariosChars(texto, arr);
        }

        // ------------------------------------------------------------
        // 4) VÁRIOS separadores com tamanhos variados (string[])
        //    Sem regex; tenta primeiro os MAIORES para resolver sobreposições (ex.: "||" vs "|").
        // ------------------------------------------------------------
        /// <summary>
        /// Divide por vários separadores string (tamanhos variados), mantendo-os no início.
        /// </summary>
        public static string[] Execute(string texto, string[] separadores, bool ignorarCase = false)
        {
            if (texto == null) throw new ArgumentNullException(nameof(texto));
            if (separadores == null || separadores.Length == 0) return new[] { texto };

            // normaliza: remove vazios/duplicados e ordena por tamanho DESC
            var listaNorm = new List<string>();
            var visto = new HashSet<string>(ignorarCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
            for (int s = 0; s < separadores.Length; s++)
            {
                var sp = separadores[s];
                if (string.IsNullOrEmpty(sp)) continue;
                if (visto.Add(sp)) listaNorm.Add(sp);
            }
            listaNorm.Sort((a, b) => b.Length.CompareTo(a.Length)); // maior primeiro
            if (listaNorm.Count == 0) return new[] { texto };

            var comp = ignorarCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            // Mapa: 1º caractere -> lista de candidatos que começam com ele (já em ordem por tamanho desc)
            var mapa = new Dictionary<char, List<string>>();
            for (int idx = 0; idx < listaNorm.Count; idx++)
            {
                string s = listaNorm[idx];
                char chave = ignorarCase ? char.ToUpperInvariant(s[0]) : s[0];
                List<string> lst;
                if (!mapa.TryGetValue(chave, out lst))
                {
                    lst = new List<string>();
                    mapa[chave] = lst;
                }
                lst.Add(s);
            }

            var partes = new List<string>(8);
            int n = texto.Length, i = 0;

            // prólogo (até o primeiro separador)
            while (i < n && !TentaMatch(texto, i, mapa, comp, out _)) i++;
            if (i > 0) partes.Add(texto.Substring(0, i));
            if (i >= n) return partes.ToArray();

            int inicioSegmento = i;

            while (i < n)
            {
                int lenSep;
                if (TentaMatch(texto, i, mapa, comp, out lenSep))
                {
                    if (i != inicioSegmento)
                    {
                        // Fechamos o segmento anterior imediatamente antes do novo separador
                        partes.Add(texto.Substring(inicioSegmento, i - inicioSegmento));
                        inicioSegmento = i;
                    }
                    // pular o tamanho do separador detectado para evitar rematch no mesmo ponto
                    i += (lenSep > 0 ? lenSep : 1);
                }
                else
                {
                    i++;
                }
            }

            partes.Add(texto.Substring(inicioSegmento));
            return partes.ToArray();

            // ---- função local (C# 7.0+) ----
            bool TentaMatch(string txt, int idx, Dictionary<char, List<string>> mp, StringComparison cmp, out int len)
            {
                len = 0;
                if (idx >= txt.Length) return false;

                char chave = (cmp == StringComparison.OrdinalIgnoreCase)
                           ? char.ToUpperInvariant(txt[idx])
                           : txt[idx];

                List<string> candidatos;
                if (!mp.TryGetValue(chave, out candidatos)) return false;

                // candidatos já em ordem por tamanho desc
                for (int c = 0; c < candidatos.Count; c++)
                {
                    string s = candidatos[c];
                    int m = s.Length;
                    if (idx + m <= txt.Length && string.Compare(txt, idx, s, 0, m, cmp) == 0)
                    {
                        len = m;
                        return true;
                    }
                }
                return false;
            }

            //public static string[] Execute(string texto, char separador)
            //{
            //    string pattern = string.Format(@"{0}.*?(?={0}|$)", Regex.Escape(separador.ToString()));
            //    return Regex.Matches(texto, pattern, RegexOptions.Singleline)
            //                .Cast<Match>()
            //                .Select(match => match.Value)
            //                .ToArray();
            //}
        }
    }
}
