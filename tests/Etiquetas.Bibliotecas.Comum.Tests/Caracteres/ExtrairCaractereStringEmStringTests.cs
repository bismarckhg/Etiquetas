using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ExtrairCaractereStringEmStringTests
    {
        [Fact]
        public void Execute_ComPosicaoValida_RetornaCaractereComoString()
        {
            // Arrange
            var texto = "abc";
            var posicao = 1;
            var expected = "b";

            // Act
            var result = ExtrairCaractereStringEmString.Execute(texto, posicao);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(3)]
        public void Execute_ComPosicaoInvalida_RetornaStringVazia(int posicao)
        {
            // Arrange
            var texto = "abc";

            // Act
            var result = ExtrairCaractereStringEmString.Execute(texto, posicao);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaStringVazia()
        {
            // Arrange
            string texto = null;
            var posicao = 0;

            // Act
            var result = ExtrairCaractereStringEmString.Execute(texto, posicao);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComTextoVazio_RetornaStringVazia()
        {
            // Arrange
            var texto = "";
            var posicao = 0;

            // Act
            var result = ExtrairCaractereStringEmString.Execute(texto, posicao);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
