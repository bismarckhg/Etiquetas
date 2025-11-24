using Etiquetas.Bibliotecas.Comum.Caracteres;
using Etiquetas.Bibliotecas.TaskCore.Interfaces;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Streams.Core
{
    public class StreamBaseTXT : StreamBase
    {
        protected FileStream FS { get; set; }

        protected string NomeECaminhoArquivo { get; set; }
        protected FileMode ModoArquivo { get; set; }
        protected FileAccess AcessoArquivo { get; set; }
        protected FileShare CompartilhamentoArquivo { get; set; }

        protected async Task ConectarAsync()
        {

            var dir = Path.GetDirectoryName(NomeECaminhoArquivo);

            if (!EhStringNuloVazioComEspacosBranco.Execute(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if ((ModoArquivo == FileMode.Open || ModoArquivo == FileMode.Truncate) &&
                (AcessoArquivo == FileAccess.Read) &&
                !File.Exists(NomeECaminhoArquivo))
            {
                throw new FileNotFoundException("Arquivo não encontrado.", NomeECaminhoArquivo);
            }

            FS = await Task.Run(() => new FileStream(NomeECaminhoArquivo, ModoArquivo, AcessoArquivo, CompartilhamentoArquivo));
        }

        public override async Task ConectarReaderOnlyUnshareAsync()
        {
            if (EstaAberto())
            {
                return;
            }

            var dir = Path.GetDirectoryName(NomeECaminhoArquivo);

            if (!EhStringNuloVazioComEspacosBranco.Execute(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (!File.Exists(NomeECaminhoArquivo))
            {
                throw new FileNotFoundException("Arquivo não encontrado.", NomeECaminhoArquivo);
            }

            FS = await Task.Run(() => File.OpenRead(NomeECaminhoArquivo));
        }

        public override async Task ConectarWriterAndReaderUnshareAsync()
        {
            if (EstaAberto())
            {
                return;
            }

            var dir = Path.GetDirectoryName(NomeECaminhoArquivo);
            if (!EhStringNuloVazioComEspacosBranco.Execute(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            // FS = await Task.Run(() => File.OpenWrite(NomeECaminhoArquivo));
            // FS = await Task.Run(() => File.Open(NomeECaminhoArquivo, FileMode.Create, FileAccess.Write, FileShare.None));
            FS = await Task.Run(() => File.Create(NomeECaminhoArquivo));
        }

        public override async Task ConectarAsync(ITaskParametros parametros)
        {
            ThrowIfDisposed();

            if (parametros == null)
            {
                throw new ArgumentNullException("Parâmetros inválidos!");
            }
            this.NomeECaminhoArquivo = (string)(parametros["NomeCaminhoArquivo"] ?? throw new ArgumentNullException("Nome arquivo Vazio ou nulo!"));
            this.ModoArquivo = parametros.RetornaSeExistir<FileMode>("ModoArquivo");
            this.AcessoArquivo = parametros.RetornaSeExistir<FileAccess>("AcessoArquivo");
            this.CompartilhamentoArquivo = parametros.RetornaSeExistir<FileShare>("CompartilhamentoArquivo");
            if (this.ModoArquivo == 0 || this.AcessoArquivo == 0)
            {
                return;
            }
            await ConectarAsync().ConfigureAwait(false);
        }

        public override async Task FecharAsync()
        {
            await Task.Run(() =>
            {
                FS?.Close();
                FS?.Dispose();
                FS = null;
            }).ConfigureAwait(false);
        }

        public override bool EstaAberto()
        {
            // "Open" is transient, but we can say it's always ready if the path exists.
            var aberto = FS != null && ((FS.CanRead && File.Exists(NomeECaminhoArquivo)) || FS.CanWrite);

            return aberto;
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
            if (disposing)
            {
                if (FS == null) return;

                if (EstaAberto())
                {
                    FS?.Close();
                }
                FS?.Dispose();
                FS = null;
            }
        }
    }
}
