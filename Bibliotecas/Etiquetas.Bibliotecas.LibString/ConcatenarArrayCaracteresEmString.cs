namespace Etiquetas.Bibliotecas.LibString
{
    public static class ConcatenarArrayCaracteresEmString
    {
        public static string Execute(char[] array)
        {
            string retornoString = string.Concat(new string(array));
            return retornoString;
        }
    }
}
