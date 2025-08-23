using System;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringPossuiElementoDeArrayString
    {
        public static bool Execute(this string texto, string[] searchItens)
        {
            return Array.Exists(searchItens, element => texto.IndexOf(element) > -1);
        }
    }
}
