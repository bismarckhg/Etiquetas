using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class IntNuloParaStringTests
    {
        [Fact]
        public void Execute_ComValorNulo_RetornaStringVazia()
        {
            // Arrange
            int? valor = null;
            var expected = "";

            // Act
            var result = IntNuloParaString.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComValorNaoNulo_RetornaString()
        {
            // Arrange
            int? valor = 123;
            var expected = "123";

            // Act
            var result = IntNuloParaString.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
