using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class EhStringNuloVazioComEspacosBrancoTests
    {
        [Fact]
        public void Execute_ComStringNula_RetornaTrue()
        {
            // Arrange
            string texto = null;

            // Act
            var result = EhStringNuloVazioComEspacosBranco.Execute(texto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComStringVazia_RetornaTrue()
        {
            // Arrange
            var texto = "";

            // Act
            var result = EhStringNuloVazioComEspacosBranco.Execute(texto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComStringDeEspacos_RetornaTrue()
        {
            // Arrange
            var texto = "   \t\n\r";

            // Act
            var result = EhStringNuloVazioComEspacosBranco.Execute(texto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComStringNormal_RetornaFalse()
        {
            // Arrange
            var texto = "abc";

            // Act
            var result = EhStringNuloVazioComEspacosBranco.Execute(texto);

            // Assert
            Assert.False(result);
        }
    }
}
