using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class SubstituiStringTests
    {
        [Theory]
        [InlineData("abcde", "bcd", "xyz", "axyze")]
        [InlineData("banana", "a", "o", "bonono")]
        [InlineData("abc", "d", "e", "abc")]
        [InlineData("abcde", "bcd", "", "ae")]
        [InlineData("abcde", "bcd", null, "ae")] // textoNovo nulo é tratado como vazio
        public void Execute_ComInputsValidos_DeveSubstituirCorretamente(string texto, string antigo, string novo, string esperado)
        {
            // Act
            var result = SubstituiString.Execute(texto, antigo, novo);

            // Assert
            Assert.Equal(esperado, result);
        }

        [Theory]
        [InlineData(null, "a", "b")]
        [InlineData("", "a", "b")]
        [InlineData("abc", null, "d")]
        public void Execute_ComInputsNulosOuVazios_DeveRetornarTextoOriginal(string texto, string antigo, string novo)
        {
            // Act
            var result = SubstituiString.Execute(texto, antigo, novo);

            // Assert
            Assert.Equal(texto, result);
        }
    }
}
