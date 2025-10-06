using System;
using System.Text;
using System.Threading;

namespace SatoTtfPrinter
{
    /// <summary>
    /// Programa de demonstração completo do Status4 TCP
    /// </summary>
    class SatoStatus4Demo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║   SATO CL4NX - Status4 TCP Demo (Portas 1024/1025) ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Solicita IP da impressora
            Console.Write("Digite o IP da impressora SATO [192.168.1.100]: ");
            string ip = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ip))
                ip = "192.168.1.100";

            // Cria instância e conecta
            var sato = new SatoStatus4TCP(ip);
            
            Console.WriteLine($"\n[*] Conectando em {ip}:1024/1025...");
            
            if (!sato.Connect())
            {
                Console.WriteLine("\n❌ FALHA NA CONEXÃO!");
                Console.WriteLine("Verifique:");
                Console.WriteLine("  - IP está correto");
                Console.WriteLine("  - Impressora está ligada");
                Console.WriteLine("  - Portas 1024/1025 estão abertas");
                Console.WriteLine("  - Status4 está habilitado na impressora");
                Console.WriteLine("\nPressione qualquer tecla para sair...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("✅ CONECTADO COM SUCESSO!\n");
            Thread.Sleep(1000); // Aguarda primeiro status

            try
            {
                bool continuar = true;
                while (continuar)
                {
                    MostrarMenu();
                    string opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            VerificarStatus(sato);
                            break;
                        case "2":
                            ImprimirEtiquetasTeste(sato);
                            break;
                        case "3":
                            MonitorarContinuo(sato);
                            break;
                        case "4":
                            SolicitarStatusManual(sato);
                            break;
                        case "5":
                            ImprimirComTextoTTF(sato);
                            break;
                        case "0":
                            continuar = false;
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            break;
                    }

                    if (continuar)
                    {
                        Console.WriteLine("\nPressione qualquer tecla para continuar...");
                        Console.ReadKey();
                    }
                }
            }
            finally
            {
                Console.WriteLine("\n[*] Desconectando...");
                sato.Disconnect();
                Console.WriteLine("✅ Desconectado. Até logo!");
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    MENU PRINCIPAL                  ║");
            Console.WriteLine("╠════════════════════════════════════════════════════╣");
            Console.WriteLine("║  1 - Verificar Status Atual                        ║");
            Console.WriteLine("║  2 - Imprimir Etiquetas de Teste (SBPL Simples)   ║");
            Console.WriteLine("║  3 - Monitoramento Contínuo                        ║");
            Console.WriteLine("║  4 - Solicitar Status Manual (ENQ)                 ║");
            Console.WriteLine("║  5 - Imprimir com Texto TTF                        ║");
            Console.WriteLine("║  0 - Sair                                          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.Write("\nEscolha uma opção: ");
        }

        static void VerificarStatus(SatoStatus4TCP sato)
        {
            Console.Clear();
            Console.WriteLine("═══ VERIFICAR STATUS ATUAL ═══\n");

            var status = sato.GetLastStatus();
            
            if (status == null || !status.IsValid)
            {
                Console.WriteLine("⚠️ Nenhum status recebido ainda.");
                Console.WriteLine("Aguarde alguns segundos ou solicite manualmente (opção 4).");
                return;
            }

            Console.WriteLine($"Estado:              {status.State}");
            Console.WriteLine($"Descrição:           {status.StatusDescription}");
            Console.WriteLine($"Código Status:       '{status.StatusCode}' (0x{((int)status.StatusCode):X2})");
            Console.WriteLine($"Etiquetas Restantes: {status.RemainingLabels}");
            Console.WriteLine($"Job ID:              {status.JobId}");
            Console.WriteLine($"Job Name:            {status.JobName}");
            Console.WriteLine($"Erro Crítico:        {(status.HasCriticalError ? "SIM ❌" : "NÃO ✅")}");
            
            Console.WriteLine();
            
            if (sato.IsPrinterReady())
            {
                Console.WriteLine("✅ Impressora PRONTA para imprimir!");
            }
            else
            {
                Console.WriteLine("❌ Impressora NÃO está pronta!");
            }
        }

        static void ImprimirEtiquetasTeste(SatoStatus4TCP sato)
        {
            Console.Clear();
            Console.WriteLine("═══ IMPRIMIR ETIQUETAS DE TESTE ═══\n");

            if (!sato.IsPrinterReady())
            {
                Console.WriteLine("❌ Impressora não está pronta!");
                return;
            }

            Console.Write("Quantas etiquetas deseja imprimir? [5]: ");
            string input = Console.ReadLine();
            int quantidade = 5;
            if (!string.IsNullOrWhiteSpace(input))
            {
                if (!int.TryParse(input, out quantidade) || quantidade <= 0)
                {
                    Console.WriteLine("Quantidade inválida!");
                    return;
                }
            }

            Console.WriteLine($"\n[*] Preparando comando SBPL para {quantidade} etiquetas...");
            
            var sbplCommand = CriarComandoSBPLTeste(quantidade);
            
            Console.WriteLine("[*] Enviando comando...");
            
            if (sato.SendPrintCommand(sbplCommand))
            {
                Console.WriteLine("✅ Comando enviado com sucesso!");
                Console.WriteLine("\n[*] Aguardando processamento...");
                Thread.Sleep(1500);
                
                sato.RequestStatus();
                Thread.Sleep(500);
                
                Console.WriteLine("\n═══ MONITORANDO PROGRESSO ═══\n");
                MonitorarProgressoImpressao(sato, quantidade, 120);
            }
            else
            {
                Console.WriteLine("❌ Falha ao enviar comando!");
            }
        }

        static void MonitorarContinuo(SatoStatus4TCP sato)
        {
            Console.Clear();
            Console.WriteLine("═══ MONITORAMENTO CONTÍNUO ═══");
            Console.WriteLine("Pressione ESC para parar\n");

            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                    break;

                var status = sato.GetLastStatus();
                
                if (status != null && status.IsValid)
                {
                    Console.SetCursorPosition(0, 3);
                    Console.WriteLine($"Estado:      {status.State,-15} | Status: {status.StatusDescription,-40}");
                    Console.WriteLine($"Restantes:   {status.RemainingLabels,-6} | Job: {status.JobName,-16} ({status.JobId})");
                    Console.WriteLine($"Pronta:      {(sato.IsPrinterReady() ? "SIM ✅" : "NÃO ❌"),-15}");
                    Console.WriteLine($"Atualizado:  {DateTime.Now:HH:mm:ss}");
                }
                else
                {
                    Console.SetCursorPosition(0, 3);
                    Console.WriteLine("Aguardando status...");
                }

                Thread.Sleep(500);
            }

            Console.WriteLine("\n\n[*] Monitoramento interrompido.");
        }

        static void SolicitarStatusManual(SatoStatus4TCP sato)
        {
            Console.Clear();
            Console.WriteLine("═══ SOLICITAR STATUS MANUAL (ENQ) ═══\n");

            Console.WriteLine("[*] Enviando comando ENQ (0x05)...");
            
            if (sato.RequestStatus())
            {
                Console.WriteLine("✅ Comando ENQ enviado!");
                Console.WriteLine("[*] Aguardando resposta...");
                
                Thread.Sleep(1000);
                
                var status = sato.GetLastStatus();
                
                if (status != null && status.IsValid)
                {
                    Console.WriteLine("\n✅ Status recebido:");
                    Console.WriteLine($"  Estado: {status.State}");
                    Console.WriteLine($"  Descrição: {status.StatusDescription}");
                    Console.WriteLine($"  Restantes: {status.RemainingLabels}");
                }
                else
                {
                    Console.WriteLine("\n⚠️ Nenhuma resposta recebida.");
                }
            }
            else
            {
                Console.WriteLine("❌ Falha ao enviar ENQ!");
            }
        }

        static void ImprimirComTextoTTF(SatoStatus4TCP sato)
        {
            Console.Clear();
            Console.WriteLine("═══ IMPRIMIR COM TEXTO TTF ═══\n");

            if (!sato.IsPrinterReady())
            {
                Console.WriteLine("❌ Impressora não está pronta!");
                return;
            }

            Console.Write("Digite o texto a imprimir [TESTE 123]: ");
            string texto = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(texto))
                texto = "TESTE 123";

            Console.Write("Quantas etiquetas? [1]: ");
            string input = Console.ReadLine();
            int quantidade = 1;
            if (!string.IsNullOrWhiteSpace(input))
            {
                if (!int.TryParse(input, out quantidade) || quantidade <= 0)
                    quantidade = 1;
            }

            Console.WriteLine($"\n[*] Preparando etiqueta com texto: \"{texto}\"");
            
            // Aqui você integraria com o SatoTtfPrinter para renderizar TTF
            // Por enquanto, vamos usar comando RD simples
            var sbplCommand = CriarComandoSBPLComTexto(texto, quantidade);
            
            Console.WriteLine("[*] Enviando comando...");
            
            if (sato.SendPrintCommand(sbplCommand))
            {
                Console.WriteLine("✅ Comando enviado!");
                Thread.Sleep(1500);
                sato.RequestStatus();
                Thread.Sleep(500);
                
                Console.WriteLine("\n═══ MONITORANDO PROGRESSO ═══\n");
                MonitorarProgressoImpressao(sato, quantidade, 60);
            }
            else
            {
                Console.WriteLine("❌ Falha ao enviar comando!");
            }
        }

        // =================== Funções Auxiliares ===================

        static void MonitorarProgressoImpressao(SatoStatus4TCP sato, int totalEnviado, int timeoutSegundos)
        {
            var inicio = DateTime.Now;
            int ultimoRestante = -1;

            while ((DateTime.Now - inicio).TotalSeconds < timeoutSegundos)
            {
                var status = sato.GetLastStatus();
                
                if (status != null && status.IsValid)
                {
                    int restantes = status.RemainingLabels;
                    int impressas = totalEnviado - restantes;

                    if (restantes != ultimoRestante)
                    {
                        // Barra de progresso
                        int percentual = (int)((impressas / (double)totalEnviado) * 100);
                        string barra = new string('█', percentual / 5) + new string('░', 20 - percentual / 5);
                        
                        Console.WriteLine($"[{barra}] {percentual}% | Impressas: {impressas}/{totalEnviado} | Restantes: {restantes}");
                        
                        ultimoRestante = restantes;
                    }

                    if (restantes == 0)
                    {
                        Console.WriteLine("\n✅ IMPRESSÃO CONCLUÍDA!");
                        Console.WriteLine($"Total impresso: {totalEnviado} etiquetas");
                        Console.WriteLine($"Tempo decorrido: {(DateTime.Now - inicio).TotalSeconds:F1}s");
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

        static byte[] CriarComandoSBPLTeste(int quantidade)
        {
            var sbpl = new StringBuilder();
            
            sbpl.Append("\x1BA");                    // <A> Início
            sbpl.Append("\x1BA1");                   // <A1> Modo padrão
            sbpl.Append("V00168H00264");             // Tamanho etiqueta (33x21mm em 203dpi)
            
            // Texto de teste
            sbpl.Append("\x1BV50");                  // Posição V
            sbpl.Append("\x1BH50");                  // Posição H
            sbpl.Append("\x1BL0202");                // Fonte L02 tamanho 02
            sbpl.Append("\x1BRDEtiqueta Teste");     // Texto
            
            // Número sequencial
            sbpl.Append("\x1BV80");
            sbpl.Append("\x1BH50");
            sbpl.Append("\x1BL0303");
            sbpl.Append("\x1BRD#");
            sbpl.Append("\x1B+100,1,1");             // Contador incremental
            
            // Quantidade e fim
            sbpl.Append($"\x1BQ{quantidade}");       // Quantidade
            sbpl.Append("\x1BZ");                    // <Z> Fim

            return Encoding.ASCII.GetBytes(sbpl.ToString());
        }

        static byte[] CriarComandoSBPLComTexto(string texto, int quantidade)
        {
            var sbpl = new StringBuilder();
            
            sbpl.Append("\x1BA");
            sbpl.Append("\x1BA1");
            sbpl.Append("V00168H00264");
            
            sbpl.Append("\x1BV60");
            sbpl.Append("\x1BH40");
            sbpl.Append("\x1BL0303");
            sbpl.Append($"\x1BRD{texto}");
            
            sbpl.Append($"\x1BQ{quantidade}");
            sbpl.Append("\x1BZ");

            return Encoding.ASCII.GetBytes(sbpl.ToString());
        }
    }
}
