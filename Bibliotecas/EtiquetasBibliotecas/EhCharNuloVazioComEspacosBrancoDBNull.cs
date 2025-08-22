namespace Etiquetas.Bibliotecas
{
    public static class EhCharNuloVazioComEspacosBrancoDBNull
    {
        public static bool Execute(this char texto)
        {
            return Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto.ToString());
        }

        public static bool Execute(this System.DBNull texto)
        {
            return (texto == System.DBNull.Value) || (texto is null);
        }
    }
}
