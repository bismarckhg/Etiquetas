using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ArrayCharPossuiUmCaractereTests
    {
        [Fact]
        public void Execute_QuandoArrayContemCaractere_RetornaTrue()
        {
            // Arrange
            var arrayChar = new[] { 'a', 'b', 'c' };
            var caractere = 'b';

            // Act
            var result = ArrayCharPossuiUmCaractere.Execute(arrayChar, caractere);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_QuandoArrayNaoContemCaractere_RetornaFalse()
        {
            // Arrange
            var arrayChar = new[] { 'a', 'b', 'c' };
            var caractere = 'd';

            // Act
            var result = ArrayCharPossuiUmCaractere.Execute(arrayChar, caractere);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoArrayEhNulo_RetornaFalse()
        {
            // Arrange
            char[] arrayChar = null;
            var caractere = 'a';

            // Act
            var result = ArrayCharPossuiUmCaractere.Execute(arrayChar, caractere);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoArrayEstaVazio_RetornaFalse()
        {
            // Arrange
            var arrayChar = new char[] { };
            var caractere = 'a';

            // Act
            var result = ArrayCharPossuiUmCaractere.Execute(arrayChar, caractere);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoCaractereEhEspaco_RetornaTrue()
        {
            // Arrange
            var arrayChar = new[] { 'a', ' ', 'c' };
            var caractere = ' ';

            // Act
            var result = ArrayCharPossuiUmCaractere.Execute(arrayChar, caractere);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_QuandoCaractereEhNuloChar_RetornaTrue()
        {
            // Arrange
            var arrayChar = new[] { 'a', '\0', 'c' };
            var caractere = '\0';

            // Act
            var result = ArrayCharPossuiUmCaractere.Execute(arrayChar, caractere);

            // Assert
            Assert.True(result);
        }
    }
}
