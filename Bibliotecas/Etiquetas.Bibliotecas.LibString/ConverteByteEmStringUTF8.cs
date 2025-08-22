using System.Text;

namespace Etiquetas.Bibliotecas.LibString
{
    public static class ConverteByteEmStringUTF8
    {
        public static string Execute(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
