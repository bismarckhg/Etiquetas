using System;

namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class StringEmArrayStringPorSeparador
    {
        public static string[] Execute(string texto, string separador, bool removeEmptyEntries = false)
        {
            var separadorChars = Etiquetas.Bibliotecas.LibArrayChar.ConverteStringParaArrayChar.Execute(separador);
            var option = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var array = texto.Split(separadorChars, option);
            return array;
        }

        public static string[] Execute(string texto, string[] separadores, bool removeEmptyEntries = false)
        {
            //var separadorChars = ConverteStringParaArrayChar.Execute(separador);
            var option = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            var array = texto.Split(separadores, option);
            return array;
        }
    }
}
