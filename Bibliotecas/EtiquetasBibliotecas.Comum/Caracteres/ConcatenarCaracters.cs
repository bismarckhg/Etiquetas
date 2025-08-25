namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConcatenarCaracters
    {
        public static string Execute(params char[] caracteres)
        {
            var resultado = ConcatenarArrayCaracteresEmString.Execute(caracteres);
            return resultado;
        }

    }
}
