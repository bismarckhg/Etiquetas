namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConcatenarTextoCaracterSeUltimoCaracterDiferente
    {
        public static string Execute(string texto, char caractere)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return caractere.ToString();
            }
            if (texto[texto.Length - 1] != caractere)
            {
                return texto + caractere;
            }
            return texto;
        }

    }
}
