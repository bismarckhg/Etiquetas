using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class NumeroCaracteresTests
    {
        [Fact]
        public void Execute_ComStringNormal_RetornaTamanhoCorreto()
        {
            // Arrange
            var texto = "abc";
            var expected = 3;

            // Act
            var result = NumeroCaracteres.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringNula_RetornaZero()
        {
            // Arrange
            string texto = null;
            var expected = 0;

            // Act
            var result = NumeroCaracteres.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringVazia_RetornaZero()
        {
            // Arrange
            var texto = "";
            var expected = 0;

            // Act
            var result = NumeroCaracteres.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringDeEspacos_RetornaZero()
        {
            // Arrange
            var texto = "   ";
            var expected = 0;

            // Act
            var result = NumeroCaracteres.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
