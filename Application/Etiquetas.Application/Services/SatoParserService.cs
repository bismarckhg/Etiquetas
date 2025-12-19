using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Etiqueta.Application.DTOs;
using Etiquetas.Core;

namespace Etiquetas.Application.Services
{
    /// <summary>
    /// Implementação do serviço de parser Sato.
    /// </summary>
    public class SatoParserService : ISatoParserService
    {
        private const byte ConstENQ = 0x05;
        private const byte ConstSTX = 0x02;
        private const byte ConstETX = 0x03;

        /// <summary>
        /// Faz o parser do pacote Sato e retorna uma lista de DTOs.
        /// </summary>
        /// <param name="package">Pacotes lido.</param>
        /// <returns>Coleção de DTO de Sato.</returns>
        public List<ISatoDto> Parse(byte[] package)
        {
            var result = new List<ISatoDto>();
            if (package == null || package.Length == 0)
            {
                return result;
            }

            var frags = SplitKeepTerminator(package, ConstETX);
            foreach (var frag in frags)
            {
                var dto = ParseFragment(frag);
                if (dto != null)
                {
                    result.Add(dto);
                }
            }

            return result;
        }

        private static SatoDto ParseFragment(byte[] frag)
        {
            if (frag == null || frag.Length == 0)
            {
                return null;
            }

            var dto = new SatoDto();
            int cursor = Array.IndexOf(frag, ConstSTX);

            SafeCopy(frag, ref cursor, dto.RetornoImpressao, dto.RetornoImpressao.Length);

            return dto;
        }

        private static byte SafeGet(byte[] arr, int idx)
        {
            if (arr == null || idx < 0 || idx >= arr.Length)
            {
                return 0;
            }

            return arr[idx];
        }

        private static void SafeCopy(byte[] src, ref int cursor, byte[] dest, int count)
        {
            if (dest == null)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                dest[i] = SafeGet(src, cursor++);
            }
        }

        private static IEnumerable<byte[]> SplitKeepTerminator(byte[] src, byte etx)
        {
            if (src == null)
            {
                yield break;
            }
  
            var buffer = new List<byte>();

            foreach (var b in src)
            {
                buffer.Add(b);

                if (b == etx)
                {
                    yield return buffer.ToArray();
                    buffer.Clear();
                }
            }

            // Retorna o último bloco caso não termine com divisor
            if (buffer.Count > 0)
            {
                yield return buffer.ToArray();
            }
        }
    }
}
