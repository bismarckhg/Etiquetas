using System;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringPossuiElementoDeArrayString
    {
        public static bool Execute(this string texto, string[] searchItens)
        {
            if (string.IsNullOrEmpty(texto) || searchItens == null || searchItens.Length == 0)
            {
                return false;
            }

            return Array.Exists(searchItens, element => element != null && texto.Contains(element));
        }
    }
}
