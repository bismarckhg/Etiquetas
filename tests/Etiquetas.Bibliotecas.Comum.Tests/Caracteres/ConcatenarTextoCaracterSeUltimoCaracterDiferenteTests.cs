using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConcatenarTextoCaracterSeUltimoCaracterDiferenteTests
    {
        [Fact]
        public void Execute_QuandoUltimoCharDiferente_Concatena()
        {
            // Arrange
            var texto = "abc";
            var caractere = 'd';
            var expected = "abcd";

            // Act
            var result = ConcatenarTextoCaracterSeUltimoCaracterDiferente.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoUltimoCharIgual_NaoConcatena()
        {
            // Arrange
            var texto = "abc";
            var caractere = 'c';
            var expected = "abc";

            // Act
            var result = ConcatenarTextoCaracterSeUltimoCaracterDiferente.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoNulo_RetornaCharComoString()
        {
            // Arrange
            string texto = null;
            var caractere = 'a';
            var expected = "a";

            // Act
            var result = ConcatenarTextoCaracterSeUltimoCaracterDiferente.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoVazio_RetornaCharComoString()
        {
            // Arrange
            var texto = "";
            var caractere = 'a';
            var expected = "a";

            // Act
            var result = ConcatenarTextoCaracterSeUltimoCaracterDiferente.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComCharDeEspaco_ConcatenaSeDiferente()
        {
            // Arrange
            var texto = "abc";
            var caractere = ' ';
            var expected = "abc ";

            // Act
            var result = ConcatenarTextoCaracterSeUltimoCaracterDiferente.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComCharDeEspaco_NaoConcatenaSeIgual()
        {
            // Arrange
            var texto = "abc ";
            var caractere = ' ';
            var expected = "abc ";

            // Act
            var result = ConcatenarTextoCaracterSeUltimoCaracterDiferente.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
