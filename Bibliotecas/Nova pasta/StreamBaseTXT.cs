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

        public override async Task ConectarAsync(params object[] parametros)
        {
            if (stDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            var pendente = await Task.Run(() => ArrayObjectosParametrosConectarAsync(parametros));

            if (pendente == 0)
            {
                await ConectarAsync().ConfigureAwait(false);
            }
        }

        protected int ArrayObjectosParametrosConectarAsync(params object[] parametros)
        {
            if (parametros == null || parametros.Length == 0)
                throw new ArgumentNullException("Parâmetros inválidos!");

            var pendente = new ConcurrentDictionary<Type, int>
            {
                [typeof(string)] = 1,
                [typeof(FileMode)] = 2,
                [typeof(FileAccess)] = 3,
                [typeof(FileShare)] = 4
            };

            var lista = new ConcurrentDictionary<Type, int>();

            foreach (var item in parametros)
            {
                var ok = false;
                var posicao = 0;
                switch (item)
                {
                    case string arquivo:
                        NomeECaminhoArquivo = EhStringNuloVazioComEspacosBranco.Execute(arquivo)
                            ? throw new ArgumentNullException("Nome arquivo Vazio ou nulo!")
                            : arquivo;
                        NomeECaminhoArquivo = arquivo;
                        ok = pendente.TryRemove(arquivo.GetType(), out posicao)
                            ? lista.TryAdd(arquivo.GetType(), posicao)
                            : throw new ArgumentException("Nome de Arquivo Duplicado!!");
                        break;
                    case FileMode modo:
                        ModoArquivo = modo;
                        ok = pendente.TryRemove(modo.GetType(), out posicao)
                            ? lista.TryAdd(modo.GetType(), posicao)
                            : throw new ArgumentException("Parametro FileMode Duplicado!!");
                        break;
                    case FileAccess acesso:
                        AcessoArquivo = acesso;
                        ok = pendente.TryRemove(acesso.GetType(), out posicao)
                            ? lista.TryAdd(acesso.GetType(), posicao)
                            : throw new ArgumentException("Parametro FileAccess Duplicado!!");
                        break;
                    case FileShare compartilhamento:
                        CompartilhamentoArquivo = compartilhamento;
                        ok = pendente.TryRemove(compartilhamento.GetType(), out posicao)
                            ? lista.TryAdd(compartilhamento.GetType(), posicao)
                            : throw new ArgumentException("Parametro FileShare Duplicado!!");
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

            return pendente.Count;
        }

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
            FS = await Task.Run(() => File.OpenWrite(NomeECaminhoArquivo));
        }

        public override async Task ConectarAsync(ITaskParametros parametros)
        {
            if (stDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }

            if (parametros == null)
            {
                throw new ArgumentNullException("Parâmetros inválidos!");
            }

            this.NomeECaminhoArquivo = (string)(parametros["NomeCaminhoArquivo"] ?? throw new ArgumentNullException("Nome arquivo Vazio ou nulo!"));

            if (parametros["ModoArquivo"] != null)
            {
                this.ModoArquivo = (FileMode)(parametros["ModoArquivo"]);
            }

            if (parametros["AcessoArquivo"] != null)
            {
                this.AcessoArquivo = (FileAccess)(parametros["AcessoArquivo"]);
            }

            if (parametros["CompartilhamentoArquivo"] != null)
            {
                this.CompartilhamentoArquivo = (FileShare)(parametros["CompartilhamentoArquivo"]);
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
                if (FS != null && FS.CanRead && File.Exists(NomeECaminhoArquivo))
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
