namespace Etiquetas.Bibliotecas.Comum.Caracteres
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
