using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class StringArrayRemoveElementosVaziosTests
    {
        [Fact]
        public void Execute_ComArrayMisto_DeveRetornarArrayFiltrado()
        {
            // Arrange
            var array = new[] { "a", null, "b", "", " ", "c" };
            var expected = new[] { "a", "b", "c" };

            // Act
            var result = StringArrayRemoveElementosVazios.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArraySoComElementosInvalidos_DeveRetornarArrayVazio()
        {
            // Arrange
            var array = new[] { null, "", "   " };

            // Act
            var result = StringArrayRemoveElementosVazios.Execute(array);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Execute_ComArraySemElementosInvalidos_DeveRetornarArrayOriginal()
        {
            // Arrange
            var array = new[] { "a", "b", "c" };
            var expected = new[] { "a", "b", "c" };

            // Act
            var result = StringArrayRemoveElementosVazios.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayVazio_DeveRetornarArrayVazio()
        {
            // Arrange
            var array = new string[0];

            // Act
            var result = StringArrayRemoveElementosVazios.Execute(array);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Execute_ComArrayNulo_DeveRetornarArrayVazio()
        {
            // Arrange
            string[] array = null;

            // Act
            var result = StringArrayRemoveElementosVazios.Execute(array);

            // Assert
            Assert.Empty(result);
        }
    }
}
