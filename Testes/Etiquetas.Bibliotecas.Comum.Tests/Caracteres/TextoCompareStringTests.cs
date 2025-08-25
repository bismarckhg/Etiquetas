using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class TextoCompareStringTests
    {
        [Theory]
        [InlineData("abc", "abc", true)]
        [InlineData("abc", "def", false)]
        [InlineData("abc", "ABC", false)]
        [InlineData("abc", null, false)]
        [InlineData(null, "abc", false)]
        [InlineData(null, null, true)]
        [InlineData("", "", true)]
        [InlineData("a", "", false)]
        public void Execute_ComDiferentesInputs_DeveRetornarResultadoEsperado(string texto1, string texto2, bool esperado)
        {
            // Act
            var resultado = TextoCompareString.Execute(texto1, texto2);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
