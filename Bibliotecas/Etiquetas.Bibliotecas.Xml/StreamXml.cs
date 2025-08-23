using Etiquetas.Bibliotecas.Streams.Core;
using Etiquetas.Bibliotecas.Streams.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Etiquetas.Bibliotecas.Xml
{
    /// <summary>
    /// Fornece uma implementação de stream para leitura e escrita de arquivos XML.
    /// </summary>
    public class StreamXml : StreamStreamBase, IStreamLeitura<XDocument>, IStreamEscrita<XDocument>
    {
        private readonly string _caminhoArquivo;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="StreamXml"/>.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho completo para o arquivo XML.</param>
        public StreamXml(string caminhoArquivo)
        {
            if (string.IsNullOrEmpty(caminhoArquivo))
                throw new ArgumentNullException(nameof(caminhoArquivo));
            _caminhoArquivo = caminhoArquivo;
        }

        /// <summary>
        /// As conexões são tratadas por operação, então este método não realiza nenhuma ação.
        /// </summary>
        public override Task ConectarAsync(params object[] parametros) => Task.CompletedTask;

        /// <summary>
        /// O fechamento é tratado por operação, então este método não realiza nenhuma ação.
        /// </summary>
        public override Task FecharAsync() => Task.CompletedTask;

        /// <summary>
        /// Lê o conteúdo do arquivo XML de forma assíncrona e o retorna como um <see cref="XDocument"/>.
        /// </summary>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        /// <returns>Um <see cref="XDocument"/> com o conteúdo do arquivo, ou null se o arquivo não existir.</returns>
        public async Task<XDocument> LerAsync(params object[] parametros)
        {
            if (!File.Exists(_caminhoArquivo))
            {
                return null;
            }

            return await Task.Run(() => XDocument.Load(_caminhoArquivo, LoadOptions.None)).ConfigureAwait(false);
        }

        /// <summary>
        /// Escreve o <see cref="XDocument"/> fornecido para o arquivo de forma assíncrona.
        /// Se o arquivo já existir, ele será sobrescrito.
        /// </summary>
        /// <param name="dados">O <see cref="XDocument"/> a ser salvo.</param>
        /// <param name="parametros">Parâmetros adicionais (não utilizados nesta implementação).</param>
        public async Task EscreverAsync(XDocument dados, params object[] parametros)
        {
            if (dados == null)
                throw new ArgumentNullException(nameof(dados));

            await Task.Run(() => dados.Save(_caminhoArquivo, SaveOptions.None)).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifica se o arquivo XML existe no caminho especificado.
        /// </summary>
        /// <returns>Verdadeiro se o arquivo existir, caso contrário, falso.</returns>
        public override bool EstaAberto()
        {
            return File.Exists(_caminhoArquivo);
        }

        /// <summary>
        /// Verifica se o arquivo XML existe e não está vazio.
        /// </summary>
        /// <returns>Verdadeiro se o arquivo existir e tiver um tamanho maior que zero, caso contrário, falso.</returns>
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

        /// <summary>
        /// Libera os recursos utilizados pela classe.
        /// </summary>
        /// <param name="disposing">Indica se a liberação está sendo feita de forma explícita.</param>
        protected override void Dispose(bool disposing)
        {
            // Nenhuma recurso não gerenciado para liberar neste modelo.
            base.Dispose(disposing);
        }
    }
}
