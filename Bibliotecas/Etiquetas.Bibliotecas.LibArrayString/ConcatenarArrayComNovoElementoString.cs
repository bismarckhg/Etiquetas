using System.Linq;

namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class ConcatenarArrayComNovoElementoString
    {
        public static string[] Execute(string[] array, string elemento)
        {
            if (EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array))
            {
                var arrayNovo = new string[] { elemento };
                return arrayNovo;
            }
            var novoArray = array.Concat(new string[] { elemento }).ToArray();
            return novoArray;
        }
    }
}