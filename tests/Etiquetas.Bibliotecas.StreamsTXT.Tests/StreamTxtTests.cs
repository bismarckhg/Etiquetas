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
            //_tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".txt");
            _tempFilePath = Path.Combine("C:\\Temp\\", Guid.NewGuid().ToString() + ".txt");
            //_tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".txt");
            //_tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".txt");
            Console.WriteLine($"Temporary file path: {_tempFilePath}");
        }

        [Fact]
        public async Task EscreverAsync_And_LerAsync_ShouldWriteAndReadCorrectly()
        {
            // Arrange
            var streamTxt = new StreamTxt();
            var linesToWrite = new string[] { "Hello", "World", "Teste", "leitura", "escrita" };

            // Act
            await streamTxt.ConectarAsync(_tempFilePath).ConfigureAwait(false);
            var parametros = new object[] { "Texto" };

            foreach (var line in linesToWrite)
            {
                parametros[0] = line; // Update the line to write
                await streamTxt.EscreverAsync<string>(parametros);
            }

            await streamTxt.FecharAsync().ConfigureAwait(false);

            await streamTxt.ConectarAsync(_tempFilePath).ConfigureAwait(false);
            var resultLines = await streamTxt.LerAsync<string[]>();
            await streamTxt.FecharAsync().ConfigureAwait(false);

            // Assert
            Assert.Equal(5, resultLines.Length);
            Assert.Equal("Hello\r\n", resultLines[0]);
            Assert.Equal("World\r\n", resultLines[1]);
            Assert.Equal("Teste\r\n", resultLines[2]);
            Assert.Equal("leitura\r\n", resultLines[3]);
            Assert.Equal("escrita\r\n", resultLines[4]);
        }

        [Fact]
        public async Task PossuiDados_ShouldReturnCorrectState()
        {
            // Arrange
            var streamTxt = new StreamTxt();

            await streamTxt.ConectarAsync(_tempFilePath).ConfigureAwait(false);

            // Assert initial state
            var semDados = streamTxt.PossuiDados();
            Assert.False(semDados);

            // Act & Assert after write
            await streamTxt.EscreverAsync<string>(new string[] { "some data" });
            var comDados = streamTxt.PossuiDados();
            await streamTxt.FecharAsync().ConfigureAwait(false);
            Assert.True(comDados);
        }

        [Fact]
        public async Task LerAsync_OnNonExistentFile_ShouldReturnEmptyArray()
        {
            // Arrange
            var streamTxt = new StreamTxt(); // File does not exist yet

            // Act
            await streamTxt.ConectarAsync(_tempFilePath).ConfigureAwait(false);
            await Assert.ThrowsAnyAsync<FileNotFoundException>(async () => await streamTxt.LerAsync<string>()).ConfigureAwait(false);
            await streamTxt.FecharAsync().ConfigureAwait(false);

            // Assert

            // Assert.NotNull(result);
            // Assert.Empty(result);
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
