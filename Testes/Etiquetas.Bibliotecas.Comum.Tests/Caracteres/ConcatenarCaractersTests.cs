using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConcatenarCaractersTests
    {
        [Fact]
        public void Execute_ComVariosCaracteres_RetornaStringConcatenada()
        {
            // Arrange
            var expected = "abc";

            // Act
            var result = ConcatenarCaracters.Execute('a', 'b', 'c');

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_SemCaracteres_RetornaStringVazia()
        {
            // Arrange
            var expected = "";

            // Act
            var result = ConcatenarCaracters.Execute();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComUmCaractere_RetornaStringComUmCaractere()
        {
            // Arrange
            var expected = "a";

            // Act
            var result = ConcatenarCaracters.Execute('a');

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_RetornaStringVazia()
        {
            // Arrange
            char[] caracteres = null;
            var expected = "";

            // Act
            var result = ConcatenarCaracters.Execute(caracteres);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
