using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Extrai texto de uma linha entre marcadores de textos.
    /// </summary>
    public static class ExtrairTextoDaLinhaEntreMarcadoresDeTextos
    {
        /// <summary>
        /// Extrai texto de uma linha entre marcadores de textos, ^FD ... ^FS   ou   ^PQ...
        /// </summary>
        /// <param name="line"></param>
        /// <param name="marcadorFD"></param>
        /// <param name="marcadorFS"></param>
        /// <param name="comandoCopias"></param>
        /// <returns></returns>
        public static string Execute(string line, string marcadorFD, string marcadorFS, string comandoCopias)
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
}
