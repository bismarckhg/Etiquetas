using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    public static class ArrayCharPossuiUmCaractere
    {
        /// <summary>
        /// Verifica em char[] (Array char) informada pois um char(cararctere).
        /// </summary>
        /// <param name="arrayChar">
        /// Char[] (array char) a ser verificada.
        /// </param>
        /// <param name="caractere">
        /// Char a ser procurado no array.
        /// </param>
        /// <returns>
        /// True se a array char[] possuir o caractere informado. Caso contrario false.
        /// </returns>
        public static bool Execute(char[] arrayChar, char caractere)
        {
            if (arrayChar == null)
            {
                return false;
            }
            return arrayChar.Contains(caractere);
        }
    }
}
