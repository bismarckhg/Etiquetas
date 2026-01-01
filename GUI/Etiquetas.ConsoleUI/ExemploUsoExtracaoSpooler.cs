using System;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Bibliotecas.TaskCore;
using Etiquetas.Bibliotecas.Xml;
using Etiquetas.Domain.Modelo;

namespace Etiquetas.ConsoleUI
{
    public static class ExemploUsoExtracaoSpooler
    {
        private static StreamXml XmlStream;

        /// <summary>
        /// Ponto de entrada principal para o exemplo de uso da classe <see cref="ExtracaoSpooler"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task Exemplo()
        {
            XmlStream = new StreamXml();

            // Criar configuração completa
            var config = new ExtracaoSpooler(EnumTipoLinguagemImpressao.ZPL);

            // Configurar ZPL
            config.ComandosImpressao.MarcadorComando = "^";
            config.ComandosImpressao.MarcadorInicioTexto = "FD";
            config.ComandosImpressao.MarcadorFimTexto = "FS";
            config.ComandosImpressao.ComandoPosicao1 = "FO";
            config.ComandosImpressao.ComandoCopias = "PQ";
            config.ComandosImpressao.ComandoBarras = "B";

            // Adicionar campos
            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "CodigoMaterial",
                PosicaoComando1 = "12,68",
                PosicaoComando2 = string.Empty,
                Obrigatorio = true
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "DescricaoMedicamento",
                PosicaoComando1 = "12,23",
                PosicaoComando2 = string.Empty,
                Obrigatorio = true
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "DescricaoMedicamento2",
                PosicaoComando1 = "12,36",
                PosicaoComando2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Campo_PrincipioAtivo",
                PosicaoComando1 = "12,39",
                PosicaoComando2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Campo_PrincipioAtivo2",
                PosicaoComando1 = "12,52",
                PosicaoComando2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Lote",
                PosicaoComando1 = "12,85",
                PosicaoComando2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Validade",
                PosicaoComando1 = "135,68",
                PosicaoComando2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "CodigoUsuario",
                PosicaoComando1 = "170,85",
                PosicaoComando2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "CodigoBarras",
                ComandoEspecifico = config.ComandosImpressao.ComandoBarras,
                PosicaoComando1 = "25,100",
                PosicaoComando2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Copias",
                ComandoEspecifico = config.ComandosImpressao.ComandoCopias,
                PosicaoComando1 = string.Empty,
                PosicaoComando2 = string.Empty,
                Obrigatorio = false
            });

            // Serializar para arquivo
            string caminhoArquivo = @"extracao_spooler.xml";
            var parametros = new TaskParametros();
            parametros.Armazena<string>(caminhoArquivo, "NomeCaminhoArquivo");

            Console.WriteLine($"Conectado ao arquivo XML:{caminhoArquivo}");
            await XmlStream.ConectarAsync(parametros).ConfigureAwait(false);
            Console.WriteLine($"Serializando dados da loja.");

            var novoParametros = new TaskParametros();
            novoParametros.Armazena<ExtracaoSpooler>(config, "Objeto");
            novoParametros.ArmazenaCancellationTokenSource(new CancellationTokenSource());

            await XmlStream.EscreverAsync<ExtracaoSpooler>(novoParametros).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"Dados serializados com sucesso em {caminhoArquivo}.");
            Console.WriteLine();

            var parametrosLeitura = new TaskParametros();
            parametrosLeitura.ArmazenaCancellationTokenSource(new CancellationTokenSource());

            // Deserializar de arquivo
            var configCarregada = await XmlStream.LerAsync<ExtracaoSpooler>(parametrosLeitura).ConfigureAwait(false);
            await XmlStream.FecharAsync().ConfigureAwait(false);
            Console.WriteLine($"\nCarregados {configCarregada.Campos.Comandos.Count} campos do arquivo.");

            // Exibir dados carregados
            Console.WriteLine($"\n=== CONFIGURAÇÕES: {configCarregada.ComandosImpressao.TipoLinguagem.ToString()} ===");
            Console.WriteLine($"Marcador Início: {configCarregada.ComandosImpressao.MarcadorInicioTexto}");
            Console.WriteLine($"Marcador Fim: {configCarregada.ComandosImpressao.MarcadorFimTexto}");
            Console.WriteLine($"Comando Posição: {configCarregada.ComandosImpressao.ComandoPosicao1}");

            Console.WriteLine("\n=== CAMPOS ===");
            foreach (var campo in configCarregada.Campos.Comandos)
            {
                Console.WriteLine($"\nCampo: {campo.NomeCampo}");
                Console.WriteLine($"  Obrigatório: {campo.Obrigatorio}");
                Console.WriteLine($"  Posição 1: {campo.PosicaoComando1}");
                if (!string.IsNullOrEmpty(campo.PosicaoComando2))
                {
                    Console.WriteLine($"  Posição 2: {campo.PosicaoComando2}");
                }
            }
        }
    }
}
