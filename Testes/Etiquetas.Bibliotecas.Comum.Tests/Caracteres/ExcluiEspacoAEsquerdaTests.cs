using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ExcluiEspacoAEsquerdaTests
    {
        [Fact]
        public void Execute_ComEspacosAEsquerda_RemoveEspacos()
        {
            // Arrange
            var texto = "  abc";
            var expected = "abc";

            // Act
            var result = ExcluiEspacoAEsquerda.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComEspacosADireita_NaoRemoveEspacos()
        {
            // Arrange
            var texto = "abc  ";
            var expected = "abc  ";

            // Act
            var result = ExcluiEspacoAEsquerda.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_SemEspacos_NaoAlteraTexto()
        {
            // Arrange
            var texto = "abc";
            var expected = "abc";

            // Act
            var result = ExcluiEspacoAEsquerda.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;

            // Act
            var result = ExcluiEspacoAEsquerda.Execute(texto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComTextoVazio_RetornaVazio()
        {
            // Arrange
            var texto = "";
            var expected = "";

            // Act
            var result = ExcluiEspacoAEsquerda.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
