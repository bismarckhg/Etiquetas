using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class AlteraCaractereTests
    {
        [Fact]
        public void Execute_QuandoCaractereExiste_SubstituiCorretamente()
        {
            // Arrange
            var texto = "banana";
            var caractere = 'a';
            var novoCaractere = 'o';
            var expected = "bonono";

            // Act
            var result = AlteraCaractere.Execute(texto, caractere, novoCaractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoCaractereNaoExiste_NaoAlteraTexto()
        {
            // Arrange
            var texto = "banana";
            var caractere = 'x';
            var novoCaractere = 'o';
            var expected = "banana";

            // Act
            var result = AlteraCaractere.Execute(texto, caractere, novoCaractere);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoEhNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;
            var caractere = 'a';
            var novoCaractere = 'o';

            // Act
            var result = AlteraCaractere.Execute(texto, caractere, novoCaractere);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_QuandoTextoEstaVazio_RetornaTextoVazio()
        {
            // Arrange
            var texto = "";
            var caractere = 'a';
            var novoCaractere = 'o';
            var expected = "";

            // Act
            var result = AlteraCaractere.Execute(texto, caractere, novoCaractere);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
