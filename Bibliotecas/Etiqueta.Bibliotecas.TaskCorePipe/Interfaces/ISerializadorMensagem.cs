namespace Etiqueta.Bibliotecas.TaskCorePipe.Interfaces
{
    /// <summary>
    /// Define o contrato para serialização e deserialização de mensagens trocadas via pipe.
    /// </summary>
    public interface ISerializadorMensagem
    {
        /// <summary>
        /// Serializa um objeto para uma representação em string (ex: JSON).
        /// </summary>
        /// <typeparam name="T">O tipo do objeto a ser serializado.</typeparam>
        /// <param name="objeto">O objeto a ser serializado.</param>
        /// <returns>A representação do objeto em string.</returns>
        string Serializar<T>(T objeto);

        /// <summary>
        /// Deserializa uma string para um objeto do tipo especificado.
        /// </summary>
        /// <typeparam name="T">O tipo do objeto de destino.</typeparam>
        /// <param name="dados">A string a ser deserializada.</param>
        /// <returns>O objeto deserializado.</returns>
        T Deserializar<T>(string dados);
    }
}
