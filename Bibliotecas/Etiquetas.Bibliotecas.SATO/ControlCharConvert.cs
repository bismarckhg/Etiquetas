using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Converte caracteres de controle para suas representações em string.
    /// </summary>
    public static class ControlCharConvert
    {
        /// <summary>
        /// Converte caracteres de controle(especiais) para suas representações em string.
        /// </summary>
        /// <param name="data">string de dados a serem convertidos.</param>
        /// <param name="chrList">dicionario string de caracteres especiais a serem trocados no string.</param>
        /// <returns>string com caracteres de controles inclusos.</returns>
        public static string Execute(string data, Dictionary<string, char> chrList)
        {
            if (data == null)
            {
                return null;
            }

            if (chrList == null)
            {
                return data;
            }

            // Inverte o dicionário para acessar pela chave de caractere
            var codeList = chrList.ToDictionary(x => x.Value, x => x.Key);

            // Usa StringBuilder para construir a nova string
            StringBuilder result = new StringBuilder();

            // Itera sobre cada caractere na string de entrada
            foreach (char ch in data)
            {
                if (codeList.ContainsKey(ch))
                {
                    // Se o caractere está no dicionário, adiciona a string correspondente ao resultado
                    result.Append(codeList[ch]);
                }
                else
                {
                    // Caso contrário, adiciona o próprio caractere
                    result.Append(ch);
                }
            }

            return result.ToString();
        }


        //public static string CriaDicionario(string data, Dictionary<string, char> chrList)
        //{
        //    // Dictionary<char, string> chrList = ControlCharListSATO().ToDictionary(x => x.Value, x => x.Key);
        //    var codeList = chrList.ToDictionary(x => x.Value, x => x.Key);
        //    char[] keys = codeList.Keys.ToArray();

        //    var dados = data;
        //    int index;

        //    do
        //    {
        //        index = dados.IndexOfAny(keys);
        //        if (index > -1)
        //        {
        //            var chr = dados[index];
        //            dados = dados.Replace(chr.ToString(), codeList[chr]);
        //        }
        //    } while (index != -1);

        //    return dados;
        //    //foreach (char key in codeList.Keys)
        //    //{
        //    //    data = data.Replace(key.ToString(), codeList[key]);
        //    //}
        //    //return data;
        //}
    }
}
