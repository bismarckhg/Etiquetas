using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class EntreLetrasTests
    {
        [Fact]
        public void Execute_ComTextoEEntreLetras_InsereCorretamente()
        {
            // Arrange
            var texto = "abc";
            var entreLetras = "-";
            var expected = "a-b-c";

            // Act
            var result = EntreLetras.Execute(texto, entreLetras);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;
            var entreLetras = "-";

            // Act
            var result = EntreLetras.Execute(texto, entreLetras);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComTextoVazio_RetornaVazio()
        {
            // Arrange
            var texto = "";
            var entreLetras = "-";
            var expected = "";

            // Act
            var result = EntreLetras.Execute(texto, entreLetras);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComEntreLetrasNulo_RetornaTextoOriginal()
        {
            // Arrange
            var texto = "abc";
            string entreLetras = null;
            var expected = "abc";

            // Act
            var result = EntreLetras.Execute(texto, entreLetras);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTextoDeUmCaractere_NaoInsereNada()
        {
            // Arrange
            var texto = "a";
            var entreLetras = "-";
            var expected = "a";

            // Act
            var result = EntreLetras.Execute(texto, entreLetras);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
