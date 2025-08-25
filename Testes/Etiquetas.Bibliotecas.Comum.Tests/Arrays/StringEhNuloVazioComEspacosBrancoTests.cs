using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class StringEhNuloVazioComEspacosBrancoTests
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData(" ", true)]
        [InlineData("  ", true)]
        [InlineData("a", false)]
        [InlineData(" a ", false)]
        public void Execute_ComDiferentesInputs_DeveRetornarResultadoEsperado(string texto, bool esperado)
        {
            // Act
            var resultado = StringEhNuloVazioComEspacosBranco.Execute(texto);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
