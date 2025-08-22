namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull
    {
        public static bool Execute(string[] array)
        {
            var vazio = Etiquetas.Bibliotecas.EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array);
            return vazio;
        }
    }
}
