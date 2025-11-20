namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class InserirString
    {
        /// <summary>
        /// Inserir string em outra string na posicao informada.
        /// </summary>
        /// <param name="texto">
        /// Texto string a receber outra string.
        /// </param>
        /// <param name="stringAInserir">
        /// String a ser inserida em outra string.
        /// </param>
        /// <param name="posicao">
        /// Inteiro com a posicao no texto informado para inserir outro texto.
        /// </param>
        /// <returns>
        /// Retorna string infromada com a string inserida na posicao.
        /// </returns>
        public static string Execute(this string texto, string stringAInserir, int posicao)
        {
            if (texto == null)
            {
                return null;
            }
            if (string.IsNullOrEmpty(stringAInserir))
            {
                return texto;
            }
            if (posicao < 0 || posicao > texto.Length)
            {
                return texto;
            }
            return texto.Insert(posicao, stringAInserir);
        }
    }
}