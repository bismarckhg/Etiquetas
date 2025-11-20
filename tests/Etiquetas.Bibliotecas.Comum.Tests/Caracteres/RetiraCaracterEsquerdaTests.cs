using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class RetiraCaracterEsquerdaTests
    {
        [Theory]
        [InlineData("abcdef", 2, "cdef")]
        [InlineData("teste", 1, "este")]
        [InlineData("12345", 5, "")]
        [InlineData("12345", 6, "")]
        public void Execute_ComInputsValidos_DeveRetirarCaracteresDaEsquerda(string texto, int quantidade, string esperado)
        {
            // Act
            var resultado = RetiraCaracterEsquerda.Execute(texto, quantidade);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData("abcdef", 0)]
        [InlineData("abcdef", -1)]
        [InlineData("", 2)]
        [InlineData(null, 2)]
        public void Execute_ComInputsDeBorda_DeveRetornarTextoOriginal(string texto, int quantidade)
        {
            // Act
            var resultado = RetiraCaracterEsquerda.Execute(texto, quantidade);

            // Assert
            Assert.Equal(texto, resultado);
        }
    }
}
