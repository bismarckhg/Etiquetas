using System;

namespace Etiquetas.Bibliotecas.LibString
{
    public static class StringPossuiElementoDeArrayString
    {
        public static bool Execute(this string texto, string[] searchItens)
        {
            return Array.Exists(searchItens, element => texto.IndexOf(element) > -1);
        }
    }
}
