using Etiquetas.Bibliotecas.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Etiquetas.Bibliotecas.Json.Tests
{
    public class DataObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class StreamJsonTests
    {
        [Fact]
        public async Task EscreverAsync_DeveCriarEGravarDadosNoArquivo()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
            var streamJson = new StreamJson<DataObject>(filePath);
            var data = new DataObject { Id = 1, Name = "Test" };

            try
            {
                await streamJson.EscreverAsync(data);

                Assert.True(File.Exists(filePath));

#if NET472
                var fileContent = await Task.Run(() => File.ReadAllText(filePath));
#else
                var fileContent = await File.ReadAllTextAsync(filePath);
#endif
                Assert.Contains("\"Id\": 1", fileContent);
                Assert.Contains("\"Name\": \"Test\"", fileContent);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [Fact]
        public async Task LerAsync_DeveLerEDeserializarDadosDoArquivo()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
            var streamJson = new StreamJson<DataObject>(filePath);
            var data = new DataObject { Id = 1, Name = "Test" };

            try
            {
                await streamJson.EscreverAsync(data);

                var readData = await streamJson.LerAsync();

                Assert.NotNull(readData);
                Assert.Equal(data.Id, readData.Id);
                Assert.Equal(data.Name, readData.Name);
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
