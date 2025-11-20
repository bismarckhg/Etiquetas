using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringContemCaractere
    {
        public static bool Execute(this string texto, char caractere)
        {
            if (EhCharNuloVazioComEspacosBranco.Execute(caractere))
            {
                return false;
            }

            if (EhStringNuloVazioComEspacosBranco.Execute(texto))
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
