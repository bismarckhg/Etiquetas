using System;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class MinusculasTests
    {
        [Theory]
        [InlineData("aBcDeFg", "abcdefg")]
        [InlineData("HELLO WORLD", "hello world")]
        [InlineData("already lower", "already lower")]
        [InlineData("123!@#", "123!@#")]
        [InlineData("", "")]
        public void Execute_ShouldConvertToLowercase(string input, string expected)
        {
            // Act
            string result = Minusculas.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ShouldThrowException_WhenInputIsNull()
        {
            // Arrange
            string input = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => Minusculas.Execute(input));
        }
    }
}
