using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class StringArrayPossuiElementoVazioOuComEspacoTests
    {
        [Fact]
        public void Execute_ComPeloMenosUmElementoVazio_DeveRetornarTrue()
        {
            // Arrange
            string[] array = { "a", "b", "" };

            // Act
            bool result = StringArrayPossuiElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComPeloMenosUmElementoNulo_DeveRetornarTrue()
        {
            // Arrange
            string[] array = { "a", "b", null };

            // Act
            bool result = StringArrayPossuiElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComPeloMenosUmElementoComEspaco_DeveRetornarTrue()
        {
            // Arrange
            string[] array = { "a", "b", " " };

            // Act
            bool result = StringArrayPossuiElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_SemElementosVazios_DeveRetornarFalse()
        {
            // Arrange
            string[] array = { "a", "b", "c" };

            // Act
            bool result = StringArrayPossuiElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComArrayVazio_DeveRetornarFalse()
        {
            // Arrange
            string[] array = { };

            // Act
            bool result = StringArrayPossuiElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComArrayNulo_DeveRetornarFalse()
        {
            // Arrange
            string[] array = null;

            // Act
            bool result = StringArrayPossuiElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.False(result);
        }
    }
}
