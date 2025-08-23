using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull
    {
        public static bool Execute(string[] array)
        {
            var vazio = EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array);
            return vazio;
        }
    }
}
