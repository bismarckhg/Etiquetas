namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class ConcatenarArrayCaractersEmString
    {
        public static string Execute(char[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }
            var retornoString = string.Concat(array);
            return retornoString;
        }

    }
}
