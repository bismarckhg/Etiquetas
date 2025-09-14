using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.StreamsTXT
{
    public class StreamTxt : StreamBase, IStreamLeitura<string[]>, IStreamEscrita<string>
    {
        private readonly string _caminhoArquivo;

        public StreamTxt(string caminhoArquivo)
        {
            if (string.IsNullOrEmpty(caminhoArquivo))
                throw new ArgumentNullException(nameof(caminhoArquivo));
            _caminhoArquivo = caminhoArquivo;
        }

        // Connections are now handled per operation, so these do nothing.
        public override Task ConectarAsync(params object[] parametros) => Task.CompletedTask;
        public override Task FecharAsync() => Task.CompletedTask;

        // IStreamLeitura<T> is now for string[], not string.
        public async Task<string[]> LerAsync(params object[] parametros)
        {
            if (!File.Exists(_caminhoArquivo))
            {
                return Array.Empty<string>();
            }

            // Replace ReadAllLinesAsync with a workaround for older .NET versions.
            // return await File.ReadAllLinesAsync(_caminhoArquivo, Encoding.UTF8).ConfigureAwait(false);
            return await Task.Run(() => File.ReadAllLines(_caminhoArquivo, Encoding.UTF8)).ConfigureAwait(false);
        }

        public async Task EscreverAsync(string dados, params object[] parametros)
        {
            // Replace AppendAllTextAsync with a workaround for older .NET versions.
            // await File.AppendAllTextAsync(_caminhoArquivo, dados + Environment.NewLine, Encoding.UTF8);
            await Task.Run(() =>
            {
                File.AppendAllText(_caminhoArquivo, dados + Environment.NewLine, Encoding.UTF8);
            }).ConfigureAwait(false);
        }

        public override bool EstaAberto()
        {
            // "Open" is transient, but we can say it's always ready if the path exists.
            return File.Exists(_caminhoArquivo);
        }

        public override bool PossuiDados()
        {
            try
            {
                if (File.Exists(_caminhoArquivo))
                {
                    return new FileInfo(_caminhoArquivo).Length > 0;
                }
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            // No unmanaged resources to dispose in this simplified model.
            base.Dispose(disposing);
        }
    }
}
