namespace Etiquetas.Bibliotecas
{
    public static class ConcatenarArrayStringEmString
    {
        public static string Execute(string[] array)
        {
            //if (Etiquetas.Bibliotecas.COLibArrayString.COEhArrayStringNuloOuVazioOuComEspacosBranco.Execute(array))
            if (EhArrayStringNuloVazioComEspacosBranco.Execute(array))
            {
                return string.Empty;
            }
            var retornoString = string.Concat(array);
            return retornoString;
        }
    }
}
