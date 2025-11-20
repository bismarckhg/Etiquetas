using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ArrayStringComparaSequenciaTests
    {
        [Fact]
        public void Execute_ComArraysIguais_RetornaTrue()
        {
            // Arrange
            var array1 = new string[] { "a", "b" };
            var array2 = new string[] { "a", "b" };

            // Act
            var result = ArrayStringComparaSequencia.Execute(array1, array2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArraysDiferentes_RetornaFalse()
        {
            // Arrange
            var array1 = new string[] { "a", "b" };
            var array2 = new string[] { "b", "a" };

            // Act
            var result = ArrayStringComparaSequencia.Execute(array1, array2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComUmArrayNulo_RetornaFalse()
        {
            // Arrange
            string[] array1 = null;
            var array2 = new string[] { "a", "b" };

            // Act
            var result = ArrayStringComparaSequencia.Execute(array1, array2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComAmbosArraysNulos_RetornaTrue()
        {
            // Arrange
            string[] array1 = null;
            string[] array2 = null;

            // Act
            var result = ArrayStringComparaSequencia.Execute(array1, array2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayVazioEArrayNulo_RetornaTrue()
        {
            // Arrange
            var array1 = new string[] { };
            string[] array2 = null;

            // Act
            var result = ArrayStringComparaSequencia.Execute(array1, array2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayVazioEArrayComEspacos_RetornaTrue()
        {
            // Arrange
            var array1 = new string[] { };
            var array2 = new string[] { " ", "\t" };

            // Act
            var result = ArrayStringComparaSequencia.Execute(array1, array2);

            // Assert
            Assert.True(result);
        }
    }
}
