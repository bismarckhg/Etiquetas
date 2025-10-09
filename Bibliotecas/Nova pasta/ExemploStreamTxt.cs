using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.StreamsTXT.Exemplo
{
    public class ExemploStreamTxt
    {
        public async Task Executar()
        {

            var _tempFilePath = Path.Combine("C:\\Temp\\", Guid.NewGuid().ToString() + ".txt");
            Console.WriteLine($"Arquivo temporário: {_tempFilePath}");

            // Arrange
            var streamTxt = new StreamTxt();
            var linesToWrite = new string[] { "Hello", "World", "Teste", "leitura", "escrita" };

            // Act
            await streamTxt.ConectarAsync(_tempFilePath).ConfigureAwait(false);

            Console.WriteLine("Conectado ao arquivo...");

            var parametros = new object[] { "Texto" };

            foreach (var line in linesToWrite)
            {
                Console.WriteLine($"Escrevendo linha: {line}");
                parametros[0] = line; // Update the line to write
                await streamTxt.EscreverAsync<string>(parametros);
                Console.WriteLine($"Escreveu linha: {line}");
            }

            await streamTxt.FecharAsync().ConfigureAwait(false);
            Console.WriteLine("Arquivo fechado após escrita.");

            await streamTxt.ConectarAsync(_tempFilePath).ConfigureAwait(false);
            Console.WriteLine("Conectado ao arquivo...");

            var resultLines = await streamTxt.LerAsync<string>();
            Console.WriteLine("Leu linhas do arquivo: {0}", resultLines);

            await streamTxt.FecharAsync().ConfigureAwait(false);
            Console.WriteLine("Arquivo fechado após leitura.");
        }
    }
}
