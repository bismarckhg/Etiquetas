using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class CompareStringArrayTests
    {
        [Fact]
        public void Execute_ComArraysDeStringsIguais_RetornaTrue()
        {
            // Arrange
            var array1 = new[] { "a", "b", "c" };
            var array2 = new[] { "a", "b", "c" };

            // Act
            var result = CompareStringArray.Execute(array1, array2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArraysDeStringsDiferentes_RetornaFalse()
        {
            // Arrange
            var array1 = new[] { "a", "b", "c" };
            var array2 = new[] { "c", "b", "a" };

            // Act
            var result = CompareStringArray.Execute(array1, array2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComUmArrayNulo_RetornaFalse()
        {
            // Arrange
            var array1 = new[] { "a", "b", "c" };
            object array2 = null;

            // Act
            var result = CompareStringArray.Execute(array1, array2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComAmbosNulos_RetornaFalse()
        {
            // Arrange
            object array1 = null;
            object array2 = null;

            // Act
            var result = CompareStringArray.Execute(array1, array2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComTiposDeArrayDiferentes_RetornaFalse()
        {
            // Arrange
            var array1 = new[] { "a", "b", "c" };
            var array2 = new[] { 1, 2, 3 };

            // Act
            var result = CompareStringArray.Execute(array1, array2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComArraysDeOutroTipo_RetornaFalse()
        {
            // Arrange
            var array1 = new[] { 1, 2, 3 };
            var array2 = new[] { 1, 2, 3 };

            // Act
            var result = CompareStringArray.Execute(array1, array2);

            // Assert
            Assert.False(result);
        }
    }
}
