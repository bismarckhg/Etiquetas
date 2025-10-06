using System;

namespace Etiqueta.Bibliotecas.TaskCorePipe.Modelos
{
    /// <summary>
    /// Contém as configurações para um pipe de comunicação.
    /// </summary>
    public class ConfiguracaoPipe
    {
        /// <summary>
        /// Nome base para o pipe. O nome final pode ser sufixado com um ID único.
        /// </summary>
        public string NomePipe { get; set; }

        /// <summary>
        /// Timeout para operações de leitura no pipe.
        /// </summary>
        public TimeSpan TimeoutLeitura { get; set; }

        /// <summary>
        /// Timeout para operações de escrita no pipe.
        /// </summary>
        public TimeSpan TimeoutEscrita { get; set; }

        /// <summary>
        /// Tamanho do buffer de entrada do pipe em bytes.
        /// </summary>
        public int BufferEntrada { get; set; }

        /// <summary>
        /// Tamanho do buffer de saída do pipe em bytes.
        /// </summary>
        public int BufferSaida { get; set; }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="ConfiguracaoPipe"/> com valores padrão.
        /// </summary>
        public ConfiguracaoPipe()
        {
            TimeoutLeitura = TimeSpan.FromSeconds(30);
            TimeoutEscrita = TimeSpan.FromSeconds(30);
            BufferEntrada = 4096;
            BufferSaida = 4096;
        }
    }
}
