using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Streams.Interfaces
{
    /// <summary>
    /// Define o contrato para streams que requerem uma conexão explícita.
    /// </summary>
    public interface IStreamConexao : IStreamControle
    {
        /// <summary>
        /// Abre a conexão de forma assíncrona.
        /// </summary>
        /// <param name="parametros">Parâmetros necessários para a conexão, como endereço IP e porta.</param>
        /// <returns>Uma tarefa que representa a operação de conexão.</returns>
        Task ConectarAsync(params object[] parametros);
    }
}
