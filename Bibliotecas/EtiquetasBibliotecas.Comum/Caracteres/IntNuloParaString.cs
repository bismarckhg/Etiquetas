namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class IntNuloParaString
    {
        public static string Execute(this int? value)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}
