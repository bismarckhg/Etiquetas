using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ConcaternarCharArraysEmCharArrayTests
    {
        [Fact]
        public void ConcatenarCharArrays_ComDoisArraysValidos_DeveRetornarArrayConcatenado()
        {
            // Arrange
            char[] array1 = { 'a', 'b' };
            char[] array2 = { 'c', 'd' };
            char[] expected = { 'a', 'b', 'c', 'd' };

            // Act
            char[] result = ConcaternarCharArraysEmCharArray.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConcatenarCharArrays_ComPrimeiroArrayNulo_DeveRetornarSegundoArray()
        {
            // Arrange
            char[] array1 = null;
            char[] array2 = { 'c', 'd' };
            char[] expected = { 'c', 'd' };

            // Act
            char[] result = ConcaternarCharArraysEmCharArray.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConcatenarCharArrays_ComSegundoArrayNulo_DeveRetornarPrimeiroArray()
        {
            // Arrange
            char[] array1 = { 'a', 'b' };
            char[] array2 = null;
            char[] expected = { 'a', 'b' };

            // Act
            char[] result = ConcaternarCharArraysEmCharArray.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConcatenarCharArrays_ComAmbosArraysNulos_DeveRetornarArrayVazio()
        {
            // Arrange
            char[] array1 = null;
            char[] array2 = null;
            char[] expected = { };

            // Act
            char[] result = ConcaternarCharArraysEmCharArray.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConcatenarCharArrays_ComPrimeiroArrayVazio_DeveRetornarSegundoArray()
        {
            // Arrange
            char[] array1 = { };
            char[] array2 = { 'c', 'd' };
            char[] expected = { 'c', 'd' };

            // Act
            char[] result = ConcaternarCharArraysEmCharArray.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConcatenarCharArrays_ComSegundoArrayVazio_DeveRetornarPrimeiroArray()
        {
            // Arrange
            char[] array1 = { 'a', 'b' };
            char[] array2 = { };
            char[] expected = { 'a', 'b' };

            // Act
            char[] result = ConcaternarCharArraysEmCharArray.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConcatenarCharArrays_ComAmbosArraysVazios_DeveRetornarArrayVazio()
        {
            // Arrange
            char[] array1 = { };
            char[] array2 = { };
            char[] expected = { };

            // Act
            char[] result = ConcaternarCharArraysEmCharArray.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
