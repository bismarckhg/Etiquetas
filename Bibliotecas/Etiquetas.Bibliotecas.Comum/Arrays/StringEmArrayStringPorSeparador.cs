using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringEmArrayStringPorSeparador
    {
        public static string[] Execute(string texto, string separador, bool removeEmptyEntries = true)
        {
            var separadorChars = ConverteStringParaArrayChar.Execute(separador);
            var option = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var array = texto.Split(separadorChars, option);
            return array;
        }

        public static string[] Execute(string texto, string[] separadores, bool removeEmptyEntries = true)
        {
            //var separadorChars = ConverteStringParaArrayChar.Execute(separador);
            var option = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var array = texto.Split(separadores, option);
            return array;
        }

        public static string[] Execute(string texto, char[] separadores, bool removeEmptyEntries = true)
        {
            //var separadorChars = ConverteStringParaArrayChar.Execute(separador);
            var option = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var array = texto.Split(separadores, option);
            return array;
        }

    }
}
