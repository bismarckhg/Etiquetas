using System;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class IndexOf
    {
        public static int Posicao(StringBuilder texto, char caractere, int sequencia)
        {
            var sequenciaAtual = 0;
            var numCaracteres = texto.Length;
            for (int i = 0; i < numCaracteres; i++)
            {
                var caractereAtual = texto[i];
                if (caractereAtual == caractere)
                {
                    sequenciaAtual++;
                    if (sequenciaAtual == sequencia)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public static int Posicao(StringBuilder sb, string value, int sequencia)
        {
            if (sb == null || value == null) throw new ArgumentNullException();
            if (value.Length == 0) return 0; // Encontrar string vazia retorna 0

            var sbprocura = new StringBuilder(value);

            for (int i = 0; i < sb.Length - sbprocura.Length + 1; i++)
            {
                bool found = true;
                for (int j = 0; j < sbprocura.Length; j++)
                {
                    if (sb[i + j] != sbprocura[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1; // Não encontrado
        }

        public static int PosicaoKMP(StringBuilder sb, char pattern, int posicaoInicial = 0)
        {
            int[] lps = new int[1];
            lps[0] = 0;
            int i = posicaoInicial; // índice para sb
            int j = 0; // índice para pattern
            while (i < sb.Length)
            {
                if (pattern == sb[i])
                {
                    i++;
                    j++;
                }

                if (j == 1)
                {
                    return i - j; // encontra correspondência
                }
                else if (i < sb.Length && pattern != sb[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i = i + 1;
                }
            }
            return -1; // não encontrada
        }

        public static int PosicaoKMP(StringBuilder sb, string pattern, int posicaoInicial = 0)
        {
            int[] lps = ComputeLPSArray(pattern);
            int i = posicaoInicial; // índice para sb
            int j = 0; // índice para pattern
            while (i < sb.Length)
            {
                if (pattern[j] == sb[i])
                {
                    i++;
                    j++;
                }

                if (j == pattern.Length)
                {
                    return i - j; // encontra correspondência
                }
                else if (i < sb.Length && pattern[j] != sb[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i = i + 1;
                }
            }
            return -1; // não encontrada
        }

        private static int[] ComputeLPSArray(string pattern)
        {
            int[] lps = new int[pattern.Length];
            int length = 0;
            int i = 1;
            lps[0] = 0;

            while (i < pattern.Length)
            {
                if (pattern[i] == pattern[length])
                {
                    length++;
                    lps[i] = length;
                    i++;
                }
                else
                {
                    if (length != 0)
                    {
                        length = lps[length - 1];
                    }
                    else
                    {
                        lps[i] = 0;
                        i++;
                    }
                }
            }
            return lps;
        }


    }
}