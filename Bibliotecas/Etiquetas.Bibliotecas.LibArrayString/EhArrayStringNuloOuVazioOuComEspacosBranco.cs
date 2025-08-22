namespace Etiquetas.Bibliotecas.LibArrayString
{
    public static class EhArrayStringNuloOuVazioOuComEspacosBranco
    {
       public static bool Execute(string[] array)
        {
            var vazio = Etiquetas.Bibliotecas.EhArrayStringNuloVazioComEspacosBranco.Execute(array);
            return vazio;
        }
    }
}
