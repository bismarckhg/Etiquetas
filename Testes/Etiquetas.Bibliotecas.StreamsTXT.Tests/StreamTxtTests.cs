using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Etiquetas.Bibliotecas.StreamsTXT;

namespace Etiquetas.Bibliotecas.StreamsTXT.Tests
{
    public class StreamTxtTests : IDisposable
    {
        private string _tempFilePath;

        // This constructor ensures each test gets a fresh file path.
        public StreamTxtTests()
        {
            _tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
        }

        [Fact]
        public async Task EscreverAsync_And_LerAsync_ShouldWriteAndReadCorrectly()
        {
            // Arrange
            var streamTxt = new StreamTxt(_tempFilePath);
            var linesToWrite = new string[] { "Hello", "World" };

            // Act
            await streamTxt.EscreverAsync(linesToWrite[0]);
            await streamTxt.EscreverAsync(linesToWrite[1]);

            var resultLines = await streamTxt.LerAsync();

            // Assert
            Assert.Equal(2, resultLines.Length);
            Assert.Equal("Hello", resultLines[0]);
            Assert.Equal("World", resultLines[1]);
        }

        [Fact]
        public async Task PossuiDados_ShouldReturnCorrectState()
        {
            // Arrange
            var streamTxt = new StreamTxt(_tempFilePath);

            // Assert initial state
            Assert.False(streamTxt.PossuiDados());

            // Act & Assert after write
            await streamTxt.EscreverAsync("some data");
            Assert.True(streamTxt.PossuiDados());
        }

        [Fact]
        public async Task LerAsync_OnNonExistentFile_ShouldReturnEmptyArray()
        {
            // Arrange
            var streamTxt = new StreamTxt(_tempFilePath); // File does not exist yet

            // Act
            var result = await streamTxt.LerAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            // Ensure the temp file is deleted after tests
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }
    }
}
