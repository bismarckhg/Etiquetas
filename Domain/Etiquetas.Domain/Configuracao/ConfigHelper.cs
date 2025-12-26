using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Configuracao
{
    /// <summary>
    /// Helper para obter valores de configuração do arquivo appsettings.xml.
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Recupera o valor associado à chave de configurações do aplicativo especificada no arquivo de configuração.
        /// </summary>
        /// <remarks>Este método lê a seção AppSettings do aplicativo no arquivo de configuração.
        /// A pesquisa diferencia maiúsculas de minúsculas. Se a chave não for encontrada, o método retorna nulo.</remarks>
        /// <param name="chave">A chave da configuração do aplicativo a ser recuperada. Não pode ser nula.</param>
        /// <returns>O valor associado à chave especificada ou nulo se a chave não existir.</returns>
        public static string ObterValor(string chave) => ConfigurationManager.AppSettings[chave];
    }
}
