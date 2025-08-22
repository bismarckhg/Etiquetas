using System.Linq;

namespace Etiquetas.Bibliotecas.LibString
{
    public static class StringContemCaractere
    {
        public static bool Execute(this string texto, char caractere)
        {
            if (Etiquetas.Bibliotecas.EhCharNuloVazioComEspacosBranco.Execute(caractere))
            {
                return false;
            }

            if (Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto))
            {
                return false;
            }

            if (texto.Contains(caractere))
            {
                return true;
            }

            return false;
        }
    }
}
