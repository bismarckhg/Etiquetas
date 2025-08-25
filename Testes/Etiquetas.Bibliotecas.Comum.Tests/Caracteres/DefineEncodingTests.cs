using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class DefineEncodingTests
    {
        [Theory]
        [InlineData("ASCII", "us-ascii")]
        [InlineData("UTF-8", "utf-8")]
        [InlineData("UTF8", "utf-8")]
        [InlineData("UTF-16", "utf-16")]
        [InlineData("UNICODE", "utf-16")]
        [InlineData("ISO-8859-1", "iso-8859-1")]
        [InlineData("latin-1", "iso-8859-1")]
        [InlineData("1252", "windows-1252")]
        [InlineData("850", "ibm850")]
        [InlineData("437", "ibm437")]
        public void Execute_ComNomesValidos_RetornaEncodingCorreto(string nome, string expectedWebName)
        {
            // Arrange & Act
            var result = DefineEncoding.Execute(nome);

            // Assert
            Assert.Equal(expectedWebName, result.WebName);
        }

        [Fact]
        public void Execute_ComNomeInvalido_RetornaUTF8()
        {
            // Arrange
            var nome = "invalido";

            // Act
            var result = DefineEncoding.Execute(nome);

            // Assert
            Assert.Equal(Encoding.UTF8, result);
        }

        [Fact]
        public void Execute_ComNomeNulo_RetornaUTF8()
        {
            // Arrange
            string nome = null;

            // Act
            var result = DefineEncoding.Execute(nome);

            // Assert
            Assert.Equal(Encoding.UTF8, result);
        }

        [Fact]
        public void Execute_ComNomeVazio_RetornaUTF8()
        {
            // Arrange
            var nome = "";

            // Act
            var result = DefineEncoding.Execute(nome);

            // Assert
            Assert.Equal(Encoding.UTF8, result);
        }
    }
}
