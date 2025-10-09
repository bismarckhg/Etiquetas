using System;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Streams.Interfaces
{
    /// <summary>
    /// Define os contratos básicos de controle para operações de stream.
    /// </summary>
    public interface IStreamControle : IDisposable
    {
        /// <summary>
        /// Verifica se o stream está atualmente aberto ou conectado.
        /// </summary>
        /// <returns>Verdadeiro se estiver aberto/conectado, caso contrário, falso.</returns>
        bool EstaAberto();

        /// <summary>
        /// Verifica se há dados disponíveis para leitura no stream.
        /// </summary>
        /// <returns>Verdadeiro se houver dados para ler, caso contrário, falso.</returns>
        bool PossuiDados();

        /// <summary>
        /// Fecha a conexão ou o stream de forma assíncrona.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação de fechamento.</returns>
        Task FecharAsync();
    }
}
