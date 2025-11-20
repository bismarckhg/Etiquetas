using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class TextoCompareStringInsensitiveAcentoInsensitiveCaseTests
    {
        [Theory]
        [InlineData("maçã", "Maça", true)]
        [InlineData("Ação", "acao", true)]
        [InlineData("Pêssego", "pessego", true)]
        [InlineData("pinguim", "pingüim", true)]
        [InlineData("teste", "TESTE", true)]
        [InlineData("abc", "def", false)]
        [InlineData(null, null, true)]
        [InlineData("abc", null, false)]
        [InlineData(null, "abc", false)]
        [InlineData("", "", true)]
        public void Execute_ComDiferentesInputs_DeveRetornarResultadoEsperado(string texto1, string texto2, bool esperado)
        {
            // Act
            var resultado = TextoCompareStringInsensitiveAcentoInsensitiveCase.Execute(texto1, texto2);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
