using System;
using System.Collections.Generic;
using System.Linq;
using Etiquetas.Application.DTOs;
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
            long quantidadeSolicitada)
        {
            var nomeJOb = $"{id}_{DateTime.Now:ddHHmmss}";
            var dto = new EtiquetaImpressaoDto();
            {
                dto.CodigoMaterial = codigoMaterial;
                dto.CodigoBarras = codigoBarras;
                dto.DescricaoMedicamento = descricaoMedicamento;
                dto.PrincipioAtivo1 = principioAtivo1;
                dto.PrincipioAtivo2 = principioAtivo2;
                dto.Lote = lote;
                dto.Validade = validade;
                dto.DataHoraInicio = DateTime.Now.ToString("o");
                dto.DataHoraFim = DateTime.Now.ToString("o");
                dto.StatusEtiqueta = 'P';
                dto.MatriculaFuncionario = matriculaFuncionario;
                dto.QuantidadeSolicitada = quantidadeSolicitada;
                dto.FaltaImpressao = quantidadeSolicitada;
            }

            return dto;
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
            DateTime validade = DateTime.MinValue;
            DateTime.TryParse(dto.Validade, out validade);
            DateTime inicio = DateTime.MinValue;
            DateTime.TryParse(dto.DataHoraInicio, out inicio);
            DateTime fim = DateTime.MinValue;
            DateTime.TryParse(dto.DataHoraFim, out fim);

            return new EtiquetaImpressao
            {
                Id = dto.Id,
                DescricaoMedicamento = dto.DescricaoMedicamento,
                PrincipioAtivo1 = dto.PrincipioAtivo1,
                PrincipioAtivo2 = dto.PrincipioAtivo2,
                CodigoMaterial = codigoMaterial,
                CodigoBarras = dto.CodigoBarras,
                Lote = dto.Lote,
                Validade = validade,
                DataHoraInicio = inicio,
                DataHoraFim = fim,
                StatusEtiqueta = dto.StatusEtiqueta,
                QuantidadeSolicitada = dto.QuantidadeSolicitada,
                FaltaImpressao = dto.FaltaImpressao,
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
                PrincipioAtivo1 = ent.PrincipioAtivo1,
                PrincipioAtivo2 = ent.PrincipioAtivo2,
                CodigoMaterial = ent.CodigoMaterial.ToString(),
                CodigoBarras = ent.CodigoBarras,
                Lote = ent.Lote,
                Validade = ent.Validade.ToString("o"),
                DataHoraInicio = ent.DataHoraInicio.ToString("o"),
                DataHoraFim = ent.DataHoraFim.ToString("o"),
                StatusEtiqueta = ent.StatusEtiqueta,
                QuantidadeSolicitada = ent.QuantidadeSolicitada,
                FaltaImpressao = ent.FaltaImpressao,
            };
        }
    }
}
