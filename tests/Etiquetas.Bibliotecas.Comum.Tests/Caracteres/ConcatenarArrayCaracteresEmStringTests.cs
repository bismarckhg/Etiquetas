using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConcatenarArrayCaracteresEmStringTests
    {
        [Fact]
        public void Execute_ComArrayDeChars_RetornaString()
        {
            // Arrange
            var array = new[] { 't', 'e', 's', 't', 'e' };
            var expected = "teste";

            // Act
            var result = ConcatenarArrayCaracteresEmString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_RetornaStringVazia()
        {
            // Arrange
            char[] array = null;
            var expected = "";

            // Act
            var result = ConcatenarArrayCaracteresEmString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaStringVazia()
        {
            // Arrange
            var array = new char[] { };
            var expected = "";

            // Act
            var result = ConcatenarArrayCaracteresEmString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
