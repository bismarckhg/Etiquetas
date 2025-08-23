using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class ArrayStringComparaSequencia
    {

        public static bool Execute(string[] array1, string[] array2)
        {
            var array2Vazio = EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array2);
            var parametrosVazio = array2Vazio && EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array1);
            var array2NaoVazio = !array2Vazio;
            var resultado = parametrosVazio || (array2NaoVazio && array1.SequenceEqual(array2));
            return resultado;
        }
    }
}
