using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Mappers
{
    /// <summary>
    /// Classe de mapeamento entre FaltaImprimir e FaltaImprimirDto.
    /// </summary>
    public static class FaltaImprimirMapper
    {
        /// <summary>
        /// Converte um DTO de FaltaImprimir para uma entidade FaltaImprimir.
        /// </summary>
        /// <param name="dto">DTO de Falta Imprimir.</param>
        /// <returns>Retorna entidade Falta Imprimir.</returns>
        public static IFaltaImprimir ToEntity(this IFaltaImprimirDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            if (!DateTime.TryParse(dto.DataImpressao, out DateTime dataImpressao))
            {
                dataImpressao = DateTime.MinValue;
            }

            var entity = new FaltaImprimir()
            {
                Id = dto.Id,
                IdEtiquetaImpressao = dto.IdEtiquetaImpressao,
                NomeDoJOB = dto.NomeDoJOB,
                DataImpressao = dataImpressao,
                StatusImpressora = dto.StatusImpressora,
                FaltaImpressao = dto.FaltaImpressao
            };

            return entity;
        }

        /// <summary>
        /// Converte uma entidade FaltaImprimir para um DTO de FaltaImprimir.
        /// </summary>
        /// <param name="entity">entidide Falta Imprimir.</param>
        /// <returns>Retorna DTO de Falta Imprimir.</returns>
        public static IFaltaImprimirDto ToDTO(this IFaltaImprimir entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = new DTOs.FaltaImprimirDto()
            {
                Id = entity.Id,
                IdEtiquetaImpressao = entity.IdEtiquetaImpressao,
                NomeDoJOB = entity.NomeDoJOB,
                DataImpressao = entity.DataImpressao.ToString("o"),
                StatusImpressora = entity.StatusImpressora,
                FaltaImpressao = entity.FaltaImpressao
            };

            AssignStatusState(dto);
            return dto;
        }

        /// <summary>
        /// Atribui o status e o estado com base no código de status da impressora.
        /// </summary>
        /// <param name="dto">DTO de Falta Imprimir</param>
        /// <returns>Retorna Entidade Falta Imprimir.</returns>
        private static IFaltaImprimirDto AssignStatusState(IFaltaImprimirDto dto)
        {
            var status = Encoding.ASCII.GetString(new byte[] { dto.StatusImpressora });
            switch (status)
            {
                case "A":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "NO ERROR";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "B":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "C":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BUFFER QUASE CHEIO";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "D":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "E":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "PAUSA IMPRESSORA PELO OPERADOR(SEM ERRO)";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "G":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "NO ERROR";
                    dto.State = "PRINTING";
                    break;
                case "H":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM";
                    dto.State = "PRINTING";
                    break;
                case "I":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BUFFER QUASE CHEIO";
                    dto.State = "PRINTING";
                    break;
                case "J":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "PRINTING";
                    break;
                case "K":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "PAUSA IMPRESSORA PELO OPERADOR(SEM ERRO)";
                    dto.State = "PRINTING";
                    break;
                case "M":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "NO ERROR";
                    dto.State = "STANDBY";
                    break;
                case "N":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM";
                    dto.State = "STANDBY";
                    break;
                case "O":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BUFFER QUASE CHEIO";
                    dto.State = "STANDBY";
                    break;
                case "P":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "STANDBY";
                    break;
                case "Q":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "PAUSA IMPRESSORA PELO OPERADOR(SEM ERRO)";
                    dto.State = "STANDBY";
                    break;
                case "S":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "NO ERROR";
                    dto.State = "ANALYZING / EDITING";
                    break;
                case "T":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM";
                    dto.State = "ANALYZING / EDITING";
                    break;
                case "U":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BUFFER QUASE CHEIO";
                    dto.State = "ANALYZING / EDITING";
                    break;
                case "V":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "ANALYZING / EDITING";
                    break;
                case "W":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "PAUSA IMPRESSORA PELO OPERADOR(SEM ERRO)";
                    dto.State = "ANALYZING / EDITING";
                    break;
                case "a":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "ESTOURO DO BUFFER";
                    dto.State = "ERROR";
                    break;
                case "b":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "CABECA IMPRESSAO ABERTA";
                    dto.State = "ERROR";
                    break;
                case "c":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "FIM DO PAPEL";
                    dto.State = "ERROR";
                    break;
                case "d":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "FIM DO RIBBON";
                    dto.State = "ERROR";
                    break;
                case "e":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "MIDIA ERRO";
                    dto.State = "ERROR";
                    break;
                case "f":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "SENSOR ERRO";
                    dto.State = "ERROR";
                    break;
                case "g":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "CABECA IMPRESSAO ERRO";
                    dto.State = "ERROR";
                    break;
                case "h":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "CORTADOR ABERTO";
                    dto.State = "ERROR";
                    break;
                case "i":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "CARD ERRO";
                    dto.State = "ERROR";
                    break;
                case "j":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "CORTADOR ERRO";
                    dto.State = "ERROR";
                    break;
                case "k":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "OUTRO ERRO";
                    dto.State = "ERROR";
                    break;
                case "o":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "RFID TAG ERRO";
                    dto.State = "ERROR";
                    break;
                case "p":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "RFID PROTECAO ERRO";
                    dto.State = "ERROR";
                    break;
                case "q":
                    dto.IsOnline = false;
                    dto.IsError = true;
                    dto.DescricaoStatusImpressora = "BATERIA ERRO";
                    dto.State = "ERROR";
                    break;
                case "0":
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "NO ERRO";
                    dto.State = "OFFLINE";
                    break;
                case "1":
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM";
                    dto.State = "OFFLINE";
                    break;
                case "2":
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BUFFER QUASE CHEIO";
                    dto.State = "OFFLINE";
                    break;
                case "3":
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "RIBBON/LABEL PERTO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "OFFLINE";
                    break;
                case "4":
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMO DO FIM";
                    dto.State = "OFFLINE";
                    break;
                case "5":
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM";
                    dto.State = "OFFLINE";
                    break;
                case "6":
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "OFFLINE";
                    break;
                case "7":
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "OFFLINE";
                    break;
                case "!":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATTERY PROXIMA DO FIM";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "\"":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATTERY PROXIMA DO FIM & RIBBON PROXIMO DO FIM";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "#":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "$":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "WAIT TO RECEIVE";
                    break;
                case "%":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM";
                    dto.State = "PRINTING";
                    break;
                case "&":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM";
                    dto.State = "PRINTING";
                    break;
                case "'":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "PRINTING";
                    break;
                case "(":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "PRINTING";
                    break;
                case ")":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM";
                    dto.State = "STANDBY";
                    break;
                case "*":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM";
                    dto.State = "STANDBY";
                    break;
                case "+":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "STANDBY";
                    break;
                case ",":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "STANDBY";
                    break;
                case "-":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM";
                    dto.State = "ANALYZING / EDITING";
                    break;
                case ".":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM";
                    dto.State = "ANALYZING / EDITING";
                    break;
                case "/":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "ANALYZING / EDITING";
                    break;
                case "@":
                    dto.IsOnline = true;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = "BATERIA PROXIMA DO FIM & RIBBON PROXIMO DO FIM & BUFFER QUASE CHEIO";
                    dto.State = "ANALYZING / EDITING";
                    break;
                default:
                    dto.IsOnline = false;
                    dto.IsError = false;
                    dto.DescricaoStatusImpressora = null;
                    dto.State = null;
                    break;
            }

            return dto;
        }
    }
}
