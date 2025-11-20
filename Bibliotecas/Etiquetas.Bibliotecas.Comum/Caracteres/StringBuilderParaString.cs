using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class StringBuilderParaString
    {
        /// <summary>
        /// Converte StringBuilder para String
        /// </summary>
        /// <param name="texto">
        /// String a receber outra string entre seus caracteres(letras).
        /// </param>
        /// <returns>
        /// Retorna string convertida da StringBuilder
        /// </returns>
        public static string Execute(this StringBuilder texto)
        {
            return texto.ToString();
        }

    }
}
