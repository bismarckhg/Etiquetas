using System;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class GuidEmString
    {
        /// <summary>
        /// Obtem Globally Unique Identifier - GUID e converte erm string
        /// </summary>
        /// <param name="guid">
        /// Globally Unique Identifier - GUID 
        /// </param>
        /// <returns>
        /// retorna Globally Unique Identifier - GUID em string
        /// </returns>
        public static string Execute(this Guid guid)
        {
            return guid.ToString();
        }
    }
}
