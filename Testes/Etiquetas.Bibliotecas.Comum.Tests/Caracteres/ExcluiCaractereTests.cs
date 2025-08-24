using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ExcluiCaractereTests
    {
        [Fact]
        public void Execute_QuandoCaractereExiste_ExcluiTodasOcorrencias()
        {
            // Arrange
            var texto = "banana";
            var caractere = 'a';
            var expected = "bnn";

            // Act
            var result = ExcluiCaractere.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoCaractereNaoExiste_NaoAlteraTexto()
        {
            // Arrange
            var texto = "banana";
            var caractere = 'x';
            var expected = "banana";

            // Act
            var result = ExcluiCaractere.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoEhNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;
            var caractere = 'a';

            // Act
            var result = ExcluiCaractere.Execute(texto, caractere);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_QuandoTextoEVazio_RetornaVazio()
        {
            // Arrange
            var texto = "";
            var caractere = 'a';
            var expected = "";

            // Act
            var result = ExcluiCaractere.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoSoTemOCaractere_RetornaVazio()
        {
            // Arrange
            var texto = "aaaa";
            var caractere = 'a';
            var expected = "";

            // Act
            var result = ExcluiCaractere.Execute(texto, caractere);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
