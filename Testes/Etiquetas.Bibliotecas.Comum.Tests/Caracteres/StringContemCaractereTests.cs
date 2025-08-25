using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringContemCaractereTests
    {
        [Theory]
        [InlineData("abc", 'b', true)]
        [InlineData("abc", 'd', false)]
        public void Execute_ComInputsValidos_DeveRetornarResultadoCorreto(string texto, char caractere, bool esperado)
        {
            // Act
            var resultado = StringContemCaractere.Execute(texto, caractere);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public void Execute_ComCaractereDeEspaco_DeveRetornarFalse()
        {
            // Arrange
            var texto = "a b";
            var caractere = ' ';

            // Act
            var resultado = StringContemCaractere.Execute(texto, caractere);

            // Assert
            Assert.False(resultado);
        }

        [Theory]
        [InlineData(null, 'a')]
        [InlineData("", 'a')]
        [InlineData(" ", 'a')]
        public void Execute_ComTextoNuloOuVazio_DeveRetornarFalse(string texto, char caractere)
        {
            // Act
            var resultado = StringContemCaractere.Execute(texto, caractere);

            // Assert
            Assert.False(resultado);
        }
    }
}
