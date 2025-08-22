namespace Etiquetas.Bibliotecas
{
    public static class EhCharNuloVazioComEspacosBranco
    {
        public static bool Execute(this char texto)
        {
            return Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBrancoDBNull.Execute(texto.ToString());
        }
    }
}
