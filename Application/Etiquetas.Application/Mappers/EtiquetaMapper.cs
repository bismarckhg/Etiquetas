using Etiquetas.Application.DTOs;
using Etiquetas.Bibliotecas.Comum.Caracteres;
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
                PrincipioAtivo = ent.PrincipioAtivo1,
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
        private static IEtiquetaImpressaoDto ExtrairDadosEtiqueta(string message, IPosicaoCamposEtiqueta posicaoCamposEtiqueta)
        {

            if (string.IsNullOrEmpty(message))
                return null;

            var dados = new EtiquetaImpressaoDto();

            try
            {
                var linhas = QuebraComandosZPLEmLinhasIndividuais(message);

                // Alias locais para evitar acessos repetidos a campos
                //var posCodigoMaterial1 = posicaoCamposEtiqueta.PosicaoCodigo;
                //var posDesc1 = posicaoCamposEtiqueta.PosicaoDescricao1;
                //var posDesc2 = posicaoCamposEtiqueta.PosicaoDescricao2;
                //var posEmbalagem = posicaoCamposEtiqueta.PosicaoEmbalagem;
                //var posLote = posicaoCamposEtiqueta.PosicaoLote;
                //var posValidade = posicaoCamposEtiqueta.PosicaoDataValidade;
                //var posUsuario = posicaoCamposEtiqueta.PosicaoUsuario;
                //var posCodigoBarras = posicaoCamposEtiqueta.PosicaoCodigoBarras;
                //var cmdCopias = posicaoCamposEtiqueta.ComandoNumeroCopias;

                var marcadorInicioTexto = posicaoCamposEtiqueta.MarcadorInicialTexto; // ex.: "^FD"
                var marcadorFimTexto = posicaoCamposEtiqueta.MarcadorFinalTexto;   // ex.: "^FS"

                // Detecta qual campo é esta linha (prefix match)
                Campo campo = Campo.Nenhum;

                var posicaoCmd1 = string.Empty;
                var posicaoCmd2 = string.Empty;

                foreach (var cmd in linhas)
                {
                    // Remove apenas espaços à esquerda para preservar posições depois de ^FD
                    var line = cmd?.TrimStart();
                    if (EhStringNuloVazioComEspacosBranco.Execute(line)) continue;

                    // Verifica Posicao dos Campos (alguns modelos usam dois comandos diferentes para o mesmo campo, um de linha e outro de coluna).
                    if (line.StartsWith(posicaoCamposEtiqueta.CodigoMaterialCmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.CodigoMaterialCmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.DescricaoMedicamentoCmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.DescricaoMedicamentoCmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.DescricaoMedicamento2Cmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.DescricaoMedicamento2Cmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.PrincipioAtivo1Cmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.PrincipioAtivo1Cmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.PrincipioAtivo2Cmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.PrincipioAtivo2Cmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.EmbalagemCmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.EmbalagemCmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.CodigoUsuarioCmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.CodigoUsuarioCmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.LoteCmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.LoteCmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.ValidadeCmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.ValidadeCmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.CodigoBarrasCmd1, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.CodigoBarrasCmd1;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.CopiasCmd, StringComparison.Ordinal))
                    {
                        posicaoCmd1 = posicaoCamposEtiqueta.CopiasCmd;
                    }

                    // Verifica segunda posição (alguns modelos usam dois comandos diferentes para o mesmo campo, um de linha e outro de coluna).
                    if (line.StartsWith(posicaoCamposEtiqueta.CodigoMaterialCmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.CodigoMaterialCmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.DescricaoMedicamentoCmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.DescricaoMedicamentoCmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.DescricaoMedicamento2Cmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.DescricaoMedicamento2Cmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.PrincipioAtivo1Cmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.PrincipioAtivo1Cmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.PrincipioAtivo2Cmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.PrincipioAtivo2Cmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.EmbalagemCmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.EmbalagemCmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.CodigoUsuarioCmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.CodigoUsuarioCmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.LoteCmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.LoteCmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.ValidadeCmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.ValidadeCmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.CodigoBarrasCmd2, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = posicaoCamposEtiqueta.CodigoBarrasCmd2;
                    }
                    else if (line.StartsWith(posicaoCamposEtiqueta.CopiasCmd, StringComparison.Ordinal))
                    {
                        posicaoCmd2 = String.Empty;
                    }

                    // Define qual campo esta sendo processado
                    if (posicaoCmd1 == posicaoCamposEtiqueta.CodigoMaterialCmd1 && posicaoCmd2 == posicaoCamposEtiqueta.CodigoMaterialCmd2)
                    {
                        campo = Campo.CodigoMaterial;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.DescricaoMedicamentoCmd1 && posicaoCmd2 == posicaoCamposEtiqueta.DescricaoMedicamentoCmd2)
                    {
                        campo = Campo.DescricaoMedicamento;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.DescricaoMedicamento2Cmd1 && posicaoCmd2 == posicaoCamposEtiqueta.DescricaoMedicamento2Cmd2)
                    {
                        campo = Campo.DescricaoMedicamento2;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.PrincipioAtivo1Cmd1 && posicaoCmd2 == posicaoCamposEtiqueta.PrincipioAtivo1Cmd2)
                    {
                        campo = Campo.PrincipioAtivo;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.PrincipioAtivo2Cmd1 && posicaoCmd2 == posicaoCamposEtiqueta.PrincipioAtivo2Cmd2)
                    {
                        campo = Campo.PrincipioAtivo2;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.EmbalagemCmd1 && posicaoCmd2 == posicaoCamposEtiqueta.EmbalagemCmd2)
                    {
                        campo = Campo.Embalagem;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.CodigoUsuarioCmd1 && posicaoCmd2 == posicaoCamposEtiqueta.CodigoUsuarioCmd2)
                    {
                        campo = Campo.CodigoUsuario;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.LoteCmd1 && posicaoCmd2 == posicaoCamposEtiqueta.LoteCmd2)
                    {
                        campo = Campo.Lote;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.ValidadeCmd1 && posicaoCmd2 == posicaoCamposEtiqueta.ValidadeCmd2)
                    {
                        campo = Campo.Validade;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.CodigoBarrasCmd1 && posicaoCmd2 == posicaoCamposEtiqueta.CodigoBarrasCmd2)
                    {
                        campo = Campo.CodigoBarras;
                    }
                    else if (posicaoCmd1 == posicaoCamposEtiqueta.CopiasCmd && posicaoCmd2 == string.Empty)
                    {
                        campo = Campo.Copias;
                    }

                    // Extrai o texto alvo da linha (entre ^FD ... ^FS) ou após ^PQ
                    var texto = ExtrairValorLinha(line, marcadorInicioTexto, marcadorFimTexto, posicaoCamposEtiqueta.CopiasCmd);
                    if (string.IsNullOrEmpty(texto))
                    {
                        continue;
                    }

                    switch (campo)
                    {
                        case Campo.CodigoMaterial:
                            // Somente dígitos (rápido, ASCII 0-9); troque por sua rotina se necessário
                            dados.CodigoMaterial = Etiquetas.Bibliotecas.Comum.Numericos.StringExtrairSomenteDigitosNumericos.Execute(texto);
                            campo = Campo.Nenhum;
                            break;

                        case Campo.DescricaoMedicamento:
                            dados.DescricaoMedicamento = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.DescricaoMedicamento2:
                            dados.DescricaoMedicamento2 = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Embalagem:
                            dados.Embalagem = texto;
                            campo = Campo.Nenhum;
                            break;

                        case Campo.CodigoUsuario:
                            dados.CodigoUsuario = texto;
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
                            //var digito = Pictogramas.Bibliotecas.EAN13.CalcularDigitoVerificador(texto);
                            var ean13 = texto + digito.ToString();
                            dados.CodigoBarras = ean13; // mantém como texto original
                            campo = Campo.Nenhum;
                            break;

                        case Campo.Copias:
                            dados.QuantidadeSolicitada = texto; // se quiser validar numérico, faça aqui
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
            CodigoMaterial,
            DescricaoMedicamento,
            DescricaoMedicamento2,
            PrincipioAtivo,
            PrincipioAtivo2,
            Embalagem,
            Lote,
            Validade,
            CodigoUsuario,
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
                QuantidadeSolicitada = ent.QuantidadeSolicitada,
                FaltaImpressao = ent.FaltaImpressao,
            };
        }
    }
}
