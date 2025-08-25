using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class OcorrenciasChar
    {
        public static int Execute(string texto, char caractere)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return 0;
            }
            return texto.Count(x => x == caractere);
        }
    }
}
