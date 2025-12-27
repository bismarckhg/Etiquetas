using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Converte uma string em um array de strings, utilizando um separador especificado.
    /// </summary>
    public static class StringEmArrayStringPorSeparador
    {
        /// <summary>
        /// Converte uma string em um array de strings, utilizando um separador especificado.
        /// </summary>
        /// <param name="texto">texto string a ser dividido em array string.</param>
        /// <param name="separador">separador(split) de divisão do texto em array string.</param>
        /// <param name="removeEmptyEntries">se true remove os arrays string vazios.</param>
        /// <returns>retorna o array string dividido.</returns>
        public static string[] Execute(string texto, string separador, bool removeEmptyEntries = true)
        {
            var separadorChars = ConverteStringParaArrayChar.Execute(separador);
            var option = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var array = texto.Split(separadorChars, option);
            return array;
        }

        /// <summary>
        /// Converte uma string em um array de strings, utilizando um array de separadores especificados. 
        /// </summary>
        /// <param name="texto">texto string a ser dividido em array string.</param>
        /// <param name="separador">separador(split) de divisão do texto em array string.</param>
        /// <param name="removeEmptyEntries">se true remove os arrays string vazios.</param>
        /// <returns>retorna o array string dividido.</returns>
        public static string[] Execute(string texto, string[] separadores, bool removeEmptyEntries = true)
        {
            //var separadorChars = ConverteStringParaArrayChar.CriaDicionario(separador);
            var option = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var array = texto.Split(separadores, option);
            return array;
        }

        /// <summary>
        /// Converte uma string em um array de strings, utilizando um array de caracteres separadores especificados.
        /// </summary>
        /// <param name="texto">texto string a ser dividido em array string.</param>
        /// <param name="separador">separador(split) de divisão do texto em array string.</param>
        /// <param name="removeEmptyEntries">se true remove os arrays string vazios.</param>
        /// <returns>retorna o array string dividido.</returns>
        public static string[] Execute(string texto, char[] separadores, bool removeEmptyEntries = true)
        {
            //var separadorChars = ConverteStringParaArrayChar.CriaDicionario(separador);
            var option = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var array = texto.Split(separadores, option);
            return array;
        }
    }
}
