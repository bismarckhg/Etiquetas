using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class EhArrayCharNuloVazioComEspacosBrancoTests
    {
        [Fact]
        public void Execute_ComArrayNulo_RetornaTrue()
        {
            // Arrange
            char[] array = null;

            // Act
            var result = EhArrayCharNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaTrue()
        {
            // Arrange
            var array = new char[] { };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayDeEspacos_RetornaTrue()
        {
            // Arrange
            var array = new[] { ' ', '\t', '\n' };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayComLetras_RetornaFalse()
        {
            // Arrange
            var array = new[] { 'a', 'b', 'c' };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComArrayMisto_RetornaFalse()
        {
            // Arrange
            var array = new[] { ' ', 'a', ' ' };

            // Act
            var result = EhArrayCharNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.False(result);
        }
    }
}
