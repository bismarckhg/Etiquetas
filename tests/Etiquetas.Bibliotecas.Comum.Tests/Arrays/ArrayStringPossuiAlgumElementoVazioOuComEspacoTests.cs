using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ArrayStringPossuiAlgumElementoVazioOuComEspacoTests
    {
        [Fact]
        public void Execute_ComElementoVazio_RetornaTrue()
        {
            // Arrange
            var array = new[] { "a", "", "c" };

            // Act
            var result = ArrayStringPossuiAlgumElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComElementoNulo_RetornaTrue()
        {
            // Arrange
            var array = new[] { "a", null, "c" };

            // Act
            var result = ArrayStringPossuiAlgumElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComElementoDeEspaco_RetornaTrue()
        {
            // Arrange
            var array = new[] { "a", " ", "c" };

            // Act
            var result = ArrayStringPossuiAlgumElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_SemElementosVazios_RetornaFalse()
        {
            // Arrange
            var array = new[] { "a", "b", "c" };

            // Act
            var result = ArrayStringPossuiAlgumElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComArrayNulo_RetornaFalse()
        {
            // Arrange
            string[] array = null;

            // Act
            var result = ArrayStringPossuiAlgumElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaFalse()
        {
            // Arrange
            var array = new string[] { };

            // Act
            var result = ArrayStringPossuiAlgumElementoVazioOuComEspaco.Execute(array);

            // Assert
            Assert.False(result);
        }
    }
}
