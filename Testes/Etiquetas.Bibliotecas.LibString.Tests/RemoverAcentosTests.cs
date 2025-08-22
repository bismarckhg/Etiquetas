using Xunit;
using Etiquetas.Bibliotecas.LibString;

namespace Etiquetas.Bibliotecas.LibString.Tests
{
    public class RemoverAcentosTests
    {
        static RemoverAcentosTests()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        [Theory]
        [InlineData("ação, coração, pássaro", "acao, coracao, passaro")]
        [InlineData("Olá, mundo!", "Ola, mundo!")]
        [InlineData("ÀÉÍÓÚàéíóúâêîôû", "AEIOUaeiouaeiou")]
        [InlineData("Çç", "Cc")]
        public void Execute_ShouldRemoveAccents(string input, string expected)
        {
            // Act
            string result = RemoverAcentos.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ShouldReturnSameString_WhenNoAccents()
        {
            // Arrange
            string input = "uma string normal sem acentos";

            // Act
            string result = RemoverAcentos.Execute(input);

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void Execute_ShouldReturnEmpty_WhenInputIsEmpty()
        {
            // Arrange
            string input = "";

            // Act
            string result = RemoverAcentos.Execute(input);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void Execute_ShouldReturnNull_WhenInputIsNull()
        {
            // Arrange
            string input = null;

            // Act
            string result = RemoverAcentos.Execute(input);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ShouldReturnWhitespace_WhenInputIsWhitespace()
        {
            // Arrange
            string input = "   ";

            // Act
            string result = RemoverAcentos.Execute(input);

            // Assert
            Assert.Equal("   ", result);
        }
    }
}
