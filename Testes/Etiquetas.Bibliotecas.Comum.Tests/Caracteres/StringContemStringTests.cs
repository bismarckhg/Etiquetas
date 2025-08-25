using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class StringContemStringTests
    {
        [Theory]
        [InlineData("abcde", "bcd", true)]
        [InlineData("abcde", "xyz", false)]
        [InlineData("abcde", "", true)] // Comportamento padrão do .Contains("")
        [InlineData("", "", true)]
        [InlineData("abcde", "abcde", true)]
        [InlineData("abc", "ABC", false)] // Deve ser case-sensitive
        [InlineData(null, "a", false)]
        [InlineData("a", null, false)]
        [InlineData(null, null, false)]
        public void Execute_ComDiferentesInputs_DeveRetornarResultadoEsperado(string texto, string contem, bool esperado)
        {
            // Act
            var resultado = StringContemString.Execute(texto, contem);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
