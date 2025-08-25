using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringPossuiElementoDeArrayStringTests
    {
        [Fact]
        public void Execute_QuandoContemElemento_DeveRetornarTrue()
        {
            var texto = "o rato roeu a roupa";
            var searchItems = new[] { "calça", "roupa", "rei" };
            Assert.True(StringPossuiElementoDeArrayString.Execute(texto, searchItems));
        }

        [Fact]
        public void Execute_QuandoNaoContemElemento_DeveRetornarFalse()
        {
            var texto = "o rato roeu a roupa";
            var searchItems = new[] { "calça", "rainha", "rei" };
            Assert.False(StringPossuiElementoDeArrayString.Execute(texto, searchItems));
        }

        [Fact]
        public void Execute_ComItemDeBuscaNulo_DeveIgnorarNuloEEncontrarValido()
        {
            var texto = "o rato roeu a roupa";
            var searchItems = new[] { "calça", null, "roupa" };
            Assert.True(StringPossuiElementoDeArrayString.Execute(texto, searchItems));
        }

        [Theory]
        [InlineData("abcde", null)]
        [InlineData("abcde", new string[0])]
        [InlineData(null, new[] { "a" })]
        [InlineData("", new[] { "a" })]
        public void Execute_ComInputsNulosOuVazios_DeveRetornarFalse(string texto, string[] searchItems)
        {
            Assert.False(StringPossuiElementoDeArrayString.Execute(texto, searchItems));
        }

        [Fact]
        public void Execute_ComBuscaCaseSensitive_DeveRespeitarCase()
        {
            var texto = "o Rato roeu a roupa";
            var searchItems = new[] { "rato" };
            Assert.False(StringPossuiElementoDeArrayString.Execute(texto, searchItems));
        }
    }
}
