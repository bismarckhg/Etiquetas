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
    public class StreamTxt : StreamBaseTXT, IStreamLeitura, IStreamEscrita
    {
        public CancellationToken CancelToken { get; set; }

        public Encoding EncodingTexto { get; set; }

        private StreamWriter Writer { get; set; }
        private StreamReader Reader { get; set; }

        public StreamTxt()
        {
            this.CancelToken = CancellationToken.None;
            this.EncodingTexto = Encoding.UTF8;
        }

        // IStreamLeitura<T> is now for string[], not string.
        public async Task<T> LerAsync<T>(params object[] parametros)
        {
            if (!File.Exists(NomeECaminhoArquivo))
            {
                throw new FileNotFoundException("Arquivo não encontrado.", NomeECaminhoArquivo);
            }

            await Task.Run(() => ArrayObjectosParametrosLerAsync(parametros)).ConfigureAwait(false);

            // Replace ReadAllLinesAsync with a workaround for older .NET versions.
            // return await File.ReadAllLinesAsync(_caminhoArquivo, Encoding.UTF8).ConfigureAwait(false);
            //return await Task.Run(() => File.ReadAllLines(NomeECaminhoArquivo, Encoding.UTF8)).ConfigureAwait(false);
            // Substitua a linha abaixo:
            // return await LerAsync().ConfigureAwait(false);

            // Por esta linha, especificando explicitamente o tipo T:
            return await LerAsync<T>().ConfigureAwait(false);
        }

        public async Task<T> LerAsync<T>(ITaskParametros parametros)
        {
            this.CancelToken = parametros.RetornoCancellationToken;
            this.EncodingTexto = parametros.RetornoEncoding;
            return await LerAsync<T>().ConfigureAwait(false);
        }

        protected async Task<string> LerAsync<T>(T objeto)
        {
            if (EstaAberto() == false)
            {
                await ConectarReaderOnlyUnshareAsync().ConfigureAwait(false);
                this.Reader = new StreamReader(FS, EncodingTexto, detectEncodingFromByteOrderMarks: false, bufferSize: 4096, leaveOpen: true);
            }

            this.CancelToken.ThrowIfCancellationRequested();
            return await this.Reader.ReadToEndAsync().ConfigureAwait(false);
        }

        protected async Task ArrayObjectosParametrosLerAsync(params object[] parametros)
        {
            var pendente = new ConcurrentDictionary<Type, int>
            {
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
        }

        protected Task<string> ArrayObjectosParametrosEscreverAsync(params object[] parametros)
        {
            var dados = string.Empty;
            if (parametros == null || parametros.Length == 0)
                throw new ArgumentNullException("Parâmetros inválidos!");

            var pendente = new ConcurrentDictionary<Type, int>
            {
                [typeof(string)] = 1,
                [typeof(CancellationToken)] = 2,
                [typeof(Encoding)] = 3
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
                var tipo = string.Empty;
                foreach (var item in pendente)
                {
                    var nomeTipo = item.Key.Name;
                    //throw new ArgumentException($"Parâmetro obrigatório não fornecido: {nomeTipo}");
                    if (nomeTipo.ToUpper() == "STRING")
                    {
                        tipo = nomeTipo;
                        break;
                    }
                }
                if (tipo == "STRING")
                {
                    throw new ArgumentException($"Parâmetro obrigatório não fornecido: {tipo}");
                }
            }
            return Task.FromResult(dados);
        }

        public async Task EscreverAsync<T>(params object[] parametros)
        {

            var dados = await Task.Run(() => ArrayObjectosParametrosEscreverAsync(parametros)).ConfigureAwait(false);

            // Replace AppendAllTextAsync with a workaround for older .NET versions.
            // await File.AppendAllTextAsync(_caminhoArquivo, dados + Environment.NewLine, Encoding.UTF8);
            //await Task.Run(() =>
            //{
            //    File.AppendAllText(NomeECaminhoArquivo, dados + Environment.NewLine, Encoding.UTF8);
            //}).ConfigureAwait(false);
            await EscreverAsync(dados).ConfigureAwait(false);
        }

        public async Task EscreverAsync<T>(ITaskParametros parametros)
        {
            this.CancelToken = parametros.RetornoCancellationToken;
            this.EncodingTexto = parametros.RetornoEncoding;
            var dados = (string)(parametros["Dados"]);
            await EscreverAsync<T>(dados).ConfigureAwait(false);
        }

        protected async Task EscreverAsync(string dados)
        {
            if (EstaAberto() == false)
            {
                await ConectarWriterAndReaderUnshareAsync().ConfigureAwait(false);
                this.Writer = new StreamWriter(FS, EncodingTexto, bufferSize: 4096, leaveOpen: true);
            }

            this.CancelToken.ThrowIfCancellationRequested();
            await this.Writer.WriteAsync(dados).ConfigureAwait(false);
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
