using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringContemNumeroInteiroTests
    {
        [Theory]
        [InlineData("123", true)]
        [InlineData("-45", true)]
        [InlineData("0", true)]
        [InlineData("abc", false)]
        [InlineData("12.34", false)]
        [InlineData("1 2", false)]
        [InlineData("999999999999999999", false)] // Maior que int.MaxValue
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        public void Execute_ComDiferentesInputs_DeveRetornarResultadoEsperado(string texto, bool esperado)
        {
            // Act
            var resultado = StringContemNumeroInteiro.Execute(texto);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
