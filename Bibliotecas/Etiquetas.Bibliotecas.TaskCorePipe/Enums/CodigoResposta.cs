using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Enums
{
    /// <summary>
    /// Define códigos de resposta padronizados, inspirados em HTTP status codes,
    /// para indicar o resultado de uma operação via pipe.
    /// </summary>
    public enum CodigoResposta
    {
        // Sucesso 2xx
        /// <summary>
        /// O comando foi executado com sucesso. (Análogo a HTTP 200 OK)
        /// </summary>
        Success = 200,
        /// <summary>
        /// O comando foi aceito para processamento, mas a conclusão não é imediata. (Análogo a HTTP 202 Accepted)
        /// </summary>
        Accepted = 202,
        /// <summary>
        /// O comando foi executado com sucesso, mas não há conteúdo para retornar. (Análogo a HTTP 204 No Content)
        /// </summary>
        NoContent = 204,

        // Erro Cliente 4xx
        /// <summary>
        /// A requisição está malformada ou contém parâmetros inválidos. (Análogo a HTTP 400 Bad Request)
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// A tarefa ou recurso especificado não foi encontrado. (Análogo a HTTP 404 Not Found)
        /// </summary>
        TaskNotFound = 404,
        /// <summary>
        /// O tempo para processar a requisição expirou. (Análogo a HTTP 408 Request Timeout)
        /// </summary>
        RequestTimeout = 408,
        /// <summary>
        /// O comando não pôde ser executado devido a um conflito com o estado atual do recurso. (Análogo a HTTP 409 Conflict)
        /// </summary>
        Conflict = 409,

        // Erro Servidor 5xx
        /// <summary>
        /// Ocorreu um erro interno inesperado no servidor/tarefa. (Análogo a HTTP 500 Internal Server Error)
        /// </summary>
        InternalServerError = 500,
        /// <summary>
        /// O serviço está temporariamente indisponível. (Análogo a HTTP 503 Service Unavailable)
        /// </summary>
        ServiceUnavailable = 503,
        /// <summary>
        /// A comunicação com um serviço upstream falhou. (Análogo a HTTP 504 Gateway Timeout)
        /// </summary>
        GatewayTimeout = 504
    }
}
