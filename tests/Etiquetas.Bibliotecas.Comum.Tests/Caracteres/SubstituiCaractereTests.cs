using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class SubstituiCaractereTests
    {
        [Fact]
        public void Execute_QuandoCaractereExiste_DeveSubstituirTodasAsOcorrencias()
        {
            // Arrange
            var texto = "banana";
            var charAntigo = 'a';
            var charNovo = 'o';
            var expected = "bonono";

            // Act
            var result = SubstituiCaractere.Execute(texto, charAntigo, charNovo);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoCaractereNaoExiste_DeveRetornarStringOriginal()
        {
            // Arrange
            var texto = "banana";
            var charAntigo = 'x';
            var charNovo = 'o';

            // Act
            var result = SubstituiCaractere.Execute(texto, charAntigo, charNovo);

            // Assert
            Assert.Equal(texto, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_DeveRetornarNulo()
        {
            // Arrange
            string texto = null;

            // Act
            var result = SubstituiCaractere.Execute(texto, 'a', 'b');

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComTextoVazio_DeveRetornarVazio()
        {
            // Arrange
            var texto = "";

            // Act
            var result = SubstituiCaractere.Execute(texto, 'a', 'b');

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
