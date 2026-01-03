using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Etiquetas.Application.DTOs;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using Etiquetas.Bibliotecas.Comum.Numericos;
using Etiquetas.Bibliotecas.ControleFilaDados;
using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Configuracao;
using Etiquetas.Domain.Entities;
using Etiquetas.Domain.Modelo;

namespace Etiquetas.Application.Mappers
{
    /// <summary>
    /// Classe de mapeamento entre EtiquetaImpressao e EtiquetaImpressaoDto.
    /// </summary>
    public static class EtiquetaMapper
    {
        /// <summary>
        /// Dicionário estático de campos e seus comandos associados.
        /// </summary>
        public static Dictionary<EnumTipoCampo, IComandosCampo> Campos { get; set; } = new Dictionary<EnumTipoCampo, IComandosCampo>();

        /// <summary>
        /// Creates a new <see cref="IEtiquetaImpressaoDto"/> instance.
        /// Cria uma nova instância de <see cref="IEtiquetaImpressaoDto"/> preenchida com o material especificado e
        /// informações sobre o medicamento.
        /// </summary>
        /// <param name="id">O identificador único do material. Maior do que zero.</param>
        /// <param name="codigoMaterial">A descrição do medicamento. Não pode ser nula.</param>
        /// <param name="codigoBarras">O código de barras associado ao material. Não pode ser nulo.</param>
        /// <param name="descricaoMedicamento">Descrição do medicamento. Não pode ser nulo.</param>
        /// <param name="principioAtivo1">A primeira parte do Nome do Principio Ativo.</param>
        /// <param name="principioAtivo2">A segunda parte do Nome do Principio ativo.</param>
        /// <param name="lote">Numero do lote do medicamento. Não pode ser nulo.</param>
        /// <param name="validade">Data de validade do material. Não pode ser nulo.</param>
        /// <param name="matriculaFuncionario">Matrícula do funcionário que solicitou a impressão. Não pode ser nulo.</param>
        /// <param name="quantidadeSolicitada">Quantidade de etiquetas solicitadas para impressão. Maior do que zero.</param>
        /// <returns>An <see cref="IEtiquetaImpressaoDto"/> Uma instancia contendo EtiquetaImpressaoDto contendo os detalhes do material para impressão da etiqueta.</returns>
        public static IEtiquetaImpressaoDto CriaDTO(
            long id,
            string codigoMaterial,
            string codigoBarras,
            string descricaoMedicamento,
            string principioAtivo1,
            string principioAtivo2,
            string lote,
            string validade,
            string matriculaFuncionario,
            string quantidadeSolicitada)
        {
            var nomeJOb = $"{id}_{DateTime.Now:ddHHmmss}";
            var dto = new EtiquetaImpressaoDto();
            {
                dto.CodigoMaterial = codigoMaterial;
                dto.CodigoBarras = codigoBarras;
                dto.DescricaoMedicamento = descricaoMedicamento;
                dto.PrincipioAtivo = principioAtivo1;
                dto.PrincipioAtivo2 = principioAtivo2;
                dto.Lote = lote;
                dto.Validade = validade;
                dto.DataHoraInicio = DateTime.Now.ToString("o");
                dto.DataHoraFim = DateTime.Now.ToString("o");
                dto.StatusEtiqueta = 'P';
                dto.CodigoUsuario = matriculaFuncionario;
                dto.QuantidadeSolicitada = quantidadeSolicitada;
                dto.FaltaImpressao = quantidadeSolicitada;
            }

            return dto;
        }

