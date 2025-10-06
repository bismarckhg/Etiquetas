# SATO CL4NX - Status4 TCP Demo

Demonstração completa do protocolo Status4 via TCP (Portas 1024/1025) para impressoras SATO CL4NX/CL6NX.

## 📋 Funcionalidades

- ✅ Conexão dual-port (1024 para impressão, 1025 para status)
- ✅ Monitoramento em tempo real de etiquetas impressas
- ✅ Cálculo automático: `Impressas = Total Enviado - Restantes`
- ✅ Detecção de erros críticos
- ✅ Interface interativa com menu
- ✅ Barra de progresso visual
- ✅ Suporte a SBPL e SZPL

## 🔧 Compilação

### Opção 1: Visual Studio
1. Abra o arquivo `SatoStatus4Demo.csproj` no Visual Studio
2. Compile (F6) ou execute (F5)

### Opção 2: Linha de Comando (MSBuild)
```cmd
msbuild SatoStatus4Demo.csproj /p:Configuration=Release
```

### Opção 3: Compilador C# Direto
```cmd
csc /target:exe /out:SatoStatus4Demo.exe /langversion:7.3 SatoStatus4TCP.cs SatoStatus4Demo.cs
```

## 🚀 Execução

```cmd
SatoStatus4Demo.exe
```

O programa solicitará o IP da impressora e apresentará um menu interativo:

```
╔════════════════════════════════════════════════════╗
║                    MENU PRINCIPAL                  ║
╠════════════════════════════════════════════════════╣
║  1 - Verificar Status Atual                        ║
║  2 - Imprimir Etiquetas de Teste (SBPL Simples)   ║
║  3 - Monitoramento Contínuo                        ║
║  4 - Solicitar Status Manual (ENQ)                 ║
║  5 - Imprimir com Texto TTF                        ║
║  0 - Sair                                          ║
╚════════════════════════════════════════════════════╝
```

## ⚙️ Configuração da Impressora

Certifique-se de que a impressora SATO está configurada com:

### Via Interface Web ou Painel:
1. **Protocol**: Status4 (ENQ) ou Status4 (Cycle)
2. **Port 1**: 1024
3. **Port 2**: 1025
4. **Legacy Status**: Desabilitado
5. **IP fixo**: Configure um IP estático

### Via Comandos SBPL:
```
<ESC>A
<ESC>CS4  (Habilita Status4)
<ESC>Z
```

## 📊 Exemplo de Uso

### 1. Verificar Status
```
Estado:              Online
Descrição:           Sem erros
Código Status:       'A' (0x41)
Etiquetas Restantes: 0
Job ID:              01
Job Name:            TESTE_JOB
Erro Crítico:        NÃO ✅
```

### 2. Monitorar Impressão
```
[████████████████████] 100% | Impressas: 50/50 | Restantes: 0

✅ IMPRESSÃO CONCLUÍDA!
Total impresso: 50 etiquetas
Tempo decorrido: 12.3s
```

## 🔍 Estrutura do Projeto

```
SatoStatus4TCP.cs       → Classe principal do protocolo Status4
SatoStatus4Demo.cs      → Programa de demonstração interativo
SatoStatus4Demo.csproj  → Arquivo de projeto .NET 4.7.2
README_Status4.md       → Este arquivo
```

## 📡 Protocolo Status4

### Formato da Resposta (32 bytes):
```
[4 bytes: size] [1: ENQ] [1: STX] [2: JobID] [1: Status] 
[6: Remaining] [16: JobName] [1: ETX]
```

### Exemplo Real:
```
0000001C 05 02 30 31 41 303030303235 54455354455F4A4F42 03
         |  |  ^^^^^ ^  ^^^^^^^^^^^^ ^^^^^^^^^^^^^^^^ ^
         |  |  JobID |  Remaining   Job Name         ETX
         |  STX      Status='A'
         ENQ
```

## 🎯 Integração com TTF

Para usar com renderização de fontes TrueType, integre com `SatoTtfPrinter.cs`:

```csharp
// Renderiza texto em bitmap
var bitmap = SatoTtfPrinter.RenderTextToBitmap("TESTE", "Arial", 10);

// Converte para comando <G>
var comandoG = SatoTtfPrinter.ConvertToGraphicCommand(bitmap, x, y);

// Monta SBPL completo
var sbpl = $"\x1BA\x1BA1V00168H00264{comandoG}\x1BQ1\x1BZ";

// Envia via TCP
sato.SendPrintCommand(Encoding.ASCII.GetBytes(sbpl));
```

## ⚠️ Troubleshooting

### Erro: "Falha na conexão"
- Verifique se o IP está correto
- Teste ping: `ping 192.168.1.100`
- Verifique firewall (portas 1024/1025)
- Certifique-se de que Status4 está habilitado

### Erro: "Nenhum status recebido"
- Aguarde alguns segundos após conectar
- Use opção 4 (Solicitar Status Manual)
- Verifique se o modo é Status4 (não Status3 ou Status5)

### Etiquetas não imprimem
- Verifique se `IsPrinterReady()` retorna `true`
- Confira se há papel/ribbon
- Verifique se não há erro crítico no status

## 📚 Referências

- Manual: CL4NX/CL6NX Programming Reference (Seção Socket Communication)
- Protocolo: Status4 (páginas 630-663)
- Portas TCP: 1024 (print), 1025 (status), 9100 (combined)

## 📝 Licença

Código de exemplo para uso livre.

---

**Desenvolvido para SATO CL4NX 203 dpi**  
Compatível com: CL4NX, CL6NX, e modelos similares que suportam Status4
