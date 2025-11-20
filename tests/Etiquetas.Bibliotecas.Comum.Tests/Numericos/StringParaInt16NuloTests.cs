using Xunit;
using Etiquetas.Bibliotecas.Comum.Numericos;

namespace Etiquetas.Bibliotecas.Comum.Tests.Numericos
{
    public class StringParaInt16NuloTests
    {
        [Fact]
        public void Execute_ComStringValida_RetornaInt16()
        {
            // Arrange
            var valor = "123";
            short? expected = 123;

            // Act
            var result = StringParaInt16Nulo.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringInvalida_RetornaNulo()
        {
            // Arrange
            var valor = "abc";

            // Act
            var result = StringParaInt16Nulo.Execute(valor);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComStringNula_RetornaNulo()
        {
            // Arrange
            string valor = null;

            // Act
            var result = StringParaInt16Nulo.Execute(valor);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComValorForaDoRange_RetornaNulo()
        {
            // Arrange
            var valor = "32768"; // Int16.MaxValue + 1

            // Act
            var result = StringParaInt16Nulo.Execute(valor);

            // Assert
            Assert.Null(result);
        }
    }
}
