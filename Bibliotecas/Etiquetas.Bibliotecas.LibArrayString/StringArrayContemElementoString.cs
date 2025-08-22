using System.Linq;

namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class StringArrayContemElementoString
    {
        public static bool Execute(string[] array, string contem)
        {
            var contemVazio = Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBrancoDBNull.Execute(contem);
            var parametrosVazio = contemVazio && EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array);
            var contemNaoVazio = !contemVazio;
            var contemString = parametrosVazio || (contemNaoVazio && array.Any(x => string.Compare(x, contem).Equals(0)));
            return contemString;
        }
    }
}
