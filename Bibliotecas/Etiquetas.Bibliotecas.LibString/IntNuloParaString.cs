namespace Etiquetas.Bibliotecas.LibString
{
    public static class IntNuloParaString
    {
        public static string Execute(this int? value)
        {
            var retornoString = value == null ? string.Empty : value?.ToString();
            return retornoString;
        }
    }
}
