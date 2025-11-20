using System.Text;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Geral;
using System.Globalization;

namespace Etiquetas.Bibliotecas.Comum.Tests.Geral
{
    public class CulturaTests
    {
        static CulturaTests()
        {
            // Register the encoding provider to enable legacy codepages like 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Fact]
        public void EncodingPadrao_DeveSerUTF8()
        {
            // Assert
            Assert.Equal(Encoding.UTF8, Cultura.EncodingPadrao);
        }

        [Fact]
        public void Padrao_NaoDeveSerNulo()
        {
            // Assert
            Assert.NotNull(Cultura.Padrao);
        }

        [Fact]
        public void CodePage_DeveSerConsistenteComCulturaPadrao()
        {
            // Arrange
            var expectedCodePage = CultureInfo.CurrentCulture.TextInfo.ANSICodePage;

            // Assert
            Assert.Equal(expectedCodePage, Cultura.CodePage);
        }

        [Fact]
        public void EncodingIdioma_DeveSerConsistenteComCodePage()
        {
            // Arrange
            var expectedEncoding = Encoding.GetEncoding(Cultura.CodePage);

            // Assert
            Assert.Equal(expectedEncoding, Cultura.EncodingIdioma);
        }
    }
}
