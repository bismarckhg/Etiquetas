namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConcatenarTextoCaracterSeUltimoCaracterDiferente
    {
        public static string Execute(string texto, char caractere)
        {
            if (EhStringNuloVazioComEspacosBranco.Execute(texto) && EhStringNuloVazioComEspacosBranco.Execute(caractere.ToString()))
            {
                return null;
            }
            if ((ExtrairTextoDireita.Execute(texto, 1) ?? "") != ($"{caractere}" ?? ""))
            {
                return $"{texto}{caractere}";
            }
            return $"{texto}";
        }

    }
}
