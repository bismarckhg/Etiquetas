namespace Etiquetas.Bibliotecas.LibInt
{
    public static class StringParaIntNulo
    {
        public static int? Execute(this string value)
        {
            var valido = int.TryParse(value, out int result);
            if (valido)
            {
                return result;
            }
            return null;
        }
    }
}