        /// <summary>
        /// Extrai os dados da etiqueta baseado na configuração fornecida.
        /// O método é inteligente e detecta automaticamente se o posicionamento
        /// usa comando único ou comandos duplos (H/V ou V/H).
        /// </summary>
        /// <param name="conteudoEtiqueta">Conteúdo completo da etiqueta a ser processada</param>
        /// <param name="configuracao">Configuração dos campos e marcadores</param>
        /// <returns>Objeto com os dados extraídos ou null se houver erro</returns>
        /// <exception cref="ArgumentNullException">Se conteudoEtiqueta ou configuracao forem nulos</exception>
        /// <exception cref="CampoObrigatorioException">Se um campo obrigatório estiver vazio</exception>
        public static IEtiquetaImpressaoDto SpolerToDto(
            this string conteudoEtiqueta,
            IPosicaoCamposEtiqueta configuracao)
        {
            if (string.IsNullOrWhiteSpace(conteudoEtiqueta))
            {
                throw new ArgumentNullException(nameof(conteudoEtiqueta), "O conteúdo da etiqueta não pode ser nulo ou vazio");
            }

            if (configuracao == null)
            {
                throw new ArgumentNullException(nameof(configuracao), "A configuração não pode ser nula");
            }

            try
            {
                // Quebra em linhas/comandos individuais
                var comandos = QuebraComandosEmLinhasIndividuais.Execute(conteudoEtiqueta, configuracao.ConfiguracaoSpooler.ComandosImpressao.TipoLinguagem);

                // Extrai os dados
                var dados = new EtiquetaImpressaoDto();
                var estado = new EstadoCampo();

                foreach (var comando in comandos)
                {
                    if (string.IsNullOrWhiteSpace(comando))
                    {
                        continue;
                    }

                    // Processa o comando atual
                    ProcessarComando(comando, configuracao, dados, estado);
                }

                // Valida campos obrigatórios
                ValidarCamposObrigatorios(dados, configuracao);

                return dados;
            }
            catch (CampoObrigatorioException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao extrair dados da etiqueta: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Processa um comando individual, identificando o campo e extraindo o texto.
        /// </summary>
        /// <param name="comando">Comando a ser processado</param>
        /// <param name="config">Configuração dos campos</param>
        /// <param name="dados">Objeto onde os dados serão armazenados</param>
        /// <param name="estado">Estado atual do processamento</param>
        private static void ProcessarComando(
            string comando,
            IPosicaoCamposEtiqueta config,
            EtiquetaImpressaoDto dados,
            EstadoCampo estado)
        {
            // Normaliza o comando (remove zeros à esquerda em posições)
            var comandoNormalizado = NormalizarComando(comando);

            // Tenta identificar qual campo é este comando
            var campoIdentificado = IdentificarCampo(comandoNormalizado, config, estado);

            if (campoIdentificado != EnumTipoCampo.Nenhum)
            {
                // Verifica se já temos todos os comandos de posição necessários
                var configCampo = ObterConfiguracaoCampo(campoIdentificado, config);

                if (configCampo != null)
                {
                    var precisaCmd2 = !string.IsNullOrWhiteSpace(configCampo.Comando2);

                    if (!precisaCmd2 || (estado.Cmd1Encontrado && estado.Cmd2Encontrado))
                    {
                        // Tenta extrair o texto
                        var texto = ExtrairTextoDoComando(comando, config);

                        if (!string.IsNullOrWhiteSpace(texto))
                        {
                            AtribuirValorAoCampo(campoIdentificado, texto, dados);
                            estado.Reset(); // Reseta para o próximo campo
                        }
                    }
                }
            }
        }

        private static async Task PreencheDicionarioCampos(IPosicaoCamposEtiqueta config)
        {
            // Lista de todos os campos possíveis
            var codigoMaterial = await config.ObterComandoCampoPeloNome("CodigoMaterial").ConfigureAwait(false);
            var descricaoMedicamento = await config.ObterComandoCampoPeloNome("Descricao").ConfigureAwait(false);
            foreach (EnumTipoCampo valor in ObtemValoresEnum)
            {
            }





            {
                { EnumTipoCampo.CodigoMaterial, codigoMaterial },
                { EnumTipoCampo.DescricaoMedicamento, config.DescricaoMedicamento },
                { EnumTipoCampo.DescricaoMedicamento2, config.DescricaoMedicamento2 },
                { EnumTipoCampo.PrincipioAtivo, config.PrincipioAtivo },
                { EnumTipoCampo.PrincipioAtivo2, config.PrincipioAtivo2 },
                { EnumTipoCampo.Embalagem, config.Embalagem },
                { EnumTipoCampo.Lote, config.Lote },
                { EnumTipoCampo.Validade, config.Validade },
                { EnumTipoCampo.CodigoUsuario, config.CodigoUsuario },
                { EnumTipoCampo.CodigoBarras, config.CodigoBarras },
                { EnumTipoCampo.Copias, config.Copias }
            };

        }

        /// <summary>
        /// Identifica qual campo está sendo processado no comando atual.
        /// Atualiza o estado conforme encontra Cmd1 e Cmd2.
        /// </summary>
        /// <param name="comando">Comando normalizado</param>
        /// <param name="config">Configuração dos campos</param>
        /// <param name="estado">Estado atual</param>
        /// <returns>Tipo do campo identificado</returns>
        private static async EnumTipoCampo IdentificarCampo(
            string comando,
            IPosicaoCamposEtiqueta config,
            EstadoCampo estado)
        {
            // Lista de todos os campos possíveis
            var codigoMaterial = await config.ObterComandoCampoPeloNome("").ConfigureAwait(false);


            var campos = new Dictionary<EnumTipoCampo, IComandosCampo>
            {
                { EnumTipoCampo.CodigoMaterial, codigoMaterial },
                { EnumTipoCampo.DescricaoMedicamento, config.DescricaoMedicamento },
                { EnumTipoCampo.DescricaoMedicamento2, config.DescricaoMedicamento2 },
                { EnumTipoCampo.PrincipioAtivo, config.PrincipioAtivo },
                { EnumTipoCampo.PrincipioAtivo2, config.PrincipioAtivo2 },
                { EnumTipoCampo.Embalagem, config.Embalagem },
                { EnumTipoCampo.Lote, config.Lote },
                { EnumTipoCampo.Validade, config.Validade },
                { EnumTipoCampo.CodigoUsuario, config.CodigoUsuario },
                { EnumTipoCampo.CodigoBarras, config.CodigoBarras },
                { EnumTipoCampo.Copias, config.Copias }
            };

            foreach (var kvp in campos)
            {
                var tipo = kvp.Key;
                var configCampo = kvp.Value;

                if (configCampo == null || string.IsNullOrWhiteSpace(configCampo.Comando1))
                {
                    continue;
                }

                // Normaliza os comandos de configuração também
                var cmd1Normalizado = NormalizarComando(configCampo.Comando1);
                var cmd2Normalizado = string.IsNullOrWhiteSpace(configCampo.Comando2)
                    ? ""
                    : NormalizarComando(configCampo.Comando2);

                // Verifica Cmd1
                if (comando.StartsWith(cmd1Normalizado, StringComparison.OrdinalIgnoreCase))
                {
                    if (estado.Tipo == tipo)
                    {
                        estado.Cmd1Encontrado = true;
                    }
                    else
                    {
                        estado.Reset();
                        estado.Tipo = tipo;
                        estado.Cmd1Encontrado = true;
                    }

                    return tipo;
                }

                // Verifica Cmd2 (se existir)
                if (!string.IsNullOrWhiteSpace(cmd2Normalizado) &&
                    comando.StartsWith(cmd2Normalizado, StringComparison.OrdinalIgnoreCase))
                {
                    if (estado.Tipo == tipo)
                    {
                        estado.Cmd2Encontrado = true;
                    }
                    else
                    {
                        estado.Reset();
                        estado.Tipo = tipo;
                        estado.Cmd2Encontrado = true;
                    }

                    return tipo;
                }
            }

            return EnumTipoCampo.Nenhum;
        }

        /// <summary>
        /// Extrai o texto de um comando baseado nos marcadores de início e fim.
        /// </summary>
        /// <param name="comando">Comando contendo o texto</param>
        /// <param name="config">Configuração com os marcadores</param>
        /// <returns>Texto extraído ou string vazia</returns>
        private static string ExtrairTextoDoComando(string comando, IPosicaoCamposEtiqueta config)
        {
            var inicio = config.MarcadorInicialTexto;
            var fim = config.MarcadorFinalTexto;

            if (string.IsNullOrEmpty(inicio) && string.IsNullOrEmpty(fim))
            {
                // SBPL: texto vem após o comando de posição
                // Extrai tudo após os primeiros 5 caracteres (ex: ESC + H0010)
                if (comando.Length > 5)
                {
                    return comando.Substring(5).Trim();
                }

                return string.Empty;
            }

            int idxInicio = -1;
            int idxFim = -1;

            if (!string.IsNullOrEmpty(inicio))
            {
                idxInicio = comando.IndexOf(inicio, StringComparison.Ordinal);
                if (idxInicio >= 0)
                {
                    idxInicio += inicio.Length;
                }
            }
            else
            {
                idxInicio = 0;
            }

            if (idxInicio < 0 || idxInicio >= comando.Length)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(fim))
            {
                idxFim = comando.IndexOf(fim, idxInicio, StringComparison.Ordinal);
            }
            else
            {
                idxFim = comando.Length;
            }

            if (idxFim < 0)
            {
                idxFim = comando.Length;
            }

            var tamanho = idxFim - idxInicio;
            if (tamanho <= 0)
            {
                return "";
            }

            return comando.Substring(idxInicio, tamanho).Trim();
        }

        /// <summary>
        /// Obtém a configuração de um campo específico.
        /// </summary>
        /// <param name="tipo">Tipo do campo</param>
        /// <param name="config">Configuração completa</param>
        /// <returns>Configuração do campo ou null</returns>
        private static IConfiguracaoCampo ObterConfiguracaoCampo(EnumTipoCampo tipo, IPosicaoCamposEtiqueta config)
        {
            switch (tipo)
            {
                case EnumTipoCampo.CodigoMaterial: return config.CodigoMaterial;
                case EnumTipoCampo.DescricaoMedicamento: return config.DescricaoMedicamento;
                case EnumTipoCampo.DescricaoMedicamento2: return config.DescricaoMedicamento2;
                case EnumTipoCampo.PrincipioAtivo: return config.PrincipioAtivo;
                case EnumTipoCampo.PrincipioAtivo2: return config.PrincipioAtivo2;
                case EnumTipoCampo.Embalagem: return config.Embalagem;
                case EnumTipoCampo.Lote: return config.Lote;
                case EnumTipoCampo.Validade: return config.Validade;
                case EnumTipoCampo.CodigoUsuario: return config.CodigoUsuario;
                case EnumTipoCampo.CodigoBarras: return config.CodigoBarras;
                case EnumTipoCampo.Copias: return config.Copias;
                default: return null;
            }
        }

        /// <summary>
        /// Atribui o valor extraído ao campo correspondente no DTO.
        /// </summary>
        /// <param name="tipo">Tipo do campo</param>
        /// <param name="valor">Valor a ser atribuído</param>
        /// <param name="dados">Objeto DTO</param>
        private static void AtribuirValorAoCampo(EnumTipoCampo tipo, string valor, EtiquetaImpressaoDto dados)
        {
            switch (tipo)
            {
                case EnumTipoCampo.CodigoMaterial:
                    dados.CodigoMaterial = Etiquetas.Bibliotecas.Comum.Numericos.StringExtrairSomenteDigitosNumericos.Execute(valor);
                    break;
                case EnumTipoCampo.DescricaoMedicamento:
                    dados.DescricaoMedicamento = valor;
                    break;
                case EnumTipoCampo.DescricaoMedicamento2:
                    dados.DescricaoMedicamento2 = valor;
                    break;
                case EnumTipoCampo.PrincipioAtivo:
                    dados.PrincipioAtivo = valor;
                    break;
                case EnumTipoCampo.PrincipioAtivo2:
                    dados.PrincipioAtivo2 = valor;
                    break;
                case EnumTipoCampo.Embalagem:
                    dados.Embalagem = valor;
                    break;
                case EnumTipoCampo.Lote:
                    dados.Lote = valor;
                    break;
                case EnumTipoCampo.Validade:
                    dados.Validade = valor;
                    break;
                case EnumTipoCampo.CodigoUsuario:
                    dados.CodigoUsuario = valor;
                    break;
                case EnumTipoCampo.CodigoBarras:
                    dados.CodigoBarras = ProcessarCodigoBarras(valor);
                    break;
                case EnumTipoCampo.Copias:
                    dados.QuantidadeSolicitada = valor;
                    break;
            }
        }

        /// <summary>
        /// Processa código de barras, calculando dígito verificador EAN-13 se necessário.
        /// </summary>
        /// <param name="codigo">Código extraído</param>
        /// <returns>Código completo com dígito verificador.</returns>
        private static string ProcessarCodigoBarras(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return string.Empty;
            }

            var apenasDigitos = StringExtrairSomenteDigitosNumericos.Execute(codigo);

            // Se tem 12 dígitos, calcula o 13º (EAN-13)
            if (apenasDigitos.Length == 12)
            {
                var digito = EAN13.CalcularDigitoVerificador(apenasDigitos);
                return apenasDigitos + digito;
            }

            return apenasDigitos;
        }

