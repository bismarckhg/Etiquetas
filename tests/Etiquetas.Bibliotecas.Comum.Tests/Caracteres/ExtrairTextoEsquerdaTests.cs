using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ExtrairTextoEsquerdaTests
    {
        [Fact]
        public void Execute_ComParametrosValidos_ExtraiCorretamente()
        {
            // Arrange
            var texto = "abcdef";
            var numeroCaracteres = 3;
            var expected = "abc";

            // Act
            var result = ExtrairTextoEsquerda.Execute(texto, numeroCaracteres);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComNumeroDeCaracteresExcedente_RetornaStringCompleta()
        {
            // Arrange
            var texto = "abc";
            var numeroCaracteres = 5;
            var expected = "abc";

            // Act
            var result = ExtrairTextoEsquerda.Execute(texto, numeroCaracteres);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Execute_ComNumeroDeCaracteresInvalido_RetornaStringVazia(int numeroCaracteres)
        {
            // Arrange
            var texto = "abcdef";

            // Act
            var result = ExtrairTextoEsquerda.Execute(texto, numeroCaracteres);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaStringVazia()
        {
            // Arrange
            string texto = null;

            // Act
            var result = ExtrairTextoEsquerda.Execute(texto, 3);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
