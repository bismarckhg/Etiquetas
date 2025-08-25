namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class EhArrayStringNuloOuVazioOuComEspacosBranco
    {
       public static bool Execute(string[] array)
        {
            var vazio = EhArrayStringNuloVazioComEspacosBranco.Execute(array);
            return vazio;
        }
    }
}
