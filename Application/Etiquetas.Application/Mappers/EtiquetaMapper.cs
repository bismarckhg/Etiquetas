using Etiquetas.Application.DTOs;
using Etiquetas.Bibliotecas.ControleFilaDados;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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

        public static IEtiquetaImpressaoDto SpolerToDto(this string impressao, IPosicaoCamposEtiqueta posicaoCamposEtiqueta)
        {
            if (impressao == null)
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







        /// <summary>
        /// Extrai o código da primeira linha (texto entre aspas)
        /// </summary>
        /// <param name="message">Mensagem completa</param>
        /// <returns>Código extraído ou null se não encontrado</returns>
        protected IEtiquetaImpressaoDto ExtrairDadosEtiqueta(string message, IPosicaoCamposEtiqueta posicaoCamposEtiqueta)
        {

            if (string.IsNullOrEmpty(message))
                return null;

            var dados = new EtiquetaImpressaoDto();

            try
            {
                var linhas = QuebraComandosZPLEmLinhasIndividuais(message);

                // Alias locais para evitar acessos repetidos a campos
                var posCodigo = posicaoCamposEtiqueta.PosicaoCodigo;
                //var posDesc1 = posicaoCamposEtiqueta.PosicaoDescricao1;
                //var posDesc2 = posicaoCamposEtiqueta.PosicaoDescricao2;
                //var posEmbalagem = posicaoCamposEtiqueta.PosicaoEmbalagem;
                //var posLote = posicaoCamposEtiqueta.PosicaoLote;
                //var posValidade = posicaoCamposEtiqueta.PosicaoDataValidade;
                //var posUsuario = posicaoCamposEtiqueta.PosicaoUsuario;
                //var posCodigoBarras = posicaoCamposEtiqueta.PosicaoCodigoBarras;
                //var cmdCopias = posicaoCamposEtiqueta.ComandoNumeroCopias;

                var marcadorFD = posicaoCamposEtiqueta.MarcadorInicialTexto; // ex.: "^FD"
                var marcadorFS = posicaoCamposEtiqueta.MarcadorFinalTexto;   // ex.: "^FS"

                // Detecta qual campo é esta linha (prefix match)
                Campo campo = Campo.Nenhum;

                foreach (var cmd in linhas)
                {
                    // Remove apenas espaços à esquerda para preservar posições depois de ^FD
                    var line = cmd?.TrimStart();
                    if (EhStringNuloVazioComEspacosBranco.Execute(line)) continue;

                    if (line.StartsWith(posCodigo, StringComparison.Ordinal)) campo = Campo.Codigo;
                    else if (line.StartsWith(posDesc1, StringComparison.Ordinal)) campo = Campo.Descricao1;
                    else if (line.StartsWith(posDesc2, StringComparison.Ordinal)) campo = Campo.Descricao2;
                    else if (line.StartsWith(posEmbalagem, StringComparison.Ordinal)) campo = Campo.Embalagem;
                    else if (line.StartsWith(posUsuario, StringComparison.Ordinal)) campo = Campo.Usuario;
                    else if (line.StartsWith(posLote, StringComparison.Ordinal)) campo = Campo.Lote;
                    else if (line.StartsWith(posValidade, StringComparison.Ordinal)) campo = Campo.Validade;
                    else if (line.StartsWith(posCodigoBarras, StringComparison.Ordinal)) campo = Campo.CodigoBarras;
                    else if (line.StartsWith(cmdCopias, StringComparison.Ordinal)) campo = Campo.Copias;

                    //if (campo == Campo.Nenhum)
                    //    continue;

                    // Extrai o texto alvo da linha (entre ^FD ... ^FS) ou após ^PQ
                    var texto = ExtrairValorLinha(line, marcadorFD, marcadorFS, cmdCopias);
                    if (string.IsNullOrEmpty(texto))
                        continue;

                    switch (campo)
                    {
                        case Campo.Codigo:
                            // Somente dígitos (rápido, ASCII 0-9); troque por sua rotina se necessário
                            dados.Codigo = StringExtrairSomenteDigitosNumericos.Execute(texto);
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Descricao1:
                            dados.Descricao1 = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Descricao2:
                            dados.Descricao2 = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Embalagem:
                            dados.Embalagem = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Usuario:
                            dados.Usuario = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Lote:
                            dados.Lote = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Validade:
                            dados.Validade = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.CodigoBarras:
                            var digito = Pictogramas.Bibliotecas.EAN13.CalcularDigitoVerificador(texto);
                            var ean13 = texto + digito.ToString();
                            dados.CodigoBarras = ean13; // mantém como texto original
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Copias:
                            dados.Copias = texto; // se quiser validar numérico, faça aqui
                            campo = Campo.Nenhum;
                            break;
                    }
                }

                return dados;
            }
            catch (Exception ex)
            {
                //OnErrorOccurred(ex);
                //return null;
            }

            // Extrai: ^FD ... ^FS   ou   ^PQ...
            string ExtrairValorLinha(string line, string marcadorFD, string marcadorFS, string comandoCopias)
            {
                if (line == null) return null;

                // 1) Tenta ^FD ... ^FS
                int idxFD = line.IndexOf(marcadorFD, StringComparison.Ordinal);
                if (idxFD >= 0)
                {
                    int ini = idxFD + marcadorFD.Length;

                    int idxFS = -1;
                    if (!string.IsNullOrEmpty(marcadorFS))
                        idxFS = line.IndexOf(marcadorFS, ini, StringComparison.Ordinal);

                    int fim = (idxFS >= 0) ? idxFS : line.Length;
                    int len = fim - ini;

                    if (len > 0)
                        return line.Substring(ini, len);
                }

                // 2) Se não tem ^FD, tenta ^PQ (cópias)
                int idxPQ = line.IndexOf(comandoCopias, StringComparison.Ordinal);
                if (idxPQ >= 0)
                {
                    int ini = idxPQ + comandoCopias.Length; // ex.: "^PQ"
                                                            // Alguns formatos usam "^PQ," ou "^PQ,"
                    while (ini < line.Length && (line[ini] == ':' || line[ini] == ',' || line[ini] == ' ')) ini++;
                    var rest = line.Substring(ini).Trim();
                    return rest.Length > 0 ? rest : null;
                }

                return null;
            }
        }

        // --- Tipos/Helpers locais ---
        private enum Campo
        {
            Nenhum = 0,
            Codigo,
            Descricao1,
            Descricao2,
            Embalagem,
            Lote,
            Validade,
            Usuario,
            CodigoBarras,
            Copias
        }

        /// <summary>
        /// Quebra comandos ZPL em linhas individuais.
        /// </summary>
        /// <param name="texto">Sppoler comandos ZPL</param>
        /// <returns>Array string com linhas separadas.</returns>
        private static string[] QuebraComandosZPLEmLinhasIndividuais(string texto)
        {

            // Separa comandos por quebra de linhas, removendo linhas vazias
            //var quebraLinhas = Etiquetas.Bibliotecas.Comum.StringEmArrayStringPorSeparador.Execute(texto, new[] { "\r\n", "\n", "\r" }, true);
            var quebraLinhas = Etiquetas.Bibliotecas.Comum.Arrays.StringEmArrayStringPorSeparador.Execute(texto, new[] { "\r\n", "\n", "\r" }, true);

            var quebraComandosEmLinhas = new ConcurrentQueue<IReadOnlyList<string>>();
            foreach (var linha in quebraLinhas)
            {
                // Processa cada linha individualmente
                // Separa varios comandos que estão em uma mesma linhas, em comandos com linhas individuais, mantendo o inicio de comando "^"
                //var quebralinhaComandoEmLinhas = Etiquetas.Bibliotecas.StringEmArrayStringComSeparadorEmCadaItem.Execute(linha, "^", true);
                var quebralinhaComandoEmLinhas = Etiquetas.Bibliotecas.Comum.Arrays.StringEmArrayStringComSeparadorEmCadaItem.Execute(linha, "^", true);

                quebraComandosEmLinhas.EnqueueBatch(quebralinhaComandoEmLinhas);
            }

            return quebraComandosEmLinhas.ToFlattenedArraySnapshot();
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
