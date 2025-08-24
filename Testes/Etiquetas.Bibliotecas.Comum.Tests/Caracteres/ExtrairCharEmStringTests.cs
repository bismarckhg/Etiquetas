using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ExtrairCharEmStringTests
    {
        [Fact]
        public void Execute_ComPosicaoValida_RetornaChar()
        {
            // Arrange
            var texto = "abc";
            var posicao = 1;
            var expected = 'b';

            // Act
            var result = ExtrairCharEmString.Execute(texto, posicao);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public void Execute_ComPosicaoInvalida_RetornaEspaco(int posicao)
        {
            // Arrange
            var texto = "abc";

            // Act
            var result = ExtrairCharEmString.Execute(texto, posicao);

            // Assert
            Assert.Equal(' ', result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaEspaco()
        {
            // Arrange
            string texto = null;
            var posicao = 0;

            // Act
            var result = ExtrairCharEmString.Execute(texto, posicao);

            // Assert
            Assert.Equal(' ', result);
        }

        [Fact]
        public void Execute_ComTextoVazio_RetornaEspaco()
        {
            // Arrange
            var texto = "";
            var posicao = 0;

            // Act
            var result = ExtrairCharEmString.Execute(texto, posicao);

            // Assert
            Assert.Equal(' ', result);
        }
    }
}
