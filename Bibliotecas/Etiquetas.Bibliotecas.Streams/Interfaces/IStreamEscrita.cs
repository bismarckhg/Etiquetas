using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Streams.Interfaces
{
    /// <summary>
    /// Define o contrato para streams que realizam operações de escrita.
    /// </summary>
    /// <typeparam name="T">O tipo de dado a ser escrito.</typeparam>
    public interface IStreamEscrita<T> : IStreamControle
    {
        /// <summary>
        /// Escreve dados no stream de forma assíncrona.
        /// </summary>
        /// <param name="dados">O objeto de dados a ser escrito.</param>
        /// <param name="parametros">Parâmetros adicionais para a operação de escrita.</param>
        /// <returns>Uma tarefa que representa a operação de escrita.</returns>
        Task EscreverAsync(T dados, params object[] parametros);
    }
}
