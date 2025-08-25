using System;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class TextoPrefixoStringInsensitiveAcentoInsensitiveCaseTests
    {
        [Theory]
        [InlineData("Maçã", "maca", true)]
        [InlineData("Ação e Reação", "acao", true)]
        [InlineData("Pêssego", "pess", true)]
        [InlineData("pinguim", "pingü", true)]
        [InlineData("TESTE", "test", true)]
        [InlineData("abc", "xyz", false)]
        [InlineData("abc", "b", false)]
        [InlineData("abc", "abcde", false)]
        [InlineData("", "", true)]
        [InlineData("abc", "", true)]
        public void Execute_ComInputsValidos_DeveRetornarResultadoEsperado(string texto, string prefixo, bool esperado)
        {
            // Act
            var resultado = TextoPrefixoStringInsensitiveAcentoInsensitiveCase.Execute(texto, prefixo);

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData(null, "a")]
        [InlineData("a", null)]
        [InlineData(null, null)]
        public void Execute_ComInputsNulos_DeveLancarExcecao(string texto, string prefixo)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => TextoPrefixoStringInsensitiveAcentoInsensitiveCase.Execute(texto, prefixo));
        }
    }
}
