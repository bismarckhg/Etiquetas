using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ExtrairTextoTests
    {
        [Fact]
        public void Execute_ComParametrosValidos_ExtraiCorretamente()
        {
            // Arrange
            var texto = "abcdef";
            var posicao = 1;
            var numeroCaracteres = 3;
            var expected = "bcd";

            // Act
            var result = ExtrairTexto.Execute(texto, posicao, numeroCaracteres);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComNumeroDeCaracteresExcedente_ExtraiAteOFinal()
        {
            // Arrange
            var texto = "abcdef";
            var posicao = 3;
            var numeroCaracteres = 10;
            var expected = "def";

            // Act
            var result = ExtrairTexto.Execute(texto, posicao, numeroCaracteres);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1, 3)]
        [InlineData(6, 1)]
        public void Execute_ComPosicaoInvalida_RetornaStringVazia(int posicao, int numeroCaracteres)
        {
            // Arrange
            var texto = "abcdef";

            // Act
            var result = ExtrairTexto.Execute(texto, posicao, numeroCaracteres);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public void Execute_ComNumeroDeCaracteresInvalido_RetornaStringVazia(int posicao, int numeroCaracteres)
        {
            // Arrange
            var texto = "abcdef";

            // Act
            var result = ExtrairTexto.Execute(texto, posicao, numeroCaracteres);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaStringVazia()
        {
            // Arrange
            string texto = null;

            // Act
            var result = ExtrairTexto.Execute(texto, 1, 3);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
