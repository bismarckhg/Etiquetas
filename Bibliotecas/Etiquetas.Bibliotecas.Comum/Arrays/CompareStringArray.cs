using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class CompareStringArray
    {
        public static bool Execute(object array1, object array2)
        {
            if (array1 is string[] arrayString1)
            {
                if (array2 is string[] arrayString2)
                {
                    bool retorno = arrayString1.SequenceEqual(arrayString2);
                    return retorno;
                }
            }
            return false;
        }

    }
}
