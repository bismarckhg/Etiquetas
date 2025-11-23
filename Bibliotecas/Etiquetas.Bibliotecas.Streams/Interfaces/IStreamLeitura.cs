using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Streams.Interfaces
{
    /// <summary>
    /// Define o contrato para streams que realizam operações de leitura.
    /// </summary>
    /// <typeparam name="T">O tipo de dado a ser lido.</typeparam>
    public interface IStreamLeitura : IStreamControle
    {
        /// <summary>
        /// Lê dados do stream de forma assíncrona.
        /// </summary>
        /// <param name="parametros">Parâmetros adicionais para a operação de leitura.</param>
        /// <returns>Uma tarefa que representa a operação de leitura, retornando o objeto lido.</returns>
        Task<T> LerAsync<T>(ITaskParametros parametros);
    }
}
