using Etiquetas.Core.Constants;
using System;

namespace Etiqueta.Application.DTOs
{
    /// <summary>
    /// 
    /// O objetivo deste protocolo de comunicação é retornar a condição da Impressora Sato e responder como um status ao host, recebendo quatro tipos de comandos de solicitação e comando de impressão.
    /// No servidor de soquete do TCP/IP,
    /// a Porta 1 é usada para recepção de dados de impressão,
    /// a Porta 2 é usada para conexão da segunda porta de retorno de status da impressora,
    /// a Porta 3 é usada para conexão de 1 porta para recebimento de dados de impressão e de retorno de status da impressora.
    /// 
    /// Quando a Porta 3 é usada,
    /// apenas os dados de retorno de status e os dados de solicitação de configuração de operação da impressora são retornados ao host.
    /// 
    /// Não use conexões da Porta 3 em conjunto com a Porta 1 ou com a Porta 2.
    /// 
    /// Não é possível usar conexão das 3 portas ao mesmo tempo.
    /// 
    /// Somente a conexao simultanea da Porta 1 e 2. Ou somente da Porta 3.
    /// 
    /// Não é possível ter várias sessões conectadas ao mesmo tempo a cada Socket.
    /// 
    /// Status4/Cycle response mode Conexao da Porta 1 e 2 ou da Porta 3.
    /// 
    /// O status da impressora é emitido em um determinado período (intervalo de 500 a 1000 ms).
    /// 
    /// O status da impressora é retornado quando este produto recebe o comando de solicitação de status do host.
    /// 
    /// ProtocoloRecebido Exemplo:
    /// 000 000 [constENQ][constSTX]00A000000                [constETX]
    /// 
    ///                   -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ///                   |   NumeroBytesEnviados1    |   NumeroBytesEnviados2    |  constENQ |  constSTX |   Job ID    |Status|             NumeroFaltaLabels           |                               Job Name (Nome do  Job)                                                         |  constETX |
    ///                   |---------------------------|---------------------------|------|------|-------------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|
    /// Sequencia Campos: |    1 |    2 |    3 |    4 |    1 |    2 |    3 |    4 |    1 |    1 |    1 |    2 |    1 |    1 |    2 |    3 |    4 |    5 |    6 |    1 |    2 |    3 |    4 |    5 |    6 |    7 |    8 |    9 |   10 |   11 |   12 |   13 |   14 |   15 |   16 |    1 |
    /// Dados:            |    0 |    0 |    0 |      |    0 |    0 |    0 |      | [constENQ]| [constSTX]|    0 |    0 |    A |    0 |    0 |    0 |    0 |    0 |    0 |    1 |    2 |    3 |    4 |    5 |    6 |    7 |    8 |    9 |   10 |   11 |   12 |   13 |   14 |   15 |   16 | [constETX]|
    /// Decimal:          |    0 |    0 |    0 |   32 |    0 |    0 |    0 |   28 |    5 |    2 |   48 |   48 |   65 |   48 |   48 |   48 |   48 |   48 |   48 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |   32 |    3 |
    /// Hexadecimal:      |  00H |  00H |  00H |  20H |  00H |  00H |  00H |  1CH |  05H |  02H |  30H |  30H |  41H |  30H |  30H |  30H |  30H |  30H |  30H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  20H |  03H |
    ///                   ----------------------------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|
    ///                   |    1 |    2 |    3 |    4 |    5 |    6 |    7 |    8 |    9 |   10 |   11 |   12 |   13 |   14 |   15 |   16 |   17 |   18 |   19 |   20 |   21 |   22 |   23 |   24 |   25 |   26 |   27 |   28 |   29 |   30 |   31 |   32 |   33 |   34 |   35 |   36 |
    ///                          NumeroBytesEnviados1 |    1 |    2 |    3 |    4 |    5 |    6 |    7 |    8 |    9 |   10 |   11 |   12 |   13 |   14 |   15 |   16 |   17 |   18 |   19 |   20 |   21 |   22 |   23 |   24 |   25 |   26 |   27 |   28 |   29 |   30 |   31 |   32 |
    ///                                                      NumeroBytesEnviados2 |    1 |    2 |    3 |    4 |    5 |    6 |    7 |    8 |    9 |   10 |   11 |   12 |   13 |   14 |   15 |   16 |   17 |   18 |   19 |   20 |   21 |   22 |   23 |   24 |   25 |   26 |   27 |   28 |
    ///                                                                           |------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|
    ///                                                                        
    /// TamanhoProtocolo
    /// = 4 + 4 = 8
    /// = 8 + 1 = 9
    /// = 9 + 1 = 10
    /// = 10 + 2 = 12
    /// = 12 + 1 = 13
    /// = 13 + 6 = 19
    /// = 19 + 16 = 35
    /// = 35 + 1 = 36
    /// 
    /// Representa um objeto de transferência de dados retornado pela Impressora Sato,
    /// referente a cada copia impressa de um JOB de impressão de etiquetas.
    /// </summary>
    /// <remarks>Cada pacote possui o numero e nome do JOB de impressão, e a quantidade de etiquetas que faltam imprimir.</remarks>
    public class SatoDto : ISatoDto
    {
        /// <inheritdoc/>
        public byte[] RetornoImpressao { get; set; } = new byte[PadraoConstantes.ConstTamanhoProtocolo];

