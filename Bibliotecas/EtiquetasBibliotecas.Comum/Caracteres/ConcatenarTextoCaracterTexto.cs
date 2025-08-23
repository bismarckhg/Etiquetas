namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConcatenarTextoCaracterTexto
    {
        public static string Execute(string texto1, char caractere, string texto2)
        {
            return $"{texto1}{caractere}{texto2}";
        }
    }
}
