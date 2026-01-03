using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Bibliotecas.TaskCore;
using Etiquetas.Bibliotecas.Xml;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Modelo;

namespace Etiquetas.Domain.Configuracao
{
    /// <summary>
    /// Implementação concreta da configuração de posições de campos em etiquetas,
    /// carregando os valores do arquivo de configuração (appsettings.xml).
    /// </summary>
    public class PosicaoCamposEtiqueta : IPosicaoCamposEtiqueta
    {
        /// <inehritdoc/>
        public IExtracaoSpooler ConfiguracaoSpooler { get; set; }

        /// <inehritdoc/>
        public StreamXml XmlStream { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PosicaoCamposEtiqueta"/> class.
        /// Inicializa uma nova instância da classe <see cref="PosicaoCamposEtiqueta"/>,
        /// carregando todas as configurações do appsettings.xml.
        /// </summary>
        /// <param name="tipoLinguagem">Tipo de linguagem de impressão a ser utilizada</param>
        public PosicaoCamposEtiqueta(EnumTipoLinguagemImpressao tipoLinguagem)
        {
            this.ConfiguracaoSpooler = new ExtracaoSpooler(tipoLinguagem);
        }

        /// <summary>
        /// Carrega todas as configurações do arquivo appsettings.xml.
        /// </summary>
        public async Task CarregarConfiguracoes()
        {
            var parametrosLeitura = new TaskParametros();
            parametrosLeitura.ArmazenaCancellationTokenSource(new CancellationTokenSource());

            // Deserializar de arquivo
            this.ConfiguracaoSpooler = await XmlStream.LerAsync<ExtracaoSpooler>(parametrosLeitura).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"\nCarregados {this.ConfiguracaoSpooler.Campos.Comandos.Count} campos do arquivo.");

            // Construir índice para acesso rápido aos comandos dos campos
            await ConstruirIndice().ConfigureAwait(false);
        }

        /// <summary>
        /// Obtém o comando do campo com base no nome do campo.
        /// </summary>
        /// <param name="nome">Nome do campo a procurar a posicao.</param>
        /// <returns>Retorna a posicao na Coleção de Comandos.</returns>
        public async Task<IComandosCampo> ObterComandoCampoPeloNome(string nome)
        {
            int posicao = await PosicaoNomeDicionario(nome).ConfigureAwait(false);
            return await ObterComandoCampoPorPosicao(posicao).ConfigureAwait(false);
        }

        /// <summary>
        /// Constrói o índice para acesso rápido aos comandos dos campos.
        /// </summary>
        /// <returns>Task async.</returns>
        protected async Task ConstruirIndice()
        {
            // Construir índice para acesso rápido aos comandos dos campos
            var indiceCampos = new ConcurrentDictionary<string, ComandosCampo>(StringComparer.OrdinalIgnoreCase);

            var posicao = -1;

            foreach (var comando in this.ConfiguracaoSpooler.Campos.Comandos)
            {
                posicao++;
                this.ConfiguracaoSpooler.Campos.IndiceComandos.TryAdd(comando.NomeCampo, posicao);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Obtém a posição do campo no dicionário de comandos com base no nome do campo.
        /// </summary>
        /// <param name="nome">Nome do campo a procurar a posicao.</param>
        /// <returns>Retorna a posicao na Coleção de Comandos.</returns>
        protected async Task<int> PosicaoNomeDicionario(string nome)
        {
            if (this.ConfiguracaoSpooler.Campos.IndiceComandos.TryGetValue(nome, out int posicao))
            {
                return await Task.FromResult(posicao).ConfigureAwait(false);
            }
            else
            {
                throw new KeyNotFoundException($"O campo '{nome}' não foi encontrado na configuração de posições de campos da etiqueta.");
            }
        }

        /// <summary>
        /// Obtém o comando do campo com base na posição fornecida.
        /// </summary>
        /// <param name="posicao">Posicao na Coleção de Campos.</param>
        /// <returns>Retorna o Campo selecionado pela posição.</returns>
        protected async Task<IComandosCampo> ObterComandoCampoPorPosicao(int posicao)
        {
            return await Task.FromResult(this.ConfiguracaoSpooler.Campos.Comandos[posicao]).ConfigureAwait(false);
        }
    }
}
