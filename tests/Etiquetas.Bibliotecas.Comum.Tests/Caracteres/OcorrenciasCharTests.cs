using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class OcorrenciasCharTests
    {
        [Fact]
        public void Execute_QuandoCaractereExiste_RetornaContagemCorreta()
        {
            // Arrange
            var texto = "banana";
            var caractere = 'a';
            var expected = 3;

            // Act
            var result = OcorrenciasChar.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoCaractereNaoExiste_RetornaZero()
        {
            // Arrange
            var texto = "banana";
            var caractere = 'x';
            var expected = 0;

            // Act
            var result = OcorrenciasChar.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoNulo_RetornaZero()
        {
            // Arrange
            string texto = null;
            var caractere = 'a';
            var expected = 0;

            // Act
            var result = OcorrenciasChar.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoVazio_RetornaZero()
        {
            // Arrange
            var texto = "";
            var caractere = 'a';
            var expected = 0;

            // Act
            var result = OcorrenciasChar.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
