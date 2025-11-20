using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Geral
{
    public static class ConversaoEncoding
    {
        /// <summary>
        /// Encoding UTF-8 com BOM (Byte Order Mark).
        /// </summary>
        public static readonly Encoding UTF8BOM = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true);

        /// <summary>
        /// Encoding ANSI pt-BR (Windows-1252) com exceção em bytes inválidos
        /// </summary>
        public static readonly Encoding Windows1252 = Encoding.GetEncoding
            (
                1252,
                EncoderFallback.ExceptionFallback,
                DecoderFallback.ExceptionFallback
            );

        /// <summary>
        /// UTF-8 sem BOM (recomendado p/ JSON, ZPL etc.)
        /// </summary>
        public static readonly Encoding UTF8SemBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    }
}
