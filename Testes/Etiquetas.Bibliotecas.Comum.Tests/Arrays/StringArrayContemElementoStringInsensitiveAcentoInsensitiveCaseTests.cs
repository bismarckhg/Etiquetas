using System;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class StringArrayContemElementoStringInsensitiveAcentoInsensitiveCaseTests
    {
        [Theory]
        [InlineData(new[] { "maçã", "banana" }, "maca", true)]
        [InlineData(new[] { "Maçã", "banana" }, "maçã", true)]
        [InlineData(new[] { "Coração", "Amor" }, "coracao", true)]
        [InlineData(new[] { "teste" }, "TESTE", true)]
        [InlineData(new[] { "teste" }, "outro", false)]
        public void Execute_ComDiferentesCasosEFormatos_DeveRetornarResultadoEsperado(string[] array, string valor, bool esperado)
        {
            var resultado = StringArrayContemElementoStringInsensitiveAcentoInsensitiveCase.Execute(array, valor);
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public void Execute_ComStringDeBuscaNula_EArrayNaoVazio_DeveRetornarFalse()
        {
            // Comportamento atual devido à lógica com parametrosVazio
            var resultado = StringArrayContemElementoStringInsensitiveAcentoInsensitiveCase.Execute(new[] { "a" }, null);
            Assert.False(resultado);
        }

        [Fact]
        public void Execute_ComStringDeBuscaVazia_EArrayNaoVazio_DeveRetornarFalse()
        {
            // Comportamento atual devido à lógica com parametrosVazio
            var resultado = StringArrayContemElementoStringInsensitiveAcentoInsensitiveCase.Execute(new[] { "a", "" }, "");
            Assert.False(resultado);
        }

        [Fact]
        public void Execute_ComStringDeBuscaVazia_EArrayVazio_DeveRetornarTrue()
        {
            var resultado = StringArrayContemElementoStringInsensitiveAcentoInsensitiveCase.Execute(new string[0], "");
            Assert.True(resultado);
        }

        [Fact]
        public void Execute_ComArrayNulo_DeveLancarArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringArrayContemElementoStringInsensitiveAcentoInsensitiveCase.Execute(null, "teste"));
        }

        [Fact]
        public void Execute_ComArrayVazio_EStringNaoVazia_DeveRetornarFalse()
        {
            var resultado = StringArrayContemElementoStringInsensitiveAcentoInsensitiveCase.Execute(new string[0], "teste");
            Assert.False(resultado);
        }
    }
}
