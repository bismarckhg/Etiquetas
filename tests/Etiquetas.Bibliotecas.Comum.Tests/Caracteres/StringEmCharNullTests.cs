using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringEmCharNullTests
    {
        [Fact]
        public void Execute_ComStringValida_DeveRetornarPrimeiroCaractere()
        {
            // Arrange
            var texto = "abc";
            char? expected = 'a';

            // Act
            var result = StringEmCharNull.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Execute_ComStringNulaOuVaziaOuEspaco_DeveRetornarNull(string texto)
        {
            // Act
            var result = StringEmCharNull.Execute(texto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComStringDeUmCaractere_DeveRetornarEsseCaractere()
        {
            // Arrange
            var texto = "Z";
            char? expected = 'Z';

            // Act
            var result = StringEmCharNull.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
