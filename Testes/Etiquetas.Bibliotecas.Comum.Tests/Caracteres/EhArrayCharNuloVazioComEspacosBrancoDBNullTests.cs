using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class EhArrayCharNuloVazioComEspacosBrancoDBNullTests
    {
        [Fact]
        public void Execute_ComArrayNulo_RetornaTrue()
        {
            // Arrange
            char[] array = null;

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaTrue()
        {
            // Arrange
            var array = new char[] { };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayDeEspacos_RetornaTrue()
        {
            // Arrange
            var array = new[] { ' ', '\t', '\n' };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayComLetras_RetornaFalse()
        {
            // Arrange
            var array = new[] { 'a', 'b', 'c' };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBrancoDBNull.Execute(array);

            // Assert
            Assert.False(result);
        }
    }
}
