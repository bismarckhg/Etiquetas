using System.Collections.Generic;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ControlCharReplace
    {
        public static string Execute(string data, Dictionary<string, char> chrList)
        {
            var result = new StringBuilder(data);
            int startIndex = 0;

            while (startIndex < data.Length)
            {
                int openBracketIndex = IndexOf.PosicaoKMP(result, "[", startIndex);
                int closeBracketIndex = (openBracketIndex != -1) ? IndexOf.PosicaoKMP(result, "]", openBracketIndex + 1) : -1;
                var procura = new StringBuilder();

                if (openBracketIndex == -1 || closeBracketIndex == -1)
                {
                    break;
                }

                for (int i = openBracketIndex; i < (closeBracketIndex + 1); i++)
                {
                    procura.Append(result[i]);
                }

                result.Replace(procura.ToString(), chrList[procura.ToString()].ToString());

                // Atualize o índice de início para após o colchete de fechamento
                startIndex = closeBracketIndex + 1;
            }

            return result.ToString();
        }


        //public static string Execute(string data, Dictionary<string, char> chrList)
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

        //public static string Execute(string data, Dictionary<string, char> chrList)
        //{
        //    //// Dictionary<string, char> chrList = ControlCharList();
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
