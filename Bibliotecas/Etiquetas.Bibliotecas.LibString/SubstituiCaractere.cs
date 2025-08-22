namespace Etiquetas.Bibliotecas.LibString
{
    public static class SubstituiCaractere
    {
        public static string Execute(string texto, char caractere, char novoCaractere)
        {
            return texto.Replace(caractere.ToString(), novoCaractere.ToString());
        }
    }
}
