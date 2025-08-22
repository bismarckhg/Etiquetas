namespace Etiquetas.Bibliotecas.LibString
{
    public static class ConcatenarTextoCaracterTexto
    {
        public static string Execute(string texto1, char caractere, string texto2)
        {
            return $"{texto1}{caractere}{texto2}";
        }
    }
}
