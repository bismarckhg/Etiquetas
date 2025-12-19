using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.ConsoleUI
{
    /// <summary>
    /// Simulador de Pacote Sato para testes.
    /// </summary>
    public static class PacoteSatoSimulado
    {
        /// <summary>
        /// Cria um pacote Sato simulado para testes.
        /// </summary>
        public static byte[] CriarPacote(string jobName, string jobId, char status, long faltaImprimir)
        {
            var pacote = new byte[28];

            pacote[0] = 0x05; // ENQ
            pacote[1] = 0x02; // STXId (2 bytes)
            var jobIdBytes = Encoding.ASCII.GetBytes(jobId.PadRight(2));
            Array.Copy(jobIdBytes, 0, pacote, 2, 2);

            // Status (1 byte)
            pacote[4] = (byte)status;

            // Numero falta imprimir (6 bytes)
            var faltaStr = faltaImprimir.ToString("000000");
            var faltaBytes = Encoding.ASCII.GetBytes(faltaStr);
            Array.Copy(faltaBytes, 0, pacote, 5, 6);

            // JobName (16 bytes)
            var jobNameBytes = Encoding.ASCII.GetBytes(jobName.PadRight(16));
            Array.Copy(jobNameBytes, 0, pacote, 11, 16);

            pacote[27] = 0x03; // ETX

            return pacote;
        }
    }
}
