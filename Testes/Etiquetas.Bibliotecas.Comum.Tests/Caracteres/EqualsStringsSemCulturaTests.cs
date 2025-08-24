using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class EqualsStringsSemCulturaTests
    {
        [Fact]
        public void Execute_ComStringsIguais_RetornaTrue()
        {
            // Arrange
            var texto1 = "abc";
            var texto2 = "abc";

            // Act
            var result = EqualsStringsSemCultura.Execute(texto1, texto2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComStringsDiferentes_RetornaFalse()
        {
            // Arrange
            var texto1 = "abc";
            var texto2 = "def";

            // Act
            var result = EqualsStringsSemCultura.Execute(texto1, texto2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComCaseDiferente_RetornaFalse()
        {
            // Arrange
            var texto1 = "abc";
            var texto2 = "Abc";

            // Act
            var result = EqualsStringsSemCultura.Execute(texto1, texto2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComUmNulo_RetornaFalse()
        {
            // Arrange
            string texto1 = null;
            var texto2 = "abc";

            // Act
            var result = EqualsStringsSemCultura.Execute(texto1, texto2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComAmbosNulos_RetornaTrue()
        {
            // Arrange
            string texto1 = null;
            string texto2 = null;

            // Act
            var result = EqualsStringsSemCultura.Execute(texto1, texto2);

            // Assert
            Assert.True(result);
        }
    }
}
