using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class OcorrenciasChar
    {
        public static int Execute(string texto, char caractere)
        {
            int resultado = texto.Count(x => x.Equals(caractere));
            return resultado;
        }
    }
}
