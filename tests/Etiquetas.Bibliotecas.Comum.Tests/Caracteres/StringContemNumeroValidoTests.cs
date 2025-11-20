using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringContemNumeroValidoTests
    {
        [Theory]
        [InlineData("123", true)]
        [InlineData("-45", true)]
        [InlineData("0", true)]
        [InlineData("123.45", true)]
        [InlineData("-10.5", true)]
        [InlineData("abc", false)]
        [InlineData("1,234.56", false)] // TryParse com cultura padrão não aceita vírgulas
        [InlineData("$99.99", false)]   // TryParse com cultura padrão não aceita símbolos de moeda
        [InlineData("1 2", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData(" ", false)]
        public void Execute_ComDiferentesInputs_DeveRetornarResultadoEsperado(string texto, bool esperado)
        {
            // Act
            var resultado = StringContemNumeroValido.Execute(texto);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
