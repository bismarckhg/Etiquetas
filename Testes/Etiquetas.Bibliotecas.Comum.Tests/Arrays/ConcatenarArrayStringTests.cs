using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ConcatenarArrayStringTests
    {
        // Testes para a sobrecarga Execute(string[], string[])

        [Fact]
        public void Execute_DoisArrays_RetornaConcatenacao()
        {
            // Arrange
            var array1 = new[] { "a", "b" };
            var array2 = new[] { "c", "d" };
            var expected = new[] { "a", "b", "c", "d" };

            // Act
            var result = ConcatenarArrayString.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_PrimeiroArrayNulo_RetornaSegundoArray()
        {
            // Arrange
            string[] array1 = null;
            var array2 = new[] { "c", "d" };
            var expected = new[] { "c", "d" };

            // Act
            var result = ConcatenarArrayString.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_SegundoArrayNulo_RetornaPrimeiroArray()
        {
            // Arrange
            var array1 = new[] { "a", "b" };
            string[] array2 = null;
            var expected = new[] { "a", "b" };

            // Act
            var result = ConcatenarArrayString.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_AmbosArraysNulos_RetornaArrayVazio()
        {
            // Arrange
            string[] array1 = null;
            string[] array2 = null;
            var expected = new string[] { };

            // Act
            var result = ConcatenarArrayString.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ArraysComElementosNulos_PreservaNulos()
        {
            // Arrange
            var array1 = new[] { "a", null };
            var array2 = new[] { "c", "" };
            var expected = new[] { "a", null, "c", "" };

            // Act
            var result = ConcatenarArrayString.Execute(array1, array2);

            // Assert
            Assert.Equal(expected, result);
        }

        // Testes para a sobrecarga Execute(params string[][])

        [Fact]
        public void ExecuteParams_VariosArrays_RetornaConcatenacaoSemVazios()
        {
            // Arrange
            var array1 = new[] { "a", "b" };
            var array2 = new[] { "c", null, "d" };
            var array3 = new[] { "", "e", " " };
            var expected = new[] { "a", "b", "c", "d", "e" };

            // Act
            var result = ConcatenarArrayString.Execute(array1, array2, array3);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExecuteParams_ComArrayNuloNaLista_IgnoraArrayNulo()
        {
            // Arrange
            var array1 = new[] { "a", "b" };
            string[] array2 = null;
            var array3 = new[] { "c", "d" };
            var expected = new[] { "a", "b", "c", "d" };

            // Act
            var result = ConcatenarArrayString.Execute(array1, array2, array3);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExecuteParams_ComParametroNulo_RetornaArrayVazio()
        {
            // Arrange & Act
            var result = ConcatenarArrayString.Execute(null);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ExecuteParams_SemParametros_RetornaArrayVazio()
        {
            // Arrange & Act
            var result = ConcatenarArrayString.Execute();

            // Assert
            Assert.Empty(result);
        }
    }
}
