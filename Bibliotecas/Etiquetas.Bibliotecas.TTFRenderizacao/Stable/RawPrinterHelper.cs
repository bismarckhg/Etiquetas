using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TTFRenderizacao.Stable
{
    internal static class RawPrinterHelper
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)] public string pDatatype;
        }

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In] DOCINFOA di);

        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", SetLastError = true)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

        public static bool SendBytesToPrinter(string printerName, byte[] bytes)
        {
            if (string.IsNullOrWhiteSpace(printerName))
                throw new ArgumentException("Nome da impressora não pode ser vazio.");

            if (bytes == null || bytes.Length == 0)
                return false;

            if (!OpenPrinter(printerName, out var h, IntPtr.Zero))
                throw new InvalidOperationException($"Não foi possível abrir a impressora: {printerName}");

            var di = new DOCINFOA
            {
                pDocName = "SBPL TTF Print Job",
                pDatatype = "RAW"
            };

            try
            {
                if (!StartDocPrinter(h, 1, di))
                    throw new InvalidOperationException("StartDocPrinter falhou.");

                if (!StartPagePrinter(h))
                    throw new InvalidOperationException("StartPagePrinter falhou.");

                IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
                try
                {
                    Marshal.Copy(bytes, 0, ptr, bytes.Length);
                    if (!WritePrinter(h, ptr, bytes.Length, out int written))
                        throw new InvalidOperationException("WritePrinter falhou.");
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }

                EndPagePrinter(h);
                EndDocPrinter(h);
                return true;
            }
            finally
            {
                ClosePrinter(h);
            }
        }
    }

}
