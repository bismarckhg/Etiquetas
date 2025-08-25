using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class EhCharNuloVazioComEspacosBrancoTests
    {
        [Theory]
        [InlineData(' ')]
        [InlineData('\t')]
        [InlineData('\n')]
        [InlineData('\r')]
        public void Execute_ComCharDeEspaco_RetornaTrue(char c)
        {
            // Arrange & Act
            var result = EhCharNuloVazioComEspacosBranco.Execute(c);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData('a')]
        [InlineData('1')]
        [InlineData('.')]
        public void Execute_ComCharNormal_RetornaFalse(char c)
        {
            // Arrange & Act
            var result = EhCharNuloVazioComEspacosBranco.Execute(c);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComCharNulo_RetornaFalse()
        {
            // Arrange
            var c = '\0';

            // Act
            var result = EhCharNuloVazioComEspacosBranco.Execute(c);

            // Assert
            Assert.False(result);
        }
    }
}
