using System.Linq;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class StringArrayContemElementoString
    {
        public static bool Execute(string[] array, string contem)
        {
            var contemVazio = EhStringNuloVazioComEspacosBrancoDBNull.Execute(contem);
            var parametrosVazio = contemVazio && EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array);
            var contemNaoVazio = !contemVazio;
            var contemString = parametrosVazio || (contemNaoVazio && array.Any(x => string.Compare(x, contem).Equals(0)));
            return contemString;
        }
    }
}
