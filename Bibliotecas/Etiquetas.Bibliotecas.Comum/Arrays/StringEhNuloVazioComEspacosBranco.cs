using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Verifica se a string é nula, vazia ou composta apenas por espaços em branco.
    /// </summary>
    public static class StringEhNuloVazioComEspacosBranco
    {
        /// <summary>
        /// Verifica se a string é nula, vazia ou composta apenas por espaços em branco.
        /// </summary>
        /// <param name="texto">string com texto</param>
        /// <returns>true ou false se a string é nula, vazia ou composta apenas por espaços em branco.</returns>
        public static bool Execute(string texto)
        {
            return EhStringNuloVazioComEspacosBranco.Execute(texto);
        }
    }
}
