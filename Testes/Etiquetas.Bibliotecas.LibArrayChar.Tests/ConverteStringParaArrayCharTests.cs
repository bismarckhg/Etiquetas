using Xunit;
using Etiquetas.Bibliotecas.LibArrayChar;

namespace Etiquetas.Bibliotecas.LibArrayChar.Tests
{
    public class ConverteStringParaArrayCharTests
    {
        [Fact]
        public void Execute_ShouldConvertStringToArrayOfChars()
        {
            // Arrange
            string input = "abc";
            char[] expected = new char[] { 'a', 'b', 'c' };

            // Act
            char[] result = ConverteStringParaArrayChar.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_WithEmptyString_ShouldReturnEmptyArray()
        {
            // Arrange
            string input = "";
            char[] expected = new char[] { };

            // Act
            char[] result = ConverteStringParaArrayChar.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_WithNullString_ShouldReturnNull()
        {
            // Arrange
            string input = null;

            // Act
            char[] result = ConverteStringParaArrayChar.Execute(input);

            // Assert
            Assert.Null(result);
        }
    }
}
