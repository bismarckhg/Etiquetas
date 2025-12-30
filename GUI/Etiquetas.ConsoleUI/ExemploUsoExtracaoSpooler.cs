using System;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
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
            var config = new ExtracaoSpooler();

            // Configurar ZPL
            config.ZPL.MarcadorInicioTexto = "^FD";
            config.ZPL.MarcadorFimTexto = "^FS";
            config.ZPL.ComandoPosicao = "^FO";
            config.ZPL.ComandoCopias = "^PQ";
            config.ZPL.ComandoBarras = "^BCN";

            // Configurar SBPL
            config.SBPL.SBPL_MarcadorESC = "<ESC>";
            config.SBPL.SBPL_ComandoHorizontal = "H";
            config.SBPL.SBPL_ComandoVertical = "V";
            config.SBPL.SBPL_MarcadorInicioTexto = string.Empty;
            config.SBPL.SBPL_MarcadorFimTexto = string.Empty;

            // Configurar EPL
            config.EPL.EPL_ComandoTexto = "A";
            config.EPL.EPL_ComandoBarras = "B";
            config.EPL.EPL_MarcadorInicioTexto = "\"";
            config.EPL.EPL_MarcadorFimTexto = "\"";

            // Adicionar campos
            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "CodigoMaterial",
                ComandoPosicao1 = "^FO12,68",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = true
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "DescricaoMedicamento",
                ComandoPosicao1 = "^FO12,23",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = true
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "DescricaoMedicamento2",
                ComandoPosicao1 = "^FO12,36",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Campo_PrincipioAtivo",
                ComandoPosicao1 = "^FO12,39",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Campo_PrincipioAtivo2",
                ComandoPosicao1 = "^FO12,52",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Lote",
                ComandoPosicao1 = "^FO12,85",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Validade",
                ComandoPosicao1 = "^FO135,68",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "CodigoUsuario",
                ComandoPosicao1 = "^FO170,85",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "CodigoBarras",
                ComandoPosicao1 = "^FO025,100",
                ComandoPosicao2 = string.Empty,
                Obrigatorio = false
            });

            config.Campos.Comandos.Add(new ComandosCampo
            {
                NomeCampo = "Copias",
                ComandoPosicao1 = "^PQ",
                ComandoPosicao2 = string.Empty,
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
            Console.WriteLine("\n=== CONFIGURAÇÕES ZPL ===");
            Console.WriteLine($"Marcador Início: {configCarregada.ZPL.MarcadorInicioTexto}");
            Console.WriteLine($"Marcador Fim: {configCarregada.ZPL.MarcadorFimTexto}");
            Console.WriteLine($"Comando Posição: {configCarregada.ZPL.ComandoPosicao}");

            Console.WriteLine("\n=== CONFIGURAÇÕES SBPL ===");
            Console.WriteLine($"Marcador ESC: {configCarregada.SBPL.SBPL_MarcadorESC}");
            Console.WriteLine($"Comando Horizontal: {configCarregada.SBPL.SBPL_ComandoHorizontal}");
            Console.WriteLine($"Comando Vertical: {configCarregada.SBPL.SBPL_ComandoVertical}");

            Console.WriteLine("\n=== CONFIGURAÇÕES EPL ===");
            Console.WriteLine($"Comando Texto: {configCarregada.EPL.EPL_ComandoTexto}");
            Console.WriteLine($"Comando Barras: {configCarregada.EPL.EPL_ComandoBarras}");

            Console.WriteLine("\n=== CAMPOS ===");
            foreach (var campo in configCarregada.Campos.Comandos)
            {
                Console.WriteLine($"\nCampo: {campo.NomeCampo}");
                Console.WriteLine($"  Obrigatório: {campo.Obrigatorio}");
                Console.WriteLine($"  Posição 1: {campo.ComandoPosicao1}");
                if (!string.IsNullOrEmpty(campo.ComandoPosicao2))
                {
                    Console.WriteLine($"  Posição 2: {campo.ComandoPosicao2}");
                }
            }
        }
    }
}
