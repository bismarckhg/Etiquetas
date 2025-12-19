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
    /// 000 000 [ENQ][STX]00A000000                [ETX]
    /// 
    ///                   -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    ///                   |   NumeroBytesEnviados1    |   NumeroBytesEnviados2    |  ENQ |  STX |   Job ID    |Status|             NumeroFaltaLabels           |                               Job Name (Nome do  Job)                                                         |  ETX |
    ///                   |---------------------------|---------------------------|------|------|-------------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|------|
    /// Sequencia Campos: |    1 |    2 |    3 |    4 |    1 |    2 |    3 |    4 |    1 |    1 |    1 |    2 |    1 |    1 |    2 |    3 |    4 |    5 |    6 |    1 |    2 |    3 |    4 |    5 |    6 |    7 |    8 |    9 |   10 |   11 |   12 |   13 |   14 |   15 |   16 |    1 |
    /// Dados:            |    0 |    0 |    0 |      |    0 |    0 |    0 |      | [ENQ]| [STX]|    0 |    0 |    A |    0 |    0 |    0 |    0 |    0 |    0 |    1 |    2 |    3 |    4 |    5 |    6 |    7 |    8 |    9 |   10 |   11 |   12 |   13 |   14 |   15 |   16 | [ETX]|
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
    public interface ISatoDto
    {

        /// <summary>
        /// Gets or sets - Retorno da Impressora Sato na segunda porta, referente a quantidade de copias que faltam imprimir.
        /// Array de byte[28].
        /// </summary>
        byte[] RetornoImpressao { get; set; }

        /// <summary>
        /// Gets - Byte de Enquiry (ENQ) enviado pela impressora Sato.
        /// </summary>
        byte ENQ { get; }

        /// <summary>
        /// Gets - Posição do Byte de Enquiry (ENQ) no array RetornoImpressao.
        /// </summary>
        int PosicaoENQRetornoImpressao { get; }

        /// <summary>
        /// Gets - Byte de Start of Text (STX) enviado pela impressora Sato.
        /// </summary>
        byte STX { get; }

        /// <summary>
        /// Gets - Posição do Byte de Start of Text (STX) no array RetornoImpressao.
        /// </summary>
        int PosicaoETXRetornoImpressao { get; }

        /// <summary>
        /// Gets - Identificador do JOB de impressão.
        /// </summary>
        string JobId { get; }

        /// <summary>
        /// Gets - Identificador (digito2) do JOB de impressão.
        /// </summary>
        byte JobId1 { get; }

        /// <summary>
        /// Gets - Identificador (digito1) do JOB de impressão.
        /// </summary>
        byte JobId2 { get; }

        /// <summary>
        /// Gets - Status da impressão do JOB de etiquetas.
        /// </summary>
        byte Status { get; }

        /// <summary>
        /// Gets - Numero de etiquetas que faltam imprimir do JOB de etiquetas.
        /// </summary>
        long NumeroFaltaImprimir { get; }

        /// <summary>
        /// Gets - Numero(digito 1) de etiquetas que faltam imprimir do JOB de etiquetas.
        /// </summary>
        byte NumeroFaltaImprimir1 { get; }

        /// <summary>
        /// Gets - Numero(digito 2) de etiquetas que faltam imprimir do JOB de etiquetas.
        /// </summary>
        byte NumeroFaltaImprimir2 { get; }

        /// <summary>
        /// Gets - Numero(digito 3) de etiquetas que faltam imprimir do JOB de etiquetas.
        /// </summary>
        byte NumeroFaltaImprimir3 { get; }

        /// <summary>
        /// Gets - Numero(digito 4) de etiquetas que faltam imprimir do JOB de etiquetas.
        /// </summary>
        byte NumeroFaltaImprimir4 { get; }

        /// <summary>
        /// Gets - Numero(digito 5) de etiquetas que faltam imprimir do JOB de etiquetas.
        /// </summary>
        byte NumeroFaltaImprimir5 { get; }

        /// <summary>
        /// Gets - Numero(digito 6) de etiquetas que faltam imprimir do JOB de etiquetas.
        /// </summary>
        byte NumeroFaltaImprimir6 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão.
        /// </summary>
        string JobName { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 1(byte por byte).
        /// </summary>
        byte JobName1 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 2(byte por byte).
        /// </summary>
        byte JobName2 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 3(byte por byte).
        /// </summary>
        byte JobName3 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 4(byte por byte).
        /// </summary>
        byte JobName4 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 5(byte por byte).
        /// </summary>
        byte JobName5 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 6(byte por byte).
        /// </summary>
        byte JobName6 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 7(byte por byte).
        /// </summary>
        byte JobName7 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 8(byte por byte).
        /// </summary>
        byte JobName8 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 9(byte por byte).
        /// </summary>
        byte JobName9 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 10(byte por byte).
        /// </summary>
        byte JobName10 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 11(byte por byte).
        /// </summary>
        byte JobName11 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 12(byte por byte).
        /// </summary>
        byte JobName12 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 13(byte por byte).
        /// </summary>
        byte JobName13 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 14(byte por byte).
        /// </summary>
        byte JobName14 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 15(byte por byte).
        /// </summary>
        byte JobName15 { get; }

        /// <summary>
        /// Gets - Nome do JOB de impressão byte 16(byte por byte).
        /// </summary>
        byte JobName16 { get; }

        /// <summary>
        /// Gets - Byte de End of Text (ETX) enviado pela impressora Sato.
        /// </summary>
        byte ETX { get; }


        /// <summary>
        /// Gets - Posição do Byte de End of Text (ETX) no array RetornoImpressao. 
        /// </summary>
        int PosicaoSTXRetornoImpressao { get; }

        /// <summary>
        /// Verifica se o protocolo retornado pela Impressora Sato é válido.
        /// </summary>
        /// <returns>Verdadeiro se validações basicas estiverem verdadeiras.</returns>
        bool EhValido();
    }
}
