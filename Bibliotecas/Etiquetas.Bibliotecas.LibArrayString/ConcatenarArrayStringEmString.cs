using System.Text;

namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class ConcatenarArrayStringEmString
    {
        public static string Execute(string[] array)
        {
            if ((EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array)))
                return string.Empty;
            var retornoString = string.Concat(array);
            return retornoString;
        }
    }

}
