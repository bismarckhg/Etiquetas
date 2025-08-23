using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Arrays
{
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
            var textoVazio = StringEhNuloVazioComEspacosBranco.Execute(texto);
            var parametrosVazio = textoVazio && ArrayCharEhNuloVazioComEspacosBrancoDBNull.Execute(arrayChar);
            var textoNaoVazio = !textoVazio;
            var resultado = parametrosVazio || (textoNaoVazio && texto.Any(caractere => ArrayCharPossuiUmCaractere.Execute(arrayChar, caractere)));
            return resultado;
        }

    }
}
