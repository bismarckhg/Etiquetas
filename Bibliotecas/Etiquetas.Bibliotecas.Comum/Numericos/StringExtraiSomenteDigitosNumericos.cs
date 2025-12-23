using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Numericos
{
    /// <summary>
    /// Extrai somente os dígitos numéricos de uma string.
    /// </summary>
    public static class StringExtrairSomenteDigitosNumericos
    {
        /// <summary>
        /// Extrai somente os dígitos numéricos de uma string.
        /// </summary>
        /// <param name="s">string contendo os digitos numéricos.</param>
        /// <returns>retorna a string somente com os digitos numéricos.</returns>
        public static string Execute(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            var sb = new StringBuilder(s.Length); // já evita realocações
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if ((uint)(ch - '0') <= 9) // ‘0’..‘9’ sem chamar IsDigit
                    sb.Append(ch);
            }
            return sb.ToString();
        }
    }
}
