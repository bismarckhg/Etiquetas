namespace Etiquetas.Bibliotecas.LibChar
{
    public static class CharSomenteLetra
    {
        public static bool Execute(char caractere)
        {
            return char.IsLetter(caractere);
        }
    }
}
