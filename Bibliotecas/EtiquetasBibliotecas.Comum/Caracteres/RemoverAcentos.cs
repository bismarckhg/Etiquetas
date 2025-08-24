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
            if (string.IsNullOrEmpty(texto))
                return texto;

            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
