using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ConcatenarArrayComNovoElementoStringTests
    {
        [Fact]
        public void Execute_ComArrayEElemento_RetornaArrayConcatenado()
        {
            // Arrange
            var array = new[] { "a", "b" };
            var elemento = "c";
            var expected = new[] { "a", "b", "c" };

            // Act
            var result = ConcatenarArrayComNovoElementoString.Execute(array, elemento);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_RetornaArrayComElemento()
        {
            // Arrange
            string[] array = null;
            var elemento = "a";
            var expected = new[] { "a" };

            // Act
            var result = ConcatenarArrayComNovoElementoString.Execute(array, elemento);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaArrayComElemento()
        {
            // Arrange
            var array = new string[] { };
            var elemento = "a";
            var expected = new[] { "a" };

            // Act
            var result = ConcatenarArrayComNovoElementoString.Execute(array, elemento);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComElementoNulo_AdicionaNuloAoArray()
        {
            // Arrange
            var array = new[] { "a", "b" };
            string elemento = null;
            var expected = new[] { "a", "b", null };

            // Act
            var result = ConcatenarArrayComNovoElementoString.Execute(array, elemento);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComElementoVazio_AdicionaStringVaziaAoArray()
        {
            // Arrange
            var array = new[] { "a", "b" };
            var elemento = "";
            var expected = new[] { "a", "b", "" };

            // Act
            var result = ConcatenarArrayComNovoElementoString.Execute(array, elemento);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
