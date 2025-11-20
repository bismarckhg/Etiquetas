using Xunit;
using Etiquetas.Bibliotecas.Comum.Numericos;

namespace Etiquetas.Bibliotecas.Comum.Tests.Numericos
{
    public class StringParaIntNuloTests
    {
        [Fact]
        public void Execute_ComStringValida_RetornaInt()
        {
            // Arrange
            var valor = "123";
            int? expected = 123;

            // Act
            var result = StringParaIntNulo.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringInvalida_RetornaNulo()
        {
            // Arrange
            var valor = "abc";

            // Act
            var result = StringParaIntNulo.Execute(valor);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComStringNula_RetornaNulo()
        {
            // Arrange
            string valor = null;

            // Act
            var result = StringParaIntNulo.Execute(valor);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComValorForaDoRange_RetornaNulo()
        {
            // Arrange
            var valor = "2147483648"; // int.MaxValue + 1

            // Act
            var result = StringParaIntNulo.Execute(valor);

            // Assert
            Assert.Null(result);
        }
    }
}
