using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Etiquetas.Application.DTOs;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using Etiquetas.Bibliotecas.ControleFilaDados;
using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Entities;

namespace Etiqueta.Application.Mappers
{
    /// <summary>
    /// Classe de mapeamento entre EtiquetaImpressao e EtiquetaImpressaoDto.
    /// </summary>
    public static class EtiquetaMapper
    {
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
        public static IEtiquetaImpressaoDto SpolerToDto(this string conteudoEtiqueta, IPosicaoCamposEtiqueta configuracao)
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
                // Prepara o conteúdo baseado no tipo de linguagem
                var conteudoProcessado = PreprocessarConteudo(conteudoEtiqueta, configuracao.TipoLinguagem);

                // Quebra em linhas/comandos individuais
                var comandos = QuebrarEmComandos(conteudoProcessado, configuracao.TipoLinguagem);

                // Extrai os dados
                var dados = new EtiquetaImpressaoDto();
                var estado = new EstadoCampo();

                foreach (var comando in comandos)
                {
                    if (string.IsNullOrWhiteSpace(comando))
                        continue;

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
