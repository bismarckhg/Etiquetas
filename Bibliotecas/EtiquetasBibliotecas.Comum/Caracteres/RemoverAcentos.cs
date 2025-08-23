using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class RemoverAcentos
    {
        /// <summary>
        /// Remove da string os caracteres especiais e de acentuação, que compartilham a iso-8859-8. São retirados ao
        /// converter o string em UTF8.
        /// </summary>
        /// <param name="texto">
        /// Texto informado com caracteres especiais e/ou de acentuação.
        /// </param>
        /// <returns>
        /// Retorna string sem os caracteres especiais e/ou acentuação. Texto string retornado em UTF-8.
        /// </returns>
        public static string Execute(this string texto)
        {
            if (EhStringNuloVazioComEspacosBranco.Execute(texto))
            {
                return texto;
            }
            else
            {
                //byte[]
                var bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(texto);
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}
