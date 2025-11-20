using System;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class RemoverEspacosEntrePalavras
    {
        /// <summary>
        /// Remove os espaços entre as palavras da string informada.
        /// </summary>
        /// <param name="texto">
        /// String informada com espaços entre palavras.
        /// </param>
        /// <returns>
        /// Retorna string sem os espaços entre palavras.
        /// </returns>
        public static string Execute(this string texto)
        {
            //string[] 
            var partes = texto.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            var resultado = string.Join(" ", partes);
            return resultado;
        }
    }
}
