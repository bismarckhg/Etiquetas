using Etiquetas.Bibliotecas;
using System.Linq;

namespace Etiquetas.Bibliotecas.LibArrayChar
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
            var texto = caractere.ToString();
            var caractereVazio = Etiquetas.Bibliotecas.EhStringNuloVazioComEspacosBranco.Execute(texto);
            var parametrosVazio = caractereVazio && Etiquetas.Bibliotecas.EhArrayCharNuloVazioComEspacosBranco.Execute(arrayChar);
            var caracterNaoVazio = !caractereVazio;
            var resultado = parametrosVazio || (caracterNaoVazio && arrayChar.Any(caracter => caracter.Equals(caractere)));
            return resultado;
        }
    }
}
