using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringEmCharTests
    {
        [Fact]
        public void Execute_ComStringValida_DeveRetornarPrimeiroCaractere()
        {
            // Arrange
            var texto = "abc";
            var expected = 'a';

            // Act
            var result = StringEmChar.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Execute_ComStringNulaOuVaziaOuEspaco_DeveRetornarCharNulo(string texto)
        {
            // Arrange
            var expected = '\0'; // default(char)

            // Act
            var result = StringEmChar.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringDeUmCaractere_DeveRetornarEsseCaractere()
        {
            // Arrange
            var texto = "Z";
            var expected = 'Z';

            // Act
            var result = StringEmChar.Execute(texto);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
