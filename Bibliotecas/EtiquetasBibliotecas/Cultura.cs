using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas
{
    public static class Cultura
    {
        public static CultureInfo Padrao { get; } = CultureInfo.CurrentCulture;
        public static int CodePage { get; } = Padrao.TextInfo.ANSICodePage;
        public static Encoding EncodingIdioma { get; } = Encoding.GetEncoding(CodePage);
        public static Encoding EncodingPadrao { get; } = Encoding.UTF8;
    }
}
