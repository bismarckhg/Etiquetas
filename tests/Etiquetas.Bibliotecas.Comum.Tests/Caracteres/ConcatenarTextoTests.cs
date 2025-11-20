using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConcatenarTextoTests
    {
        [Fact]
        public void Execute_ComVariosTextos_RetornaStringConcatenada()
        {
            // Arrange
            var expected = "abcdef";

            // Act
            var result = ConcatenarTexto.Execute("abc", "def");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_SemTextos_RetornaStringVazia()
        {
            // Arrange
            var expected = "";

            // Act
            var result = ConcatenarTexto.Execute();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComUmTexto_RetornaOProprioTexto()
        {
            // Arrange
            var expected = "abc";

            // Act
            var result = ConcatenarTexto.Execute("abc");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_TrataComoVazio()
        {
            // Arrange
            var expected = "abcdef";

            // Act
            var result = ConcatenarTexto.Execute("abc", null, "def");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTextoVazio_ConcatenaNormalmente()
        {
            // Arrange
            var expected = "abcdef";

            // Act
            var result = ConcatenarTexto.Execute("abc", "", "def");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_RetornaStringVazia()
        {
            // Arrange
            string[] textos = null;
            var expected = "";

            // Act
            var result = ConcatenarTexto.Execute(textos);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
