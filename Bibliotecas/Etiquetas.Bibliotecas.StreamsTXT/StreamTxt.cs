using Etiquetas.Bibliotecas.Comum.Caracteres;
using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.StreamsTXT
{
    public class StreamTxt : StreamBaseTXT, IStreamLeitura<string>, IStreamEscrita<string>
    {
        public CancellationToken CancelToken { get; set; }

        public Encoding EncodingTexto { get; set; }

        public StreamTxt()
        {
            this.CancelToken = CancellationToken.None;
            this.EncodingTexto = Encoding.UTF8;
        }

        // IStreamLeitura<T> is now for string[], not string.
        public async Task<string> LerAsync(params object[] parametros)
        {
            if (!File.Exists(NomeECaminhoArquivo))
            {
                throw new FileNotFoundException("Arquivo não encontrado.", NomeECaminhoArquivo);
            }

            await Task.Run(() => ArrayObjectosParametrosLerEscreverAsync(parametros)).ConfigureAwait(false);

            // Replace ReadAllLinesAsync with a workaround for older .NET versions.
            // return await File.ReadAllLinesAsync(_caminhoArquivo, Encoding.UTF8).ConfigureAwait(false);
            //return await Task.Run(() => File.ReadAllLines(NomeECaminhoArquivo, Encoding.UTF8)).ConfigureAwait(false);
            return await LerAsync().ConfigureAwait(false);
        }

        public async Task<string> LerAsync(ITaskParametros parametros)
        {
            this.CancelToken = parametros.RetornoCancellationToken;
            this.EncodingTexto = parametros.RetornoEncoding;
            return await LerAsync().ConfigureAwait(false);
        }

        protected async Task<string> LerAsync()
        {
            using (FS) // Garante o fechamento do FileStream após a leitura
            using (var sr = new StreamReader(
                FS,
                EncodingTexto,
                detectEncodingFromByteOrderMarks: false
                )
            )   // ANSI não usa BOM; força 1252 ))
            {
                this.CancelToken.ThrowIfCancellationRequested();
                return await sr.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        protected Task<string> ArrayObjectosParametrosLerEscreverAsync(params object[] parametros)
        {
            var dados = string.Empty;
            if (parametros == null || parametros.Length == 0)
                throw new ArgumentNullException("Parâmetros inválidos!");

            var pendente = new ConcurrentDictionary<Type, int>
            {
                [typeof(string)] = 1,
                [typeof(CancellationToken)] = 1,
                [typeof(Encoding)] = 2
            };
            var lista = new ConcurrentDictionary<Type, int>();
            foreach (var item in parametros)
            {
                var ok = false;
                var posicao = 0;
                switch (item)
                {
                    case string retorno:
                        dados = EhStringNuloVazioComEspacosBranco.Execute(retorno)
                               ? throw new ArgumentNullException("Dados Vazio ou nulo!")
                               : retorno;
                        ok = pendente.TryRemove(dados.GetType(), out posicao)
                            ? lista.TryAdd(dados.GetType(), posicao)
                            : throw new ArgumentException("Parametro Dados Duplicado!!");
                        break;
                    case CancellationToken cancellationToken:
                        CancelToken = cancellationToken;
                        ok = pendente.TryRemove(cancellationToken.GetType(), out posicao)
                            ? lista.TryAdd(cancellationToken.GetType(), posicao)
                            : throw new ArgumentException("Parametro FileMode Duplicado!!");
                        break;
                    case Encoding encoding:
                        EncodingTexto = encoding;
                        ok = pendente.TryRemove(encoding.GetType(), out posicao)
                            ? lista.TryAdd(encoding.GetType(), posicao)
                            : throw new ArgumentException("Parametro FileAccess Duplicado!!");
                        break;
                    default:
                        break;
                }
            }

            if (pendente.Count != 0)
            {
                foreach (var item in pendente)
                {
                    var nomeTipo = item.Key.Name;
                    throw new ArgumentException($"Parâmetro obrigatório não fornecido: {nomeTipo}");
                }
            }

            return Task.FromResult(dados);
        }

        public async Task EscreverAsync(params object[] parametros)
        {

            var dados = await Task.Run(() => ArrayObjectosParametrosLerEscreverAsync(parametros)).ConfigureAwait(false);

            // Replace AppendAllTextAsync with a workaround for older .NET versions.
            // await File.AppendAllTextAsync(_caminhoArquivo, dados + Environment.NewLine, Encoding.UTF8);
            //await Task.Run(() =>
            //{
            //    File.AppendAllText(NomeECaminhoArquivo, dados + Environment.NewLine, Encoding.UTF8);
            //}).ConfigureAwait(false);
            await EscreverAsync(dados).ConfigureAwait(false);
        }

        public async Task EscreverAsync(ITaskParametros parametros)
        {
            this.CancelToken = parametros.RetornoCancellationToken;
            this.EncodingTexto = parametros.RetornoEncoding;
            var dados = (string)(parametros["Dados"]);
            await EscreverAsync(dados).ConfigureAwait(false);
        }

        protected async Task EscreverAsync(string dados)
        {
            using (FS) // Garante o fechamento do FileStream após a escrita
            using (var sw = new StreamWriter(
                FS,
                EncodingTexto,
                    bufferSize: 4096,
                    leaveOpen: true
                )
            )   // ANSI não usa BOM; força 1252 ))
            {
                this.CancelToken.ThrowIfCancellationRequested();
                await sw.WriteAsync(dados).ConfigureAwait(false);
            }
        }
        public override bool EstaAberto()
        {
            // "Open" is transient, but we can say it's always ready if the path exists.
            return File.Exists(NomeECaminhoArquivo);
        }

        public override bool PossuiDados()
        {
            try
            {
                if (File.Exists(NomeECaminhoArquivo))
                {
                    return new FileInfo(NomeECaminhoArquivo).Length > 0;
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
