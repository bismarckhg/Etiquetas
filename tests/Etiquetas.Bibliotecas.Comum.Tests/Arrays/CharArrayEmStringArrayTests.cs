using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class CharArrayEmStringArrayTests
    {
        [Fact]
        public void Execute_ComArrayDeChars_RetornaArrayDeStrings()
        {
            // Arrange
            var arrayChar = new[] { 'a', 'b', 'c' };
            var expected = new[] { "a", "b", "c" };

            // Act
            var result = CharArrayEmStringArray.Execute(arrayChar);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_RetornaArrayVazio()
        {
            // Arrange
            char[] arrayChar = null;
            var expected = new string[] { };

            // Act
            var result = CharArrayEmStringArray.Execute(arrayChar);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaArrayVazio()
        {
            // Arrange
            var arrayChar = new char[] { };
            var expected = new string[] { };

            // Act
            var result = CharArrayEmStringArray.Execute(arrayChar);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComCaracteresEspeciais_ConverteCorretamente()
        {
            // Arrange
            var arrayChar = new[] { ' ', '\t', '\0' };
            var expected = new[] { " ", "\t", "\0" };

            // Act
            var result = CharArrayEmStringArray.Execute(arrayChar);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
