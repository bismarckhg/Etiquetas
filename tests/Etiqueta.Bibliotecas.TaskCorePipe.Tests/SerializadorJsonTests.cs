using Etiqueta.Bibliotecas.TaskCorePipe.Comunicacao;
using Etiqueta.Bibliotecas.TaskCorePipe.Modelos;
using Xunit;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Tests
{
    public class SerializadorJsonTests
    {
        [Fact]
        public void Serializar_E_Deserializar_Deve_Retornar_Objeto_Original()
        {
            // Arrange
            var serializador = new SerializadorJson();
            var mensagemOriginal = new MensagemPipe
            {
                IdOrigem = "TesteOrigem",
                IdDestino = "TesteDestino",
                Payload = new ComandoPipe
                {
                    Comando = Enums.TipoComando.PING,
                    Parametros = "TestePayload"
                }
            };

            // Act
            var json = serializador.Serializar(mensagemOriginal);
            var mensagemDeserializada = serializador.Deserializar<MensagemPipe>(json);

            // Assert
            Assert.NotNull(mensagemDeserializada);
            Assert.Equal(mensagemOriginal.IdMensagem, mensagemDeserializada.IdMensagem);
            Assert.Equal(mensagemOriginal.IdOrigem, mensagemDeserializada.IdOrigem);

            var comandoOriginal = mensagemOriginal.Payload as ComandoPipe;
            var comandoDeserializado = mensagemDeserializada.Payload as ComandoPipe;

            Assert.NotNull(comandoDeserializado);
            Assert.Equal(comandoOriginal.Comando, comandoDeserializado.Comando);
            Assert.Equal(comandoOriginal.Parametros.ToString(), comandoDeserializado.Parametros.ToString());
        }
    }
}
