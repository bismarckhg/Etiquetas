using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringContemStringInsensitiveAcentoInsensitiveCaseTests
    {
        [Theory]
        [InlineData("Maçã", "maca", true)]
        [InlineData("Ação e Reação", "acao", true)]
        [InlineData("Teste de Código", "CODIGO", true)]
        [InlineData("PROJETO", "projeto", true)]
        [InlineData("Coração", "coracao", true)]
        [InlineData("Pêssego", "pessego", true)]
        [InlineData("pinguim", "pingüim", true)]
        [InlineData("abc", "xyz", false)]
        public void Execute_ComInputsValidos_DeveRetornarResultadoCorreto(string texto, string contem, bool esperado)
        {
            // Act
            var resultado = StringContemStringInsensitiveAcentoInsensitiveCase.Execute(texto, contem);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData(null, "a")]
        [InlineData("a", null)]
        [InlineData(null, null)]
        [InlineData("", "a")]
        [InlineData("a", "")]
        [InlineData(" ", "a")]
        [InlineData("a", " ")]
        public void Execute_ComInputsNulosOuVazios_DeveRetornarFalse(string texto, string contem)
        {
            // Act
            var resultado = StringContemStringInsensitiveAcentoInsensitiveCase.Execute(texto, contem);

            // Assert
            Assert.False(resultado);
        }
    }
}
