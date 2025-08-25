using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class IntParaStringTests
    {
        [Fact]
        public void Execute_ComInteiroPositivo_RetornaString()
        {
            // Arrange
            var valor = 123;
            var expected = "123";

            // Act
            var result = IntParaString.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComInteiroNegativo_RetornaString()
        {
            // Arrange
            var valor = -123;
            var expected = "-123";

            // Act
            var result = IntParaString.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComZero_RetornaStringZero()
        {
            // Arrange
            var valor = 0;
            var expected = "0";

            // Act
            var result = IntParaString.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
