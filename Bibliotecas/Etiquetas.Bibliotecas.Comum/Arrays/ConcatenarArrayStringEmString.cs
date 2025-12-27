namespace Etiquetas.Bibliotecas.Comum.Arrays
{
    /// <summary>
    /// Concatena um array de strings em uma única string.
    /// </summary>
    public static class ConcatenarArrayStringEmString
    {
        /// <summary>
        /// Concatena um array de strings em uma única string.
        /// </summary>
        /// <param name="array">Array de string a ser concatenada.</param>
        /// <returns>retorna string concatenada.</returns>
        public static string Execute(string[] array)
        {
            //if (Etiquetas.Bibliotecas.COLibArrayString.COEhArrayStringNuloOuVazioOuComEspacosBranco.CriaDicionario(array))
            if (EhArrayStringNuloVazioComEspacosBranco.Execute(array))
            {
                return string.Empty;
            }

            var retornoString = string.Concat(array);
            return retornoString;
        }
    }
}
