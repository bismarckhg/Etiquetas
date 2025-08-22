using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Etiquetas.Bibliotecas.COLibIdioma
{
    public static class Cultura
    {
        public static CultureInfo Padrao { get; } = CultureInfo.CurrentCulture;
        public static int CodePage { get; } = Padrao.TextInfo.ANSICodePage;
        public static Encoding EncodingIdioma { get; } = Encoding.GetEncoding(CodePage);
        public static Encoding EncodingPadrao { get; } = Encoding.UTF8;
    }
}
