using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Streams.Core
{
    public class StreamTxt : StreamBaseTXT, IStreamLeitura<string>, IStreamEscrita<string>
    {
        //private readonly string _caminhoArquivo;

        public StreamTxt()
        {
        }

        // Connections are now handled per operation, so these do nothing.
        public override Task ConectarAsync(params object[] parametros) => Task.CompletedTask;
        public override Task FecharAsync() => Task.CompletedTask;

        // IStreamLeitura<T> is now for string[], not string.
        public async Task<string> LerAsync(params object[] parametros)
        {
            if (!File.Exists(NomeECaminhoArquivo))
            {
                return string.Empty;
            }

            //// Replace ReadAllLinesAsync with a workaround for older .NET versions.
            //// return await File.ReadAllLinesAsync(_caminhoArquivo, Encoding.UTF8).ConfigureAwait(false);
            //return await Task.Run(() => File.ReadAllLines(_caminhoArquivo, Encoding.UTF8)).ConfigureAwait(false);
            var cancellationToken = CancellationToken.None;
            var encoding = Encoding.UTF8;

            foreach (var item in parametros)
            {
                if (item is CancellationToken ct)
                {
                    cancellationToken = ct;
                }

                if (item is Encoding enc)
                {
                    encoding = enc;
                }
            }

            using (var sr = new StreamReader(
                FS,
                encoding,
                detectEncodingFromByteOrderMarks: false
                )
            )   // ANSI não usa BOM; força 1252 ))
            {
                cancellationToken.ThrowIfCancellationRequested();
                return await sr.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        public async Task EscreverAsync(string dados, params object[] parametros)
        {
            var cancellationToken = CancellationToken.None;
            var encoding = Encoding.UTF8;

            foreach (var item in parametros)
            {
                if (item is CancellationToken ct)
                {
                    cancellationToken = ct;
                }

                if (item is Encoding enc)
                {
                    encoding = enc;
                }
            }

            // Replace AppendAllTextAsync with a workaround for older .NET versions.
            // await File.AppendAllTextAsync(_caminhoArquivo, dados + Environment.NewLine, Encoding.UTF8);
            await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var sw = new StreamWriter(
                    FS,
                    encoding,
                    bufferSize: 4096,
                    leaveOpen: true
                    )
                )   // ANSI não usa BOM; força 1252 ))
                {
                    sw.WriteLine(dados);
                }
            }).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            // No unmanaged resources to dispose in this simplified model.
            base.Dispose(disposing);
        }
    }
}
