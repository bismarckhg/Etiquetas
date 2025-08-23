using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class DefineEncoding
    {
        public static Encoding Execute(string nomeEncoding)
        {
            var conversaoEncoding = Maiusculas.Execute(nomeEncoding);
            switch (conversaoEncoding)
            {
                case "ASCII":
                    return Encoding.ASCII;
                case "UTF-8":
                case "UTF8":
                    return  Encoding.UTF8;
                case "UTF-16":
                case "UTF16":
                case "UNICODE":
                    return Encoding.Unicode;
                case "ISO-8859-1":
                case "LATIN-1":
                    return Encoding.GetEncoding("iso-8859-1");
                case "WINDOWS1252":
                case "WINDOWS-1252":
                case "1252":
                    return  Encoding.GetEncoding(1252);
                case "DOS LATIN-1":
                case "MSDOS LATIN-1":
                case "850":
                    return Encoding.GetEncoding(850);
                case "MSDOSUS":
                case "MSDOS-US":
                case "MSDOS US":
                case "DOSUS":
                case "DOS-US":
                case "DOS US":
                case "437":
                    return Encoding.GetEncoding(850);
                default:
                    return Encoding.UTF8;
            }
        }
    }
}
