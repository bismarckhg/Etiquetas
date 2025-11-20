using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringPossuiSomenteDigitosNumericosTests
    {
        [Theory]
        [InlineData("12345", true)]
        [InlineData("0987", true)]
        [InlineData("123a", false)]
        [InlineData("123 45", false)]
        [InlineData("-123", false)]
        [InlineData("12.3", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void Execute_ComDiferentesInputs_DeveRetornarResultadoEsperado(string texto, bool esperado)
        {
            // Act
            var resultado = StringPossuiSomenteDigitosNumericos.Execute(texto);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
