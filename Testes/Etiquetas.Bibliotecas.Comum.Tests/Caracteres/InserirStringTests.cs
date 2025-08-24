using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class InserirStringTests
    {
        [Fact]
        public void Execute_NoMeio_InsereCorretamente()
        {
            // Arrange
            var texto = "ad";
            var stringAInserir = "bc";
            var posicao = 1;
            var expected = "abcd";

            // Act
            var result = InserirString.Execute(texto, stringAInserir, posicao);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_NoInicio_InsereCorretamente()
        {
            // Arrange
            var texto = "cd";
            var stringAInserir = "ab";
            var posicao = 0;
            var expected = "abcd";

            // Act
            var result = InserirString.Execute(texto, stringAInserir, posicao);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_NoFim_InsereCorretamente()
        {
            // Arrange
            var texto = "ab";
            var stringAInserir = "cd";
            var posicao = 2;
            var expected = "abcd";

            // Act
            var result = InserirString.Execute(texto, stringAInserir, posicao);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComPosicaoInvalida_RetornaTextoOriginal()
        {
            // Arrange
            var texto = "abc";
            var stringAInserir = "x";
            var posicao = 4;

            // Act
            var result = InserirString.Execute(texto, stringAInserir, posicao);

            // Assert
            Assert.Equal(texto, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;
            var stringAInserir = "a";
            var posicao = 0;

            // Act
            var result = InserirString.Execute(texto, stringAInserir, posicao);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComStringAInserirNula_RetornaTextoOriginal()
        {
            // Arrange
            var texto = "abc";
            string stringAInserir = null;
            var posicao = 1;

            // Act
            var result = InserirString.Execute(texto, stringAInserir, posicao);

            // Assert
            Assert.Equal(texto, result);
        }
    }
}