        /// <summary>
        /// Valida se todos os campos obrigatórios foram preenchidos.
        /// </summary>
        /// <param name="dados">Dados extraídos</param>
        /// <param name="config">Configuração com indicação de obrigatoriedade</param>
        /// <exception cref="CampoObrigatorioException">Se algum campo obrigatório estiver vazio</exception>
        private static void ValidarCamposObrigatorios(IEtiquetaImpressaoDto dados, IPosicaoCamposEtiqueta config)
        {
            ValidarCampo(config.CodigoMaterial, dados.CodigoMaterial, "Código do Material");
            ValidarCampo(config.DescricaoMedicamento, dados.DescricaoMedicamento, "Descrição do Medicamento");
            ValidarCampo(config.DescricaoMedicamento2, dados.DescricaoMedicamento2, "Descrição do Medicamento 2");
            ValidarCampo(config.PrincipioAtivo, dados.PrincipioAtivo, "Princípio Ativo 1");
            ValidarCampo(config.PrincipioAtivo2, dados.PrincipioAtivo2, "Princípio Ativo 2");
            ValidarCampo(config.Embalagem, dados.Embalagem, "Embalagem");
            ValidarCampo(config.Lote, dados.Lote, "Lote");
            ValidarCampo(config.Validade, dados.Validade, "Validade");
            ValidarCampo(config.CodigoUsuario, dados.CodigoUsuario, "Código do Usuário");
            ValidarCampo(config.CodigoBarras, dados.CodigoBarras, "Código de Barras");
        }

