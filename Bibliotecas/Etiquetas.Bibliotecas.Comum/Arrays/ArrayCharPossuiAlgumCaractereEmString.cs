using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Verifica em string se possui algum caractere de char[] (array char).
    /// </summary>
    public static class ArrayCharPossuiAlgumCaractereEmString
    {
        /// <summary>
        /// Verifica em string se possui algum caractere de char[] (array char).
        /// </summary>
        /// <param name="arrayChar">
        /// Char[] (array char) com os caracteres de busca.
        /// </param>
        /// <param name="texto">
        /// String a ser verificada se possui algum caractere de char[] (array char).
        /// </param>
        /// <returns>
        /// True se a string tiver algum dos caracteres em char[] (array char). Caso contrario false.
        /// </returns>
        public static bool Execute(char[] arrayChar, string texto)
        {
            if (arrayChar == null || string.IsNullOrEmpty(texto))
            {
                return false;
            }

            return texto.Any(caractere => ArrayCharPossuiUmCaractere.Execute(arrayChar, caractere));
        }

    }
}
