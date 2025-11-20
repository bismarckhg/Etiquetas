using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConverteByteEmStringUTF8Tests
    {
        [Fact]
        public void Execute_ComBytesUTF8_RetornaStringCorreta()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("teste com acentuação");
            var expected = "teste com acentuação";

            // Act
            var result = ConverteByteEmStringUTF8.Execute(bytes);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_RetornaStringVazia()
        {
            // Arrange
            byte[] bytes = null;
            var expected = "";

            // Act
            var result = ConverteByteEmStringUTF8.Execute(bytes);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaStringVazia()
        {
            // Arrange
            var bytes = new byte[] { };
            var expected = "";

            // Act
            var result = ConverteByteEmStringUTF8.Execute(bytes);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComBytesInvalidosUTF8_UsaCaracteresDeFallback()
        {
            // Arrange
            // Sequência de bytes inválida em UTF-8 (um byte de continuação sem um byte de início)
            var bytes = new byte[] { 0x80, 0x81 };
            var expected = "��"; // Caractere de substituição padrão

            // Act
            var result = ConverteByteEmStringUTF8.Execute(bytes);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
