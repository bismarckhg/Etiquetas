namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConcatenarArrayCaracteresEmString
    {
        public static string Execute(char[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }
            return new string(array);
        }
    }
}
