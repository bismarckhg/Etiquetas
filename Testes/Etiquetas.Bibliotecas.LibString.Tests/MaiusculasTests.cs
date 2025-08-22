using System;
using Xunit;
using Etiquetas.Bibliotecas.LibString;

namespace Etiquetas.Bibliotecas.LibString.Tests
{
    public class MaiusculasTests
    {
        [Theory]
        [InlineData("aBcDeFg", "ABCDEFG")]
        [InlineData("hello world", "HELLO WORLD")]
        [InlineData("ALREADY UPPER", "ALREADY UPPER")]
        [InlineData("123!@#", "123!@#")]
        [InlineData("", "")]
        public void Execute_ShouldConvertToUppercase(string input, string expected)
        {
            // Act
            string result = Maiusculas.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ShouldThrowException_WhenInputIsNull()
        {
            // Arrange
            string input = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => Maiusculas.Execute(input));
        }
    }
}