        /// <inheritdoc/>
        public byte ENQ
        {
            get => GetRetornoImpressao(PosicaoENQRetornoImpressao);
        }

        /// <inheritdoc/>
        public int PosicaoENQRetornoImpressao
        {
            get => GetPosicaoEmRetornoImpressao(PadraoConstantes.ConstENQ);
        }

        /// <inheritdoc/>
        public byte STX
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao);
        }

        /// <inheritdoc/>
        public int PosicaoSTXRetornoImpressao
        {
            get => GetPosicaoEmRetornoImpressao(PadraoConstantes.ConstSTX);
        }

        /// <inheritdoc/>
        public string JobId
        {
            get => GetJobId();
        }

        /// <inheritdoc/>
        public byte JobId1
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 1);
        }

        /// <inheritdoc/>
        public byte JobId2
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 2);
        }

        /// <inheritdoc/>
        public byte Status
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 3);
        }

        /// <inheritdoc/>
        public long NumeroFaltaImprimir => GetNumeroFaltaImprimir();

        /// <inheritdoc/>
        public byte NumeroFaltaImprimir1
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 4);
        }

        /// <inheritdoc/>
        public byte NumeroFaltaImprimir2
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 5);
        }

        /// <inheritdoc/>
        public byte NumeroFaltaImprimir3
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 6);
        }

        /// <inheritdoc/>
        public byte NumeroFaltaImprimir4
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 7);
        }

        /// <inheritdoc/>
        public byte NumeroFaltaImprimir5
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 8);
        }

        /// <inheritdoc/>
        public byte NumeroFaltaImprimir6
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 9);
        }

        /// <inheritdoc/>
        public string JobName
        {
            get => GetJobName();
        }

        /// <inheritdoc/>
        public byte JobName1
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 10);
        }

        /// <inheritdoc/>
        public byte JobName2
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 11);
        }

        /// <inheritdoc/>
        public byte JobName3
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 12);
        }

        /// <inheritdoc/>
        public byte JobName4
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 13);
        }

        /// <inheritdoc/>
        public byte JobName5
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 14);
        }

        /// <inheritdoc/>
        public byte JobName6
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 15);
        }

        /// <inheritdoc/>
        public byte JobName7
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 16);
        }

        /// <inheritdoc/>
        public byte JobName8
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 17);
        }

        /// <inheritdoc/>
        public byte JobName9
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 18);
        }

        /// <inheritdoc/>
        public byte JobName10
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 19);
        }

        /// <inheritdoc/>
        public byte JobName11
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 20);
        }

        /// <inheritdoc/>
        public byte JobName12
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 21);
        }

        /// <inheritdoc/>
        public byte JobName13
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 22);
        }

        /// <inheritdoc/>
        public byte JobName14
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 23);
        }

        /// <inheritdoc/>
        public byte JobName15
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 24);
        }

        /// <inheritdoc/>
        public byte JobName16
        {
            get => GetRetornoImpressao(PosicaoSTXRetornoImpressao + 25);
        }

        /// <summary>
        /// Gets - Byte de End of Text (constETX) enviado pela impressora Sato.
        /// </summary>
        public byte ETX
        {
            get => GetRetornoImpressao(PosicaoETXRetornoImpressao);
        }

        /// <summary>
        /// Gets - Posição do Byte de End of Text (constETX) no array RetornoImpressao. 
        /// </summary>
        public int PosicaoETXRetornoImpressao
        {
            get => GetPosicaoEmRetornoImpressao(PadraoConstantes.ConstETX);
        }

        /// <summary>
        /// Verifica se o protocolo retornado pela Impressora Sato é válido.
        /// </summary>
        /// <returns>Verdadeiro se validações basicas estiverem verdadeiras.</returns>
        public bool EhValido()
        {
            return ENQ == PadraoConstantes.ConstENQ
                && STX == PadraoConstantes.ConstSTX
                && ETX == PadraoConstantes.ConstETX
                && PosicaoENQRetornoImpressao == 0
                && PosicaoSTXRetornoImpressao == 1
                && PosicaoETXRetornoImpressao == PadraoConstantes.ConstTamanhoProtocolo - 1
                && RetornoImpressao.Length == PadraoConstantes.ConstTamanhoProtocolo;
        }

        /// <summary>
        /// Gets - Identificador do JOB de impressão.
        /// </summary>
        /// <returns>Retorna string JobId concatenação char.</returns>
        protected string GetJobId()
        {
            return string.Format("{0}{1}", (char)JobId1, (char)JobId2);
        }

        /// <summary>
        /// Gets - Retorna o byte de Retorno da Impressão na posição informada.
        /// </summary>
        /// <param name="posicao">posicao no array de retorno da impressão.</param>
        /// <returns>Retorna byte de Retorbno Impressao.</returns>
        protected byte GetRetornoImpressao(int posicao)
        {
            try
            {
                return RetornoImpressao[posicao];
            }
            catch
            {
                return 32;
            }
        }

        /// <summary>
        /// Gets - Numero de etiquetas que faltam imprimir do JOB de etiquetas.
        /// </summary>
        /// <returns>Falta Imprimir cocatenado em long.</returns>
        protected long GetNumeroFaltaImprimir()
        {
            try
            {
                var digits = new byte[]
                {
                    NumeroFaltaImprimir1, NumeroFaltaImprimir2,
                    NumeroFaltaImprimir3, NumeroFaltaImprimir4,
                    NumeroFaltaImprimir5, NumeroFaltaImprimir6,
                };
                var s = string.Concat(Array.ConvertAll(digits, d => ((char)d).ToString()));
                long.TryParse(s, out long v);
                return v;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets - Retorna a posição do valor informado no array RetornoImpressao.
        /// </summary>
        /// <param name="valor">Obtem a posicao do valor do byte informado dentro do Array RetornoImpressao.</param>
        /// <returns>Retorna a posicao no array de RetornoImpressao do valor do byte informado.</returns>
        protected int GetPosicaoEmRetornoImpressao(byte valor)
        {
            return Array.IndexOf(RetornoImpressao, valor);
        }

        /// <summary>
        /// Gets - Nome do JOB de impressão.
        /// </summary>
        /// <returns>string com o JobName concatenado byte.</returns>
        protected string GetJobName()
        {
            var arr = new byte[]
            {
                JobName1,
                JobName2,
                JobName3,
                JobName4,
                JobName5,
                JobName6,
                JobName7,
                JobName8,
                JobName9,
                JobName10,
                JobName11,
                JobName12,
                JobName13,
                JobName14,
                JobName15,
                JobName16,
            };
            return System.Text.Encoding.ASCII.GetString(arr).Trim('\0', ' ');
        }
    }
}
