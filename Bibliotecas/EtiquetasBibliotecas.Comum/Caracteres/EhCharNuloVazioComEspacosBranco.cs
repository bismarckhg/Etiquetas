namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class EhCharNuloVazioComEspacosBranco
    {
        public static bool Execute(this char texto)
        {
            return EhStringNuloVazioComEspacosBrancoDBNull.Execute(texto.ToString());
        }
    }
}
