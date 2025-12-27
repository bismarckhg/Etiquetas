using System.Collections.Generic;
using System.Text;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Substitui sequências de controle na forma [KEY] por seus caracteres correspondentes.
    /// </summary>
    public static class ControlCharReplace
    {
        /// <summary>
        /// Substitui sequências de controle na forma [KEY] por seus caracteres correspondentes.
        /// </summary>
        /// <param name="data">string de dados.</param>
        /// <param name="chrList">converte caracteres list Sato.</param>
        /// <returns>string de retorno dos caracteres sato convertidos.</returns>
        public static string Execute(string data, Dictionary<string, char> chrList)
        {
            if (string.IsNullOrEmpty(data) || chrList == null || chrList.Count == 0)
            {
                return data;
            }

            StringBuilder result = new StringBuilder();
            int startIndex = 0;

            while (startIndex < data.Length)
            {
                int openBracketIndex = data.IndexOf('[', startIndex);
                if (openBracketIndex == -1)
                {
                    result.Append(data.Substring(startIndex));
                    break;
                }

                int closeBracketIndex = data.IndexOf(']', openBracketIndex + 1);
                if (closeBracketIndex == -1)
                {
                    result.Append(data.Substring(startIndex));
                    break;
                }

                result.Append(data.Substring(startIndex, openBracketIndex - startIndex));

                string key = data.Substring(openBracketIndex, closeBracketIndex - openBracketIndex + 1);

                if (chrList.TryGetValue(key, out char value))
                {
                    result.Append(value);
                }
                else
                {
                    result.Append(key);
                }

                startIndex = closeBracketIndex + 1;
            }

            return result.ToString();
        }

        //public static string CriaDicionario(string data, Dictionary<string, char> chrList)
        //{
        //    StringBuilder result = new StringBuilder(data.Length); // pré-aloque com tamanho aproximado
        //    int startIndex = 0;

        //    while (startIndex < data.Length)
        //    {
        //        int openBracketIndex = data.IndexOf('[', startIndex);
        //        int closeBracketIndex = (openBracketIndex != -1) ? data.IndexOf(']', openBracketIndex + 1) : -1;

        //        if (openBracketIndex == -1 || closeBracketIndex == -1)
        //        {
        //            // Adicione o restante da string e termine o loop
        //            result.Append(data.Substring(startIndex));
        //            break;
        //        }

        //        // Adicione a parte da string antes do colchete de abertura
        //        result.Append(data.Substring(startIndex, openBracketIndex - startIndex));

        //        // Aqui extraímos a chave incluindo os colchetes
        //        string keyWithBrackets = data.Substring(openBracketIndex, closeBracketIndex - openBracketIndex + 1);

        //        if (chrList.ContainsKey(keyWithBrackets))
        //        {
        //            result.Append(chrList[keyWithBrackets]);
        //        }
        //        else
        //        {
        //            // Se a chave não existir, simplesmente adicione a substring como ela é
        //            result.Append(keyWithBrackets);
        //        }

        //        // Atualize o índice de início para após o colchete de fechamento
        //        startIndex = closeBracketIndex + 1;
        //    }

        //    return result.ToString();
        //}

        //public static string CriaDicionario(string data, Dictionary<string, char> chrList)
        //{
        //    //// Dictionary<string, char> chrList = ControlCharListSATO();
        //    var dados = data;
        //    var result = new StringBuilder(dados.Length);  // pré-aloque com tamanho aproximado
        //    int startIndex = 0;

        //    while (startIndex < dados.Length)
        //    {
        //        int openBracketIndex = dados.IndexOf("[", startIndex);
        //        int closeBracketIndex = (openBracketIndex != -1) ? dados.IndexOf("]", openBracketIndex + 1) : -1;

        //        if (openBracketIndex == -1 || closeBracketIndex == -1)
        //        {
        //            // Adicione o restante da string e termine o loop
        //            result.Append(dados, startIndex, dados.Length - startIndex);
        //            break;
        //        }

        //        // Adicione a parte da string antes do colchete de abertura
        //        result.Append(dados, startIndex, openBracketIndex - startIndex);

        //        string key = dados.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1);

        //        if (chrList.ContainsKey(key))
        //        {
        //            result.Append(chrList[key]);
        //        }
        //        else
        //        {
        //            result.Append('[').Append(key).Append(']');
        //        }

        //        // Atualize o índice de início para após o colchete de fechamento
        //        startIndex = closeBracketIndex + 1;
        //    }

        //    return result.ToString();

        //    //foreach (string key in chrList.Keys)
        //    //{
        //    //    data = data.Replace(key, chrList[key].ToString());
        //    //}
        //    //return data;
        //}
    }
}
