using Etiqueta.Bibliotecas.TaskCorePipe.Interfaces;
using Newtonsoft.Json;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Comunicacao
{
    /// <summary>
    /// Implementação de um serializador de mensagens usando Newtonsoft.Json.
    /// </summary>
    public class SerializadorJson : ISerializadorMensagem
    {
        private readonly JsonSerializerSettings _settings;

        /// <summary>
        /// Construtor que inicializa as configurações do serializador JSON.
        /// </summary>
        public SerializadorJson()
        {
            _settings = new JsonSerializerSettings
            {
                // Ignora loops de referência, útil para objetos complexos.
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                // Formatação para depuração, pode ser alterado para None em produção.
                Formatting = Formatting.Indented,
                // Preserva a informação de tipo, útil para deserializar objetos polimórficos (como o Payload).
                TypeNameHandling = TypeNameHandling.Objects
            };
        }

        /// <summary>
        /// Deserializa uma string JSON para um objeto do tipo especificado.
        /// </summary>
        /// <typeparam name="T">O tipo do objeto de destino.</typeparam>
        /// <param name="dados">A string JSON a ser deserializada.</param>
        /// <returns>O objeto deserializado.</returns>
        public T Deserializar<T>(string dados)
        {
            return JsonConvert.DeserializeObject<T>(dados, _settings);
        }

        /// <summary>
        /// Serializa um objeto para uma representação em string JSON.
        /// </summary>
        /// <typeparam name="T">O tipo do objeto a ser serializado.</typeparam>
        /// <param name="objeto">O objeto a ser serializado.</param>
        /// <returns>A representação do objeto em string JSON.</returns>
        public string Serializar<T>(T objeto)
        {
            return JsonConvert.SerializeObject(objeto, typeof(T), _settings);
        }
    }
}
