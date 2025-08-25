using System;
using System.Text;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringBuilderParaStringTests
    {
        [Fact]
        public void Execute_ComStringBuilderComConteudo_DeveRetornarString()
        {
            // Arrange
            var sb = new StringBuilder("teste");
            var expected = "teste";

            // Act
            var result = StringBuilderParaString.Execute(sb);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringBuilderVazio_DeveRetornarStringVazia()
        {
            // Arrange
            var sb = new StringBuilder();

            // Act
            var result = StringBuilderParaString.Execute(sb);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComStringBuilderNulo_DeveLancarExcecao()
        {
            // Arrange
            StringBuilder sb = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => StringBuilderParaString.Execute(sb));
        }
    }
}
