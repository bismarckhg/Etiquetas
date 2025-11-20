namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class EhCharNuloVazioComEspacosBrancoDBNull
    {
        public static bool Execute(this char texto)
        {
            return char.IsWhiteSpace(texto);
        }

        public static bool Execute(this System.DBNull texto)
        {
            return true;
        }
    }
}
