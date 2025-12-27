using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Exceção lançada quando um campo obrigatório não é encontrado ou está vazio.
    /// </summary>
    public class CampoObrigatorioException : Exception
    {
        /// <summary>
        /// Inicializa uma nova instância da exceção.
        /// </summary>
        /// <param name="mensagem">Mensagem descrevendo o erro</param>
        public CampoObrigatorioException(string mensagem) : base(mensagem)
        {
        }
    }
}
