using Xunit;
using Etiquetas.Bibliotecas.Comum.Geral;
using System.Globalization;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Tests.Geral
{
    public class CulturaTests
    {
        [Fact]
        public void Padrao_NaoEhNulo()
        {
            // Arrange & Act
            var culturaPadrao = Cultura.Padrao;

            // Assert
            Assert.NotNull(culturaPadrao);
        }

        [Fact]
        public void CodePage_CorrespondeACulturaPadrao()
        {
            // Arrange
            var expectedCodePage = Cultura.Padrao.TextInfo.ANSICodePage;

            // Act
            var actualCodePage = Cultura.CodePage;

            // Assert
            Assert.Equal(expectedCodePage, actualCodePage);
        }

        [Fact]
        public void EncodingIdioma_CorrespondeAoCodePage()
        {
            // Arrange
            // Precisa registrar o provedor para GetEncoding funcionar em .NET Core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var expectedEncoding = Encoding.GetEncoding(Cultura.CodePage);

            // Act
            var actualEncoding = Cultura.EncodingIdioma;

            // Assert
            Assert.Equal(expectedEncoding, actualEncoding);
        }

        [Fact]
        public void EncodingPadrao_EhUTF8()
        {
            // Arrange & Act & Assert
            Assert.Equal(Encoding.UTF8, Cultura.EncodingPadrao);
        }
    }
}
