using System;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ExcluirElementosVaziosArrayStringTests
    {
        [Fact]
        public void Execute_ComArrayMisto_DeveRetornarArraySemElementosVazios()
        {
            // Arrange
            string[] array = { "a", null, "b", "", " ", "c" };
            string[] expected = { "a", "b", "c" };

            // Act
            string[] result = ExcluirElementosVaziosArrayString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArraySemElementosVazios_DeveRetornarArrayOriginal()
        {
            // Arrange
            string[] array = { "a", "b", "c" };
            string[] expected = { "a", "b", "c" };

            // Act
            string[] result = ExcluirElementosVaziosArrayString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArraySoComElementosVazios_DeveRetornarArrayVazio()
        {
            // Arrange
            string[] array = { null, "", " " };
            string[] expected = { };

            // Act
            string[] result = ExcluirElementosVaziosArrayString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayVazio_DeveRetornarArrayVazio()
        {
            // Arrange
            string[] array = { };
            string[] expected = { };

            // Act
            string[] result = ExcluirElementosVaziosArrayString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_DeveLancarArgumentNullException()
        {
            // Arrange
            string[] array = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ExcluirElementosVaziosArrayString.Execute(array));
        }
    }
}
