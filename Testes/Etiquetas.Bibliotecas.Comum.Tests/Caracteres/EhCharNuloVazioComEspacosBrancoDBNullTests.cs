using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class EhCharNuloVazioComEspacosBrancoDBNullTests
    {
        [Theory]
        [InlineData(' ')]
        [InlineData('\t')]
        [InlineData('\n')]
        public void Execute_Char_ComCharDeEspaco_RetornaTrue(char c)
        {
            // Arrange & Act
            var result = EhCharNuloVazioComEspacosBrancoDBNull.Execute(c);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData('a')]
        [InlineData('1')]
        public void Execute_Char_ComCharNormal_RetornaFalse(char c)
        {
            // Arrange & Act
            var result = EhCharNuloVazioComEspacosBrancoDBNull.Execute(c);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_DBNull_ComDBNullValue_RetornaTrue()
        {
            // Arrange
            var value = DBNull.Value;

            // Act
            var result = EhCharNuloVazioComEspacosBrancoDBNull.Execute(value);

            // Assert
            Assert.True(result);
        }
    }
}
