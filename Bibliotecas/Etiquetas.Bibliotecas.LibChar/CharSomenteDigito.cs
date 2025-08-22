namespace Etiquetas.Bibliotecas.LibChar
{
    public static class CharSomenteDigito
    {
        public static bool Execute(char caractere)
        {
            return char.IsDigit(caractere);
        }

        public static bool Execute(char? caractere)
        {
            if (caractere == null)
            {
                return false;
            }
            return char.IsDigit((char)caractere);
        }
    }
}
