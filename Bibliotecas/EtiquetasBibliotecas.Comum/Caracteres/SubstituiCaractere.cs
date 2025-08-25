namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class SubstituiCaractere
    {
        public static string Execute(string texto, char caractere, char novoCaractere)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }
            return texto.Replace(caractere, novoCaractere);
        }
    }
}
