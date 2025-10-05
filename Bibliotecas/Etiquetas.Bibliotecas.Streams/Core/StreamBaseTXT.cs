using Etiquetas.Bibliotecas.Comum.Caracteres;
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

            ParametrosNecessariosConectarAsync(parametros);

            var dir = Path.GetDirectoryName(NomeECaminhoArquivo);

            if (!EhStringNuloVazioComEspacosBranco.Execute(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);


            FS = await Task.Run(() => Task.FromResult(new FileStream(NomeECaminhoArquivo, ModoArquivo, AcessoArquivo, CompartilhamentoArquivo)));
        }

        protected void ParametrosNecessariosConectarAsync(params object[] parametros)
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
                foreach (var item in pendente)
                {
                    var nomeTipo = item.Key.Name;
                    throw new ArgumentException($"Parâmetro obrigatório não fornecido: {nomeTipo}");
                }
            }
        }

        public override Task FecharAsync()
        {
            FS?.Close();
            FS?.Dispose();
            FS = null;
            return Task.CompletedTask;
        }

        public override bool EstaAberto()
        {
            // "Open" is transient, but we can say it's always ready if the path exists.
            var aberto = File.Exists(NomeECaminhoArquivo) && FS != null && (FS.CanRead || FS.CanWrite);

            return aberto;
        }

        public override bool PossuiDados()
        {
            try
            {
                if (File.Exists(NomeECaminhoArquivo) && FS != null && FS.CanRead)
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
