using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class LinhaStringBuilder
    {
        public static string Execute(StringBuilder texto, int linhaTexto)
        {
            if (texto == null || linhaTexto < 1)
            {
                return string.Empty;
            }

            string[] lines = texto.ToString().Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);

            if (linhaTexto > lines.Length)
            {
                return string.Empty;
            }

            return lines[linhaTexto - 1];
        }
    }
}
