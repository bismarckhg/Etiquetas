using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Enums
{
    /// <summary>
    /// Categoriza os tipos de erros que podem ocorrer no sistema.
    /// </summary>
    public enum TipoErro
    {
        /// <summary>
        /// Erro relacionado a arquivos ou parâmetros de configuração.
        /// </summary>
        Configuracao,
        /// <summary>
        /// Erro relacionado à comunicação via pipe (conexão, etc.).
        /// </summary>
        Comunicacao,
        /// <summary>
        /// Erro relacionado a recursos do sistema (memória, disco, etc.).
        /// </summary>
        Recurso,
        /// <summary>
        /// Erro relacionado à autenticação ou permissões.
        /// </summary>
        Seguranca,
        /// <summary>
        /// Erro na lógica de negócio da tarefa.
        /// </summary>
        LogicaNegocio,
        /// <summary>
        /// Um erro que não se encaixa nas outras categorias.
        /// </summary>
        Indeterminado
    }
}
