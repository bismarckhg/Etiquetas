using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConcatenarTextoCaracterTests
    {
        [Fact]
        public void Execute_ComStringEChar_RetornaConcatenacao()
        {
            // Arrange
            var texto = "ab";
            var caractere = 'c';
            var expected = "abc";

            // Act
            var result = ConcatenarTextoCaracter.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringNula_RetornaCharComoString()
        {
            // Arrange
            string texto = null;
            var caractere = 'a';
            var expected = "a";

            // Act
            var result = ConcatenarTextoCaracter.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringVazia_RetornaCharComoString()
        {
            // Arrange
            var texto = "";
            var caractere = 'a';
            var expected = "a";

            // Act
            var result = ConcatenarTextoCaracter.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComCharDeEspaco_ConcatenaCorretamente()
        {
            // Arrange
            var texto = "abc";
            var caractere = ' ';
            var expected = "abc ";

            // Act
            var result = ConcatenarTextoCaracter.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
