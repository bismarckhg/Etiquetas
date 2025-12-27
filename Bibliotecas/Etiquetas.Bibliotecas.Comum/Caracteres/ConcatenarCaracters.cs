namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Classe responsável por concatenar vários caracteres em uma string.
    /// </summary>
    public static class ConcatenarCaracters
    {
        /// <summary>
        /// Concatena vários caracteres em uma string.
        /// </summary>
        /// <param name="caracteres">array de carcateres</param>
        /// <returns>string de caracteres concatenada.</returns>
        public static string Execute(params char[] caracteres)
        {
            var resultado = ConcatenarArrayCaracteresEmString.Execute(caracteres);
            return resultado;
        }

    }
}