        /// <summary>
        /// Valida um campo individual.
        /// </summary>
        /// <param name="configCampo">Configuração do campo</param>
        /// <param name="valor">Valor extraído</param>
        /// <param name="nomeCampo">Nome do campo para mensagem de erro</param>
        private static void ValidarCampo(IConfiguracaoCampo configCampo, string valor, string nomeCampo)
        {
            if (configCampo != null && configCampo.Obrigatorio && string.IsNullOrWhiteSpace(valor))
            {
                throw new CampoObrigatorioException($"O campo '{nomeCampo}' é obrigatório e não foi encontrado na etiqueta.");
            }
        }

        /// <summary>
        /// Normaliza o comando removendo zeros à esquerda de posições numéricas.
        /// Exemplo: "^FO0010,0005" vira "^FO10,5"
        /// </summary>
        /// <param name="comando">Comando original</param>
        /// <returns>Comando normalizado</returns>
        private static string NormalizarComando(string comando)
        {
            if (string.IsNullOrWhiteSpace(comando))
            {
                return comando;
            }

            // Detecta padrões numéricos e remove zeros à esquerda
            var sb = new StringBuilder();
            var dentroDeNumero = false;
            var zerosAcumulados = 0;

            for (int i = 0; i < comando.Length; i++)
            {
                var c = comando[i];

                if (char.IsDigit(c))
                {
                    if (!dentroDeNumero)
                    {
                        dentroDeNumero = true;
                        zerosAcumulados = 0;
                    }

                    if (c == '0' && zerosAcumulados >= 0)
                    {
                        zerosAcumulados++;
                    }
                    else
                    {
                        // Encontrou dígito diferente de zero
                        if (zerosAcumulados > 0)
                        {
                            // Se era apenas zeros, mantém um
                            if (i + 1 >= comando.Length || !char.IsDigit(comando[i + 1]))
                            {
                                sb.Append('0');
                            }
                        }

                        zerosAcumulados = -1; // Desativa contagem
                        sb.Append(c);
                    }
                }
                else
                {
                    if (dentroDeNumero && zerosAcumulados > 0)
                    {
                        // Terminou número só com zeros, mantém um
                        sb.Append('0');
                    }

                    dentroDeNumero = false;
                    zerosAcumulados = 0;
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converte um DTO de EtiquetaImpressao para a entidade EtiquetaImpressao.
        /// </summary>
        /// <param name="dto">recebe DTO Etiqueta impressao.</param>
        /// <returns>Retorna entidade EtiquetaImpressao.</returns>
        public static IEtiquetaImpressao ToEntity(this IEtiquetaImpressaoDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            long codigoMaterial = 0;
            long.TryParse(dto.CodigoMaterial, out codigoMaterial);
            //DateTime validade = DateTime.MinValue;
            if (DateTime.TryParse(dto.Validade, out var validade))
            {

            }

            DateTime inicio = DateTime.MinValue;
            DateTime.TryParse(dto.DataHoraInicio, out inicio);
            DateTime fim = DateTime.MinValue;
            DateTime.TryParse(dto.DataHoraFim, out fim);
            long.TryParse(dto.QuantidadeSolicitada, out long quantidadeSolicitada);
            long.TryParse(dto.FaltaImpressao, out long faltaImpressao);

            return new EtiquetaImpressao
            {
                Id = dto.Id,
                DescricaoMedicamento = dto.DescricaoMedicamento,
                DescricaoMedicamento2 = dto.DescricaoMedicamento2,
                PrincipioAtivo = dto.PrincipioAtivo,
                PrincipioAtivo2 = dto.PrincipioAtivo2,
                CodigoMaterial = codigoMaterial,
                CodigoBarras = dto.CodigoBarras,
                Embalagem = dto.Embalagem,
                Lote = dto.Lote,
                Validade = validade,
                DataHoraInicio = inicio,
                DataHoraFim = fim,
                StatusEtiqueta = dto.StatusEtiqueta,
                QuantidadeSolicitada = quantidadeSolicitada,
                FaltaImpressao = faltaImpressao,
            };
        }

        /// <summary>
        /// Converte uma entidade <see cref="EtiquetaImpressao"/> em seu objeto de transferência de dados <see cref="EtiquetaImpressaoDto"/> correspondente.
        /// </summary>
        /// <param name="ent">A instância de <see cref="EtiquetaImpressao"/> a ser convertida. Pode ser <see langword="null"/>.</param>
        /// <returns>Um <see cref="EtiquetaImpressaoDto"/> representando os dados de <paramref name="ent"/>, ou <see
        /// langword="null"/> se <paramref name="ent"/> for <see langword="null"/>.</returns>
        public static EtiquetaImpressaoDto ToDto(this EtiquetaImpressao ent)
        {
            if (ent == null)
            {
                return null;
            }

            return new EtiquetaImpressaoDto
            {
                Id = ent.Id,
                DescricaoMedicamento = ent.DescricaoMedicamento,
                DescricaoMedicamento2 = ent.DescricaoMedicamento2,
                PrincipioAtivo = ent.PrincipioAtivo,
                PrincipioAtivo2 = ent.PrincipioAtivo2,
                CodigoMaterial = ent.CodigoMaterial.ToString(),
                CodigoBarras = ent.CodigoBarras,
                Lote = ent.Lote,
                Embalagem = ent.Embalagem,
                Validade = ent.Validade.ToString("o"),
                DataHoraInicio = ent.DataHoraInicio.ToString("o"),
                DataHoraFim = ent.DataHoraFim.ToString("o"),
                StatusEtiqueta = ent.StatusEtiqueta,
                QuantidadeSolicitada = ent.QuantidadeSolicitada.ToString(),
                FaltaImpressao = ent.FaltaImpressao.ToString(),
            };
        }
    }
}
