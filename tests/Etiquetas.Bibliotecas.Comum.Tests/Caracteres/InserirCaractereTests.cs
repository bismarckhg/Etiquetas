using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class InserirCaractereTests
    {
        [Fact]
        public void Execute_NoMeio_InsereCorretamente()
        {
            // Arrange
            var texto = "ac";
            var caractere = 'b';
            var posicao = 1;
            var expected = "abc";

            // Act
            var result = InserirCaractere.Execute(texto, caractere, posicao);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_NoInicio_InsereCorretamente()
        {
            // Arrange
            var texto = "bc";
            var caractere = 'a';
            var posicao = 0;
            var expected = "abc";

            // Act
            var result = InserirCaractere.Execute(texto, caractere, posicao);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_NoFim_InsereCorretamente()
        {
            // Arrange
            var texto = "ab";
            var caractere = 'c';
            var posicao = 2;
            var expected = "abc";

            // Act
            var result = InserirCaractere.Execute(texto, caractere, posicao);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        public void Execute_ComPosicaoInvalida_RetornaTextoOriginal(int posicao)
        {
            // Arrange
            var texto = "abc";
            var caractere = 'x';

            // Act
            var result = InserirCaractere.Execute(texto, caractere, posicao);

            // Assert
            Assert.Equal(texto, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;
            var caractere = 'a';
            var posicao = 0;

            // Act
            var result = InserirCaractere.Execute(texto, caractere, posicao);

            // Assert
            Assert.Null(result);
        }
    }
}
