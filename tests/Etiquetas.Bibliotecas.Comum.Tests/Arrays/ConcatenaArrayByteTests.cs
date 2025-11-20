using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ConcatenaArrayByteTests
    {
        [Fact]
        public void Execute_ComDoisArrays_RetornaArrayConcatenado()
        {
            // Arrange
            var array1 = new byte[] { 1, 2 };
            var array2 = new byte[] { 3, 4 };
            var expected = new byte[] { 1, 2, 3, 4 };

            // Act
            var result = ConcatenaArrayByte.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComPrimeiroArrayNulo_RetornaSegundoArray()
        {
            // Arrange
            byte[] array1 = null;
            var array2 = new byte[] { 3, 4 };
            var expected = new byte[] { 3, 4 };

            // Act
            var result = ConcatenaArrayByte.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComSegundoArrayNulo_RetornaPrimeiroArray()
        {
            // Arrange
            var array1 = new byte[] { 1, 2 };
            byte[] array2 = null;
            var expected = new byte[] { 1, 2 };

            // Act
            var result = ConcatenaArrayByte.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComAmbosArraysNulos_RetornaArrayVazio()
        {
            // Arrange
            byte[] array1 = null;
            byte[] array2 = null;
            var expected = new byte[] { };

            // Act
            var result = ConcatenaArrayByte.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArraysVazios_RetornaArrayVazio()
        {
            // Arrange
            var array1 = new byte[] { };
            var array2 = new byte[] { };
            var expected = new byte[] { };

            // Act
            var result = ConcatenaArrayByte.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
