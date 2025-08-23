using System.Linq;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringArrayContemElementoString
    {
        public static bool Execute(string[] array, string contem)
        {
            if (EhStringNuloVazioComEspacosBrancoDBNull.Execute(contem))
            {
                return EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array);
            }

            if (EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array))
            {
                return false;
            }

            return array.Any(x => string.Compare(x, contem, System.StringComparison.Ordinal) == 0);
        }
    }
}
