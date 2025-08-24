using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Globalization;
using System.Threading;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class DecimalParaStringSemFormatacaoDaCulturaTests
    {
        [Fact]
        public void Execute_ComValorDecimal_RetornaStringComPonto()
        {
            // Arrange
            decimal value = 123.45m;
            var expected = "123.45";

            // Forçar uma cultura que usa vírgula para garantir que a conversão é invariante
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            // Act
            var result = DecimalParaStringSemFormatacaoDaCultura.Execute(value);

            // Assert
            Assert.Equal(expected, result);

            // Cleanup
            Thread.CurrentThread.CurrentCulture = originalCulture;
        }

        [Fact]
        public void Execute_ComZero_RetornaStringZero()
        {
            // Arrange
            decimal value = 0m;
            var expected = "0";

            // Act
            var result = DecimalParaStringSemFormatacaoDaCultura.Execute(value);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
