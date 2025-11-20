using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConcatenarTextoCaracterTextoTests
    {
        [Fact]
        public void Execute_ComTextosEChar_RetornaConcatenacao()
        {
            // Arrange
            var texto1 = "abc";
            var caractere = '-';
            var texto2 = "def";
            var expected = "abc-def";

            // Act
            var result = ConcatenarTextoCaracterTexto.Execute(texto1, caractere, texto2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComPrimeiroTextoNulo_ConcatenaCorretamente()
        {
            // Arrange
            string texto1 = null;
            var caractere = '-';
            var texto2 = "def";
            var expected = "-def";

            // Act
            var result = ConcatenarTextoCaracterTexto.Execute(texto1, caractere, texto2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComSegundoTextoNulo_ConcatenaCorretamente()
        {
            // Arrange
            var texto1 = "abc";
            var caractere = '-';
            string texto2 = null;
            var expected = "abc-";

            // Act
            var result = ConcatenarTextoCaracterTexto.Execute(texto1, caractere, texto2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComAmbosTextosNulos_RetornaCharComoString()
        {
            // Arrange
            string texto1 = null;
            var caractere = '-';
            string texto2 = null;
            var expected = "-";

            // Act
            var result = ConcatenarTextoCaracterTexto.Execute(texto1, caractere, texto2);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
