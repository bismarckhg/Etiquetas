using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class ConverteByteEmStringUTF8
    {
        public static string Execute(byte[] bytes)
        {
            if (bytes == null)
            {
                return string.Empty;
            }
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
