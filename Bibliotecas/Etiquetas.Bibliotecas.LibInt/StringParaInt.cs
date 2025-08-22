namespace Etiquetas.Bibliotecas.LibInt
{
    public static class StringParaInt
    {
        public static int Execute(this string value)
        {
            var valido = int.TryParse(value, out int result);
            if (valido)
            {
                return result;
            }
            return default;
        }
    }
}