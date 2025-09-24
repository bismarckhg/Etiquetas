using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Geral
{
    public static class ConversaoEncoding
    {
        public static Encoding UTF8() => new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        // Encoding ANSI pt-BR (Windows-1252) com exceção em bytes inválidos
        public static Encoding Windows1252() => Encoding.GetEncoding
            (
                1252,
                EncoderFallback.ExceptionFallback,
                DecoderFallback.ExceptionFallback
            );
        // UTF-8 sem BOM (recomendado p/ JSON, ZPL etc.)
        public static Encoding UTF8SemBom() => new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);


    }
}
