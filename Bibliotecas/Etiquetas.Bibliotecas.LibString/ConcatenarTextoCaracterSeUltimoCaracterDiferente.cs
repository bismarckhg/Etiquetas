namespace Etiquetas.Bibliotecas.LibString
{
    public static class ConcatenarTextoCaracterSeUltimoCaracterDiferente
    {
        public static string Execute(string texto, char caractere)
        {
            if (Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto) && Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(caractere.ToString()))
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
