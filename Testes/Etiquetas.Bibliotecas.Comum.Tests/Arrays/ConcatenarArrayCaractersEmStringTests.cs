using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ConcatenarArrayCaractersEmStringTests
    {
        [Fact]
        public void Execute_ComArrayDeChars_RetornaStringConcatenada()
        {
            // Arrange
            var array = new[] { 'a', 'b', 'c' };
            var expected = "abc";

            // Act
            var result = ConcatenarArrayCaractersEmString.Execute(array);

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
            var result = ConcatenarArrayCaractersEmString.Execute(array);

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
            var result = ConcatenarArrayCaractersEmString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
