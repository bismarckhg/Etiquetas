using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SatoTtfPrinter
{
    /// <summary>
    /// Implementação completa do protocolo Status4 via TCP (Portas 1024/1025)
    /// Baseado no manual CL4NX/CL6NX Programming Reference
    /// </summary>
    public class SatoStatus4TCP : IDisposable
    {
        private const int PORT_PRINT = 1024;      // Porta para envio de impressão
        private const int PORT_STATUS = 1025;     // Porta para recebimento de status
        private const int TIMEOUT_MS = 5000;      // Timeout de 5 segundos
        
        private string _ipAddress;
        private TcpClient _printClient;
        private TcpClient _statusClient;
        private NetworkStream _printStream;
        private NetworkStream _statusStream;
        private bool _isConnected;
        private Thread _statusListenerThread;
        private bool _keepListening;
        
        // Último status recebido
        private Status4Response _lastStatus;
        private readonly object _statusLock = new object();

        public SatoStatus4TCP(string ipAddress)
        {
            _ipAddress = ipAddress;
            _isConnected = false;
        }

        /// <summary>
        /// Conecta nas portas 1024 (print) e 1025 (status)
        /// </summary>
        public bool Connect()
        {
            try
            {
                // Conecta na porta de impressão (1024)
                _printClient = new TcpClient();
                _printClient.Connect(_ipAddress, PORT_PRINT);
                _printStream = _printClient.GetStream();
                _printStream.ReadTimeout = TIMEOUT_MS;
                _printStream.WriteTimeout = TIMEOUT_MS;

                // Conecta na porta de status (1025)
                _statusClient = new TcpClient();
                _statusClient.Connect(_ipAddress, PORT_STATUS);
                _statusStream = _statusClient.GetStream();
                _statusStream.ReadTimeout = TIMEOUT_MS;
                _statusStream.WriteTimeout = TIMEOUT_MS;

                _isConnected = true;

                // Inicia thread de escuta contínua de status
                _keepListening = true;
                _statusListenerThread = new Thread(StatusListenerLoop);
                _statusListenerThread.IsBackground = true;
                _statusListenerThread.Start();

                Console.WriteLine($"[Status4TCP] Conectado em {_ipAddress}:1024/1025");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO Status4TCP] Falha na conexão: {ex.Message}");
                Disconnect();
                return false;
            }
        }

        /// <summary>
        /// Desconecta das portas
        /// </summary>
        public void Disconnect()
        {
            _keepListening = false;
            _isConnected = false;

            if (_statusListenerThread != null && _statusListenerThread.IsAlive)
            {
                _statusListenerThread.Join(1000);
            }

            _printStream?.Close();
            _printClient?.Close();
            _statusStream?.Close();
            _statusClient?.Close();

            Console.WriteLine("[Status4TCP] Desconectado");
        }

        /// <summary>
        /// Envia comando SBPL de impressão pela porta 1024
        /// </summary>
        public bool SendPrintCommand(byte[] sbplCommand)
        {
            if (!_isConnected)
            {
                Console.WriteLine("[ERRO] Não conectado");
                return false;
            }

            try
            {
                _printStream.Write(sbplCommand, 0, sbplCommand.Length);
                _printStream.Flush();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao enviar comando: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Solicita status via comando ENQ (0x05) pela porta 1024
        /// </summary>
        public bool RequestStatus()
        {
            if (!_isConnected)
                return false;

            try
            {
                byte[] enqCommand = new byte[] { 0x05 }; // ENQ
                _printStream.Write(enqCommand, 0, enqCommand.Length);
                _printStream.Flush();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] Falha ao solicitar status: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtém o último status recebido
        /// </summary>
        public Status4Response GetLastStatus()
        {
            lock (_statusLock)
            {
                return _lastStatus;
            }
        }

        /// <summary>
        /// Obtém o número de etiquetas restantes na fila
        /// </summary>
        public int GetRemainingLabels()
        {
            var status = GetLastStatus();
            return status?.RemainingLabels ?? -1;
        }

        /// <summary>
        /// Verifica se a impressora está pronta (sem erros críticos)
        /// </summary>
        public bool IsPrinterReady()
        {
            var status = GetLastStatus();
            if (status == null || !status.IsValid)
                return false;

            return status.State != PrinterState.Error && !status.HasCriticalError;
        }

        /// <summary>
        /// Aguarda até que todas as etiquetas sejam impressas
        /// </summary>
        public bool WaitForPrintCompletion(int timeoutSeconds = 60)
        {
            var startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalSeconds < timeoutSeconds)
            {
                var status = GetLastStatus();
                if (status != null && status.IsValid)
                {
                    if (status.RemainingLabels == 0)
                    {
                        Console.WriteLine("[Status4TCP] Impressão concluída!");
                        return true;
                    }

                    Console.WriteLine($"[Status4TCP] Etiquetas restantes: {status.RemainingLabels}");
                }

                Thread.Sleep(500);
            }

            Console.WriteLine("[Status4TCP] Timeout: Impressão não concluída no tempo esperado");
            return false;
        }

        // =================== Thread de Escuta de Status ===================

        private void StatusListenerLoop()
        {
            byte[] buffer = new byte[256];

            while (_keepListening && _isConnected)
            {
                try
                {
                    if (_statusStream.DataAvailable)
                    {
                        int bytesRead = _statusStream.Read(buffer, 0, buffer.Length);
                        if (bytesRead > 0)
                        {
                            var status = ParseStatus4Response(buffer, bytesRead);
                            if (status != null && status.IsValid)
                            {
                                lock (_statusLock)
                                {
                                    _lastStatus = status;
                                }

                                Console.WriteLine($"[Status4TCP] {status}");
                            }
                        }
                    }

                    Thread.Sleep(100); // Polling a cada 100ms
                }
                catch (Exception ex)
                {
                    if (_keepListening)
                    {
                        Console.WriteLine($"[ERRO StatusListener] {ex.Message}");
                    }
                }
            }
        }

        // =================== Parsing da Resposta Status4 ===================

        private Status4Response ParseStatus4Response(byte[] data, int length)
        {
            // Formato esperado (LAN/WLAN, Legacy Status desabilitado): 32 bytes
            // [4 bytes: size] [1: ENQ] [1: STX] [2: JobID] [1: Status] [6: Remaining] [16: JobName] [1: ETX]
            
            if (length < 27)
                return null;

            try
            {
                var response = new Status4Response();
                int offset = 0;

                // Bytes 0-3: Tamanho (ex: "0000001C" = 28 bytes em ASCII hex)
                string sizeStr = Encoding.ASCII.GetString(data, offset, 8);
                offset += 8;

                // Byte 4: ENQ (0x05)
                if (data[offset] != 0x05)
                    return null;
                offset++;

                // Byte 5: STX (0x02)
                if (data[offset] != 0x02)
                    return null;
                offset++;

                // Bytes 6-7: Job ID (2 bytes ASCII)
                response.JobId = Encoding.ASCII.GetString(data, offset, 2).Trim();
                offset += 2;

                // Byte 8: Status (1 byte ASCII)
                char statusChar = (char)data[offset];
                response.StatusCode = statusChar;
                response.State = ParsePrinterState(statusChar);
                response.StatusDescription = GetStatusDescription(statusChar);
                response.HasCriticalError = IsErrorStatus(statusChar);
                offset++;

                // Bytes 9-14: Remaining labels (6 bytes ASCII, ex: "000123")
                string remainingStr = Encoding.ASCII.GetString(data, offset, 6);
                if (int.TryParse(remainingStr, out int remaining))
                    response.RemainingLabels = remaining;
                offset += 6;

                // Bytes 15-30: Job name (16 bytes ASCII)
                if (length >= 27)
                {
                    response.JobName = Encoding.ASCII.GetString(data, offset, 16).TrimEnd();
                    offset += 16;
                }

                // Byte 31: ETX (0x03)
                if (length > offset && data[offset] == 0x03)
                {
                    response.IsValid = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO Parse] {ex.Message}");
                return null;
            }
        }

        private PrinterState ParsePrinterState(char statusCode)
        {
            // Baseado na tabela de status da documentação
            if (statusCode >= '0' && statusCode <= '7')
                return PrinterState.Offline;
            else if ((statusCode >= 'A' && statusCode <= 'E') || (statusCode >= '!' && statusCode <= '$'))
                return PrinterState.Online;
            else if ((statusCode >= 'G' && statusCode <= 'K') || (statusCode >= '%' && statusCode <= '('))
                return PrinterState.Printing;
            else if ((statusCode >= 'M' && statusCode <= 'Q') || (statusCode >= ')' && statusCode <= ','))
                return PrinterState.Standby;
            else if ((statusCode >= 'S' && statusCode <= 'W') || (statusCode >= '-' && statusCode <= '@'))
                return PrinterState.Analyzing;
            else if (statusCode >= 'a' && statusCode <= 'q')
                return PrinterState.Error;
            else
                return PrinterState.Unknown;
        }

        private bool IsErrorStatus(char statusCode)
        {
            return statusCode >= 'a' && statusCode <= 'q';
        }

        private string GetStatusDescription(char statusCode)
        {
            switch (statusCode)
            {
                case '0': case 'A': case 'G': case 'M': case 'S':
                    return "Sem erros";
                case '1': case 'B': case 'H': case 'N': case 'T':
                    return "Ribbon/Etiqueta próximo do fim";
                case '2': case 'C': case 'I': case 'O': case 'U':
                    return "Buffer quase cheio";
                case '4': case 'E': case 'K': case 'Q': case 'W':
                    return "Impressão pausada";
                
                // Erros críticos
                case 'b': return "ERRO: Cabeça aberta";
                case 'c': return "ERRO: Papel acabou";
                case 'd': return "ERRO: Ribbon acabou";
                case 'e': return "ERRO: Erro de mídia";
                case 'f': return "ERRO: Erro de sensor";
                case 'g': return "ERRO: Erro na cabeça";
                case 'h': return "ERRO: Tampa/cortador aberto";
                case 'j': return "ERRO: Erro no cortador";
                case 'k': return "ERRO: Outros erros";

                default:
                    return $"Status: {statusCode}";
            }
        }

        public void Dispose()
        {
            Disconnect();
        }
    }

    // =================== Estruturas de Dados ===================

    public class Status4Response
    {
        public bool IsValid { get; set; }
        public string JobId { get; set; }
        public char StatusCode { get; set; }
        public PrinterState State { get; set; }
        public string StatusDescription { get; set; }
        public int RemainingLabels { get; set; }
        public string JobName { get; set; }
        public bool HasCriticalError { get; set; }

        public override string ToString()
        {
            return $"[{State}] {StatusDescription} | Restantes: {RemainingLabels} | Job: {JobName} ({JobId})";
        }
    }

    public enum PrinterState
    {
        Unknown,
        Offline,
        Online,
        Printing,
        Standby,
        Analyzing,
        Error
    }

    // =================== Exemplo de Uso ===================

    public class Status4TCPExample
    {
        public static void ExemploCompleto()
        {
            // Conecta na impressora
            var sato = new SatoStatus4TCP("192.168.1.100");
            
            if (!sato.Connect())
            {
                Console.WriteLine("Falha na conexão!");
                return;
            }

            try
            {
                // Verifica se está pronta
                Thread.Sleep(500); // Aguarda primeiro status
                
                if (!sato.IsPrinterReady())
                {
                    Console.WriteLine("Impressora não está pronta!");
                    return;
                }

                // Monta comando SBPL para imprimir 10 etiquetas
                int totalEtiquetas = 10;
                var sbplCommand = CriarComandoSBPL(totalEtiquetas);

                Console.WriteLine($"Enviando comando para imprimir {totalEtiquetas} etiquetas...");
                
                // Envia comando de impressão
                if (sato.SendPrintCommand(sbplCommand))
                {
                    Console.WriteLine("Comando enviado com sucesso!");
                    
                    // Aguarda 1 segundo para a impressora processar
                    Thread.Sleep(1000);
                    
                    // Solicita status
                    sato.RequestStatus();
                    Thread.Sleep(500);
                    
                    // Monitora o progresso
                    MonitorarProgresso(sato, totalEtiquetas, timeoutSegundos: 60);
                }
            }
            finally
            {
                sato.Disconnect();
            }
        }

        private static void MonitorarProgresso(SatoStatus4TCP sato, int totalEnviado, int timeoutSegundos)
        {
            var inicio = DateTime.Now;
            int ultimoRestante = -1;

            Console.WriteLine("\n=== MONITORANDO IMPRESSÃO ===");

            while ((DateTime.Now - inicio).TotalSeconds < timeoutSegundos)
            {
                var status = sato.GetLastStatus();
                
                if (status != null && status.IsValid)
                {
                    int restantes = status.RemainingLabels;
                    int impressas = totalEnviado - restantes;

                    if (restantes != ultimoRestante)
                    {
                        Console.WriteLine($"Impressas: {impressas}/{totalEnviado} | Restantes: {restantes} | Status: {status.StatusDescription}");
                        ultimoRestante = restantes;
                    }

                    if (restantes == 0)
                    {
                        Console.WriteLine("\n✅ IMPRESSÃO CONCLUÍDA!");
                        Console.WriteLine($"Total impresso: {totalEnviado} etiquetas");
                        return;
                    }

                    if (status.HasCriticalError)
                    {
                        Console.WriteLine($"\n❌ ERRO CRÍTICO: {status.StatusDescription}");
                        return;
                    }
                }

                Thread.Sleep(500);
            }

            Console.WriteLine("\n⏱️ TIMEOUT: Impressão não concluída no tempo esperado");
        }

        private static byte[] CriarComandoSBPL(int quantidade)
        {
            // Exemplo de comando SBPL simples
            var sbpl = new StringBuilder();
            
            sbpl.Append("\x1BA");           // <A> Início
            sbpl.Append("\x1BA1");          // <A1> Modo de impressão
            sbpl.Append("V00168H00264");    // Tamanho da etiqueta
            sbpl.Append("\x1BV50");         // Posição V
            sbpl.Append("\x1BH50");         // Posição H
            sbpl.Append("\x1BRD12345");     // Texto exemplo
            sbpl.Append($"\x1BQ{quantidade}"); // Quantidade
            sbpl.Append("\x1BZ");           // <Z> Fim

            return Encoding.ASCII.GetBytes(sbpl.ToString());
        }
    }
}
