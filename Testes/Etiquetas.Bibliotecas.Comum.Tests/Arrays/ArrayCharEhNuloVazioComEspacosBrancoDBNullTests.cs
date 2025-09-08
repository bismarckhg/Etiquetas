using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ArrayCharEhNuloVazioComEspacosBrancoDBNullTests
    {
        [Fact]
        public void Execute_ComArrayNulo_RetornaTrue()
        {
            // Arrange
            char[] input = null;

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaTrue()
        {
            // Arrange
            var input = new char[] { };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayDeEspacos_RetornaTrue()
        {
            // Arrange
            var input = new char[] { ' ', ' ', '\t' };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(input);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayComCaracteres_RetornaFalse()
        {
            // Arrange
            var input = new char[] { 'a', 'b', 'c' };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComArrayMisto_RetornaFalse()
        {
            // Arrange
            var input = new char[] { ' ', 'a', ' ' };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(input);

            // Assert
            Assert.False(result);
        }
    }
}
