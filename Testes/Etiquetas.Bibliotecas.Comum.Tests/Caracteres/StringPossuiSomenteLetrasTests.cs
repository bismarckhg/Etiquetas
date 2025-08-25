using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringPossuiSomenteLetrasTests
    {
        [Theory]
        [InlineData("abc", true)]
        [InlineData("AbcDef", true)]
        [InlineData("Ação", true)] // char.IsLetter considera acentos
        [InlineData("abc1", false)]
        [InlineData("abc ", false)]
        [InlineData("123", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void Execute_ComDiferentesInputs_DeveRetornarResultadoEsperado(string texto, bool esperado)
        {
            // Act
            var resultado = StringPossuiSomenteLetras.Execute(texto);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
