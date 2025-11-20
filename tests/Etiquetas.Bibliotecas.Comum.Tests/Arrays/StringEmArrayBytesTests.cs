using System.Text;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class StringEmArrayBytesTests
    {
        [Fact]
        public void ExecuteASCII_ShouldConvertStringToAsciiBytes()
        {
            // Arrange
            string input = "Hello";
            byte[] expected = new byte[] { 72, 101, 108, 108, 111 };

            // Act
            byte[] result = StringEmArrayBytes.ExecuteASCII(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExecuteUTF8_ShouldConvertStringToUtf8Bytes()
        {
            // Arrange
            string input = "Olá";
            byte[] expected = new byte[] { 79, 108, 195, 161 };

            // Act
            byte[] result = StringEmArrayBytes.ExecuteUTF8(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_WithCustomEncoding_ShouldConvertCorrectly()
        {
            // Arrange
            string input = "€"; // Euro sign U+20AC
            Encoding utf32 = Encoding.UTF32;
            byte[] expected = new byte[] { 172, 32, 0, 0 }; // UTF-32 LE

            // Act
            byte[] result = StringEmArrayBytes.Execute(utf32, input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExecuteASCII_WithNull_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() => StringEmArrayBytes.ExecuteASCII(null));
        }

        [Fact]
        public void ExecuteUTF8_WithNull_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() => StringEmArrayBytes.ExecuteUTF8(null));
        }
    }
}
