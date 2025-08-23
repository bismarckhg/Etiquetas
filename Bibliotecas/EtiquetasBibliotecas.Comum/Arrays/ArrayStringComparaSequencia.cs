using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class ArrayStringComparaSequencia
    {

        public static bool Execute(string[] array1, string[] array2)
        {
            var array1Vazio = EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array1);
            var array2Vazio = EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array2);

            if (array1Vazio && array2Vazio)
            {
                return true;
            }

            if (array1Vazio || array2Vazio)
            {
                return false;
            }

            return array1.SequenceEqual(array2);
        }
    }
}
