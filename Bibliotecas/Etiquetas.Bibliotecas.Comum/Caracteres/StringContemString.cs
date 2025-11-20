namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringContemString
    {
        public static bool Execute(string texto, string contem)
        {
            if (texto == null || contem == null)
            {
                return false;
            }
            return texto.Contains(contem);
        }
    }
}
