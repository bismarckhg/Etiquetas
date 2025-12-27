using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    /// <summary>
    /// Classe responsável por concatenar vários textos em uma unica string.
    /// </summary>
    public static class ConcatenarTexto
    {
        /// <summary>
        /// Concatena string's em uma unica string.
        /// </summary>
        /// <param name="texto">
        /// Um ou mais parametros String para concatenar em outra string.
        /// </param>
        /// <returns>
        /// Retorna string concatenada por 1 ou mais parametros.
        /// </returns>
        public static string Execute(params string[] texto)
        {
            var resultado = ConcatenarArrayStringEmString.Execute(texto);
            return resultado;
        }
    }
}
