using Xunit;
using Etiquetas.Bibliotecas.Comum.Numericos;

namespace Etiquetas.Bibliotecas.Comum.Tests.Numericos
{
    public class StringParaInt64Tests
    {
        [Fact]
        public void Execute_ComStringValida_RetornaInt64()
        {
            // Arrange
            var valor = "1234567890123";
            long expected = 1234567890123;

            // Act
            var result = StringParaInt64.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringInvalida_RetornaZero()
        {
            // Arrange
            var valor = "abc";

            // Act
            var result = StringParaInt64.Execute(valor);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Execute_ComStringNula_RetornaZero()
        {
            // Arrange
            string valor = null;

            // Act
            var result = StringParaInt64.Execute(valor);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Execute_ComValorForaDoRange_RetornaZero()
        {
            // Arrange
            var valor = "9223372036854775808"; // long.MaxValue + 1

            // Act
            var result = StringParaInt64.Execute(valor);

            // Assert
            Assert.Equal(0, result);
        }
    }
}
