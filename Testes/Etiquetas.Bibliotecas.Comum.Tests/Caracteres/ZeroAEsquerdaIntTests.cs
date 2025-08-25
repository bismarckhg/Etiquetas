using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ZeroAEsquerdaIntTests
    {
        [Theory]
        [InlineData(123, 5, "00123")]
        [InlineData(123, 2, "123")] // Não deve truncar
        [InlineData(98, 0, "98")]
        [InlineData(12345, 9, "000012345")]
        public void Execute_ComInputsValidos_DeveRetornarStringFormatada(int numero, int tamanho, string esperado)
        {
            // Act
            var resultado = ZeroAEsquerdaInt.Execute(numero, tamanho);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public void Execute_ComTamanhoMaiorQueNove_DeveLimitarEmNove()
        {
            // Arrange
            var numero = 123;
            var tamanho = 15;
            var expected = "000000123"; // "D9"

            // Act
            var result = ZeroAEsquerdaInt.Execute(numero, tamanho);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTamanhoNegativo_DeveTratarComoZero()
        {
            // Arrange
            var numero = 123;
            var tamanho = -5;
            var expected = "123"; // "D0"

            // Act
            var result = ZeroAEsquerdaInt.Execute(numero, tamanho);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
