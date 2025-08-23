using Xunit;
using Etiqueta.Bibliotecas.LibInt;
using Etiquetas.Bibliotecas.Comum.Numericos;

namespace Etiqueta.Bibliotecas.LibInt.Tests
{
    public class StringParaIntTests
    {
        [Theory]
        [InlineData("123", 123)]
        [InlineData("-45", -45)]
        [InlineData("0", 0)]
        public void Execute_WithValidIntegerString_ShouldReturnInteger(string input, int expected)
        {
            // Act
            var result = StringParaInt.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("12.34")]
        [InlineData("")]
        [InlineData(null)]
        public void Execute_WithInvalidString_ShouldReturnDefault(string input)
        {
            // Arrange
            var expected = default(int); // 0

            // Act
            var result = StringParaInt.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
