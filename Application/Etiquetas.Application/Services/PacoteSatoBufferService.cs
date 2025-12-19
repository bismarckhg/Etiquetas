using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Etiqueta.Application.DTOs;
using Etiquetas.Core.Constants;

namespace Etiquetas.Application.Services
{
    /// <summary>
    /// Serviço responsável por armazenar pacotes incompletos e montá-los quando completos.
    /// </summary>
    public class PacoteSatoBufferService
    {
        // Buffer de bytes pendentes (por origem/conexão)
        private readonly ConcurrentDictionary<string, List<byte>> _buffersPendentes;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacoteSatoBufferService"/> class.
        /// </summary>
        public PacoteSatoBufferService()
        {
            _buffersPendentes = new ConcurrentDictionary<string, List<byte>>();
        }

        /// <summary>
        /// Processa bytes recebidos, retornando pacotes completos encontrados.
        /// Armazena bytes incompletos no buffer para processamento futuro.
        /// </summary>
        /// <param name="origem">Identificador da origem (ex: "Porta2", "TCP:192.168.1.100").</param>
        /// <param name="bytesRecebidos">Array de bytes recebidos da impressora.</param>
        /// <returns>Lista de pacotes completos prontos para parsing.</returns>
        public List<byte[]> ProcessarBytes(string origem, byte[] bytesRecebidos)
        {
            if (bytesRecebidos == null || bytesRecebidos.Length == 0)
            {
                return new List<byte[]>();
            }

            var pacotesCompletos = new List<byte[]>();

            // Obter ou criar buffer para esta origem
            var buffer = _buffersPendentes.GetOrAdd(origem, _ => new List<byte>());

            lock (buffer)
            {
                // Adicionar novos bytes ao buffer
                buffer.AddRange(bytesRecebidos);

                // Tentar extrair pacotes completos
                while (true)
                {
                    var pacote = ExtrairPacoteCompleto(buffer);
                    if (pacote == null)
                    {
                        break; // Não há mais pacotes completos
                    }

                    pacotesCompletos.Add(pacote);
                }
            }

            return pacotesCompletos;
        }

        /// <summary>
        /// Tenta extrair um pacote completo do buffer.
        /// Remove os bytes do buffer se encontrar um pacote válido.
        /// </summary>
        /// <param name="buffer">Buffer de bytes pendentes.</param>
        /// <returns>Array de bytes do pacote completo, ou null se não houver pacote completo.</returns>
        private byte[] ExtrairPacoteCompleto(List<byte> buffer)
        {
            if (buffer.Count < PadraoConstantes.ConstTamanhoProtocolo)
            {
                return null; // Não há bytes suficientes
            }

            // Procurar STX
            int posENQ = buffer.IndexOf(PadraoConstantes.ConstENQ);
            if (posENQ == -1)
            {
                // Não encontrou ENQ, descartar até encontrar ou limpar buffer antigo
                if (buffer.Count > 100)
                {
                    Console.WriteLine($"[BUFFER] Descartando {buffer.Count} bytes sem STX");
                    buffer.Clear();
                }

                return null;
            }

            // Descartar bytes antes do ENQ
            if (posENQ > 0)
            {
                Console.WriteLine($"[BUFFER] Descartando {posENQ} bytes antes do ENQ");
                buffer.RemoveRange(0, posENQ);
                posENQ = 0;
            }

            // Verificar se há bytes suficientes após o STX
            if (buffer.Count < PadraoConstantes.ConstTamanhoProtocolo)
            {
                return null; // Aguardar mais bytes
            }

            // Procurar ETX na posição esperada
            int posETX = posENQ + (PadraoConstantes.ConstTamanhoProtocolo - 1);
            if (posETX >= buffer.Count)
            {
                return null; // Aguardar mais bytes
            }

            if (buffer[posETX] != PadraoConstantes.ConstETX)
            {
                // ETX não está na posição esperada, procurar o próximo ENQ
                Console.WriteLine($"[BUFFER] ETX não encontrado na posição {posETX}, byte={buffer[posETX]:X2}");
                buffer.RemoveAt(0); // Remove o STX inválido
                return null;
            }

            // Pacote completo encontrado
            var pacoteCompleto = buffer.GetRange(posENQ, PadraoConstantes.ConstTamanhoProtocolo).ToArray();
            buffer.RemoveRange(0, PadraoConstantes.ConstTamanhoProtocolo);

            Console.WriteLine($"[BUFFER] Pacote completo extraído: {PadraoConstantes.ConstTamanhoProtocolo} bytes");
            return pacoteCompleto;
        }

        /// <summary>
        /// Limpa o buffer de uma origem específica.
        /// </summary>
        /// <param name="origem">Identificador da origem.</param>
        public void LimparBuffer(string origem)
        {
            if (_buffersPendentes.TryRemove(origem, out var buffer))
            {
                Console.WriteLine($"[BUFFER] Buffer de '{origem}' limpo: {buffer.Count} bytes descartados");
            }
        }

        /// <summary>
        /// Retorna informações sobre os buffers pendentes.
        /// </summary>
        /// <returns>Dicionário com origem e quantidade de bytes pendentes.</returns>
        public Dictionary<string, int> ObterStatusBuffers()
        {
            return _buffersPendentes.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Count
            );
        }
    }
}
