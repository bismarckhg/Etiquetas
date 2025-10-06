using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Etiquetas.Bibliotecas.TTFRenderizacao.Stable
{
    /// <summary>
    /// Versão CORRIGIDA - Renderização otimizada para impressoras térmicas
    /// .NET Framework 4.7.2, C# 7.3
    /// </summary>
    public static class SatoTtfPrinter
    {
        // ===== CONFIGURAÇÕES =====
        private const int DPI_X = 203;
        private const int DPI_Y = 203;
        private const bool INVERT_BIT_ORDER = false;
        public const bool DEBUG_MODE = true; // Ativa logs de debug (público para acesso em outras classes)
        // =========================

        public static bool PrintTextAsGraphic(
            string printerName,
            string text,
            string fontName,
            float fontSizePt,
            bool isBold,
            int positionX_dots,
            int positionY_dots,
            double labelWidthMm,
            double labelHeightMm)
        {
            try
            {
                if (DEBUG_MODE)
                    Console.WriteLine($"[DEBUG] Iniciando impressão: '{text}' em {fontName} {fontSizePt}pt");

                var job = new List<byte>();

                // Início da etiqueta
                job.AddRange(BeginLabel(labelWidthMm, labelHeightMm));

                // Renderiza o texto em bitmap 1bpp
                var bitmap = GdiTextRenderer.RenderTo1bpp(text, fontName, fontSizePt, isBold, DPI_X, DPI_Y);

                if (DEBUG_MODE)
                    Console.WriteLine($"[DEBUG] Bitmap gerado: {bitmap.Width}×{bitmap.Height} pixels");

                // Converte para comando <G> SBPL
                var graphicCmd = SbplConverter.CreateGraphicCommandFromBitmap(bitmap, INVERT_BIT_ORDER);

                if (DEBUG_MODE)
                    Console.WriteLine($"[DEBUG] Comando <G> gerado com {graphicCmd.Length} bytes");

                // Posiciona e adiciona o gráfico
                job.AddRange(PositionCommand(positionX_dots, positionY_dots));
                job.AddRange(graphicCmd);

                // Finaliza a etiqueta
                job.AddRange(EndLabel(1));

                // Envia para o spooler
                bool result = RawPrinterHelper.SendBytesToPrinter(printerName, job.ToArray());

                if (DEBUG_MODE)
                    Console.WriteLine($"[DEBUG] Resultado do envio: {(result ? "SUCESSO" : "FALHA")}");

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ERRO] {ex.Message}");
                if (DEBUG_MODE)
                    Console.Error.WriteLine($"[STACK] {ex.StackTrace}");
                return false;
            }
        }

        private static byte[] BeginLabel(double widthMm, double heightMm)
        {
            var cmd = new List<byte>();
            int widthDots = MmToDots(widthMm, DPI_X);
            int heightDots = MmToDots(heightMm, DPI_Y);

            AddEsc(cmd, "A");
            AddEsc(cmd, "A1");
            AddAscii(cmd, "V");
            AddAscii(cmd, heightDots.ToString("00000"));
            AddAscii(cmd, "H");
            AddAscii(cmd, widthDots.ToString("000"));

            return cmd.ToArray();
        }

        private static byte[] PositionCommand(int x, int y)
        {
            var cmd = new List<byte>();
            AddEsc(cmd, "V");
            AddAscii(cmd, y.ToString());
            AddEsc(cmd, "H");
            AddAscii(cmd, x.ToString());
            return cmd.ToArray();
        }

        private static byte[] EndLabel(int quantity)
        {
            var cmd = new List<byte>();
            AddEsc(cmd, "Q");
            AddAscii(cmd, quantity.ToString());
            AddEsc(cmd, "Z");
            return cmd.ToArray();
        }

        private static void AddEsc(List<byte> buffer, string text)
        {
            buffer.Add(0x1B);
            AddAscii(buffer, text);
        }

        private static void AddAscii(List<byte> buffer, string text)
        {
            buffer.AddRange(Encoding.ASCII.GetBytes(text));
        }

        private static int MmToDots(double mm, int dpi)
        {
            return (int)Math.Round(mm * dpi / 25.4);
        }
    }

    internal static class GdiTextRenderer
    {
        internal struct MonoBitmap
        {
            public int Width;
            public int Height;
            public int StrideBytes;
            public byte[] Bits;
        }

        public static MonoBitmap RenderTo1bpp(
            string text,
            string fontName,
            float points,
            bool bold,
            int dpiX,
            int dpiY)
        {
            IntPtr hdc = GDI.CreateCompatibleDC(IntPtr.Zero);
            if (hdc == IntPtr.Zero)
                throw new Exception("CreateCompatibleDC falhou.");

            try
            {
                // Calcula altura em pixels
                int pxHeight = (int)Math.Round(points * dpiY / 72.0);

                // Cria fonte com configurações otimizadas para impressora térmica
                var lf = new GDI.LOGFONTW
                {
                    lfHeight = -pxHeight,
                    lfWeight = bold ? 700 : 400,
                    lfCharSet = 1, // DEFAULT_CHARSET
                    lfOutPrecision = 3, // OUT_TT_PRECIS (TrueType)
                    lfClipPrecision = 2, // CLIP_DEFAULT_PRECIS
                    lfQuality = 3, // NONANTIALIASED_QUALITY (SEM anti-aliasing!)
                    lfPitchAndFamily = 0,
                    lfFaceName = fontName ?? "Arial"
                };

                IntPtr hFont = GDI.CreateFontIndirectW(ref lf);
                if (hFont == IntPtr.Zero)
                {
                    GDI.DeleteDC(hdc);
                    throw new Exception("CreateFontIndirectW falhou.");
                }

                IntPtr hFontOld = GDI.SelectObject(hdc, hFont);
                try
                {
                    // Mede o texto
                    GDI.SIZE size;
                    if (!GDI.GetTextExtentPoint32W(hdc, text, text.Length, out size))
                        throw new Exception("GetTextExtentPoint32W falhou.");

                    int width = Math.Max(1, size.cx);
                    int height = Math.Max(1, size.cy);

                    if (SatoTtfPrinter.DEBUG_MODE)
                        Console.WriteLine($"[DEBUG] Tamanho medido: {width}×{height} px");

                    // Cria DIB 1bpp
                    var dib = DIBSection.Create1bppTopDown(hdc, width, height, dpiX, dpiY);

                    // Preenche com branco (0x00 = branco em 1bpp quando bit 0 da paleta é branco)
                    Array.Clear(dib.ManagedBits, 0, dib.ManagedBits.Length);

                    IntPtr hbmOld = GDI.SelectObject(hdc, dib.HBitmap);
                    try
                    {
                        // Configura modo de desenho
                        GDI.SetBkMode(hdc, 1); // TRANSPARENT
                        GDI.SetTextColor(hdc, 0x000000); // Preto
                        GDI.SetBkColor(hdc, 0xFFFFFF); // Branco

                        // Desenha o texto
                        var rc = new GDI.RECT { left = 0, top = 0, right = width, bottom = height };
                        if (!GDI.ExtTextOutW(hdc, 0, 0, 0x0004 /*ETO_CLIPPED*/, ref rc, text, text.Length, null))
                            throw new Exception("ExtTextOutW falhou.");

                        // Força flush do GDI
                        GDI.GdiFlush();

                        // Copia bits da memória
                        Marshal.Copy(dib.pBits, dib.ManagedBits, 0, dib.ManagedBits.Length);

                        if (SatoTtfPrinter.DEBUG_MODE)
                        {
                            int blackPixels = CountBlackPixels(dib.ManagedBits, width, height, dib.StrideBytes);
                            Console.WriteLine($"[DEBUG] Pixels pretos no bitmap: {blackPixels}");
                        }

                        return new MonoBitmap
                        {
                            Width = dib.Width,
                            Height = dib.Height,
                            StrideBytes = dib.StrideBytes,
                            Bits = dib.ManagedBits
                        };
                    }
                    finally
                    {
                        GDI.SelectObject(hdc, hbmOld);
                        dib.Dispose();
                    }
                }
                finally
                {
                    GDI.SelectObject(hdc, hFontOld);
                    GDI.DeleteObject(hFont);
                }
            }
            finally
            {
                GDI.DeleteDC(hdc);
            }
        }

        private static int CountBlackPixels(byte[] bits, int width, int height, int strideBytes)
        {
            int count = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int row = y * strideBytes;
                    int byteIndex = row + (x >> 3);
                    int bit = 7 - (x & 7);
                    if ((bits[byteIndex] & (1 << bit)) != 0) // bit=1 = preto com nossa paleta
                        count++;
                }
            }
            return count;
        }
    }

    internal sealed class DIBSection : IDisposable
    {
        internal IntPtr HBitmap;
        internal IntPtr pBits;
        internal int Width;
        internal int Height;
        internal int StrideBytes;
        internal byte[] ManagedBits;

        private DIBSection() { }

        public static DIBSection Create1bppTopDown(IntPtr hdc, int width, int height, int dpiX, int dpiY)
        {
            var bmi = new GDI.BITMAPINFO();
            bmi.bmiHeader = new GDI.BITMAPINFOHEADER
            {
                biSize = (uint)Marshal.SizeOf(typeof(GDI.BITMAPINFOHEADER)),
                biWidth = width,
                biHeight = -height, // top-down
                biPlanes = 1,
                biBitCount = 1,
                biCompression = 0,
                biSizeImage = 0,
                biXPelsPerMeter = (int)Math.Round(dpiX * 39.37007874),
                biYPelsPerMeter = (int)Math.Round(dpiY * 39.37007874),
                biClrUsed = 2,
                biClrImportant = 2
            };

            bmi.bmiColors = new GDI.RGBQUAD[2];
            // IMPORTANTE: Em 1bpp, índice 0 = branco, índice 1 = preto
            bmi.bmiColors[0] = new GDI.RGBQUAD { rgbRed = 255, rgbGreen = 255, rgbBlue = 255, rgbReserved = 0 };
            bmi.bmiColors[1] = new GDI.RGBQUAD { rgbRed = 0, rgbGreen = 0, rgbBlue = 0, rgbReserved = 0 };

            IntPtr pBits;
            IntPtr hbm = GDI.CreateDIBSection(hdc, ref bmi, 0, out pBits, IntPtr.Zero, 0);
            if (hbm == IntPtr.Zero)
                throw new Exception("CreateDIBSection falhou.");

            int stride = ((width + 31) / 32) * 4;
            var managed = new byte[stride * height];

            return new DIBSection
            {
                HBitmap = hbm,
                pBits = pBits,
                Width = width,
                Height = height,
                StrideBytes = stride,
                ManagedBits = managed
            };
        }

        public void Dispose()
        {
            if (HBitmap != IntPtr.Zero)
            {
                GDI.DeleteObject(HBitmap);
                HBitmap = IntPtr.Zero;
            }
        }
    }

    internal static class SbplConverter
    {
        public static byte[] CreateGraphicCommandFromBitmap(GdiTextRenderer.MonoBitmap bitmap, bool invertBits)
        {
            int tileCols = (bitmap.Width + 7) / 8;
            int tileRows = (bitmap.Height + 7) / 8;

            if (SatoTtfPrinter.DEBUG_MODE)
                Console.WriteLine($"[DEBUG] Tiles: {tileCols}×{tileRows} (bbb={tileCols:D3}, ccc={tileRows:D3})");

            var payload = PackTiles8x8(bitmap.Bits, bitmap.Width, bitmap.Height, bitmap.StrideBytes, tileCols, tileRows, invertBits);

            string hex = ToHex(payload);

            var cmd = new List<byte>();
            cmd.Add(0x1B); // ESC
            cmd.AddRange(Encoding.ASCII.GetBytes("G"));
            cmd.AddRange(Encoding.ASCII.GetBytes("H"));
            cmd.AddRange(Encoding.ASCII.GetBytes(tileCols.ToString("000")));
            cmd.AddRange(Encoding.ASCII.GetBytes(tileRows.ToString("000")));
            cmd.AddRange(Encoding.ASCII.GetBytes(hex));

            return cmd.ToArray();
        }

        private static byte[] PackTiles8x8(byte[] src, int width, int height, int strideBytes, int tileCols, int tileRows, bool invertBits)
        {
            var list = new List<byte>(tileCols * tileRows * 8);

            if (SatoTtfPrinter.DEBUG_MODE)
                Console.WriteLine($"[DEBUG] PackTiles: width={width}, height={height}, stride={strideBytes}, tiles={tileCols}×{tileRows}");

            // LÓGICA CORRETA: Varre blocos linha por linha (top→bottom)
            for (int tr = 0; tr < tileRows; tr++)
            {
                int y0 = tr * 8;

                // Para cada linha DENTRO dos blocos (0-7), varre TODOS os blocos horizontais
                for (int dy = 0; dy < 8; dy++)
                {
                    int y = y0 + dy;

                    // Varre todos os blocos horizontais nesta linha
                    for (int tc = 0; tc < tileCols; tc++)
                    {
                        int x0 = tc * 8;
                        byte b = 0;

                        // Dentro de cada linha, varre os 8 pixels horizontalmente (left→right)
                        for (int dx = 0; dx < 8; dx++)
                        {
                            int x = x0 + dx;

                            bool isBlack = false;

                            // Verifica se está dentro dos limites do bitmap
                            if (x < width && y < height)
                            {
                                isBlack = GetPixel1bpp(src, x, y, strideBytes);
                            }

                            if (isBlack)
                            {
                                // MSB = pixel da esquerda, LSB = pixel da direita
                                b |= (byte)(1 << (7 - dx));
                            }
                        }

                        if (invertBits)
                            b = ReverseBits(b);

                        list.Add(b);
                    }
                }
            }

            if (SatoTtfPrinter.DEBUG_MODE)
            {
                Console.WriteLine($"[DEBUG] Total de bytes gerados: {list.Count}");
                Console.WriteLine($"[DEBUG] Primeiros 16 bytes: {string.Join("", list.Take(16).Select(b => b.ToString("X2")))}");
            }

            return list.ToArray();
        }

        private static bool GetPixel1bpp(byte[] src, int x, int y, int strideBytes)
        {
            int row = y * strideBytes;
            int byteIndex = row + (x >> 3);
            int bit = 7 - (x & 7);
            return (src[byteIndex] & (1 << bit)) != 0;
        }

        private static byte ReverseBits(byte v)
        {
            v = (byte)(((v * 0x0802u & 0x22110u) | (v * 0x8020u & 0x88440u)) * 0x10101u >> 16);
            return v;
        }

        private static string ToHex(byte[] data)
        {
            var sb = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
                sb.Append(data[i].ToString("X2"));
            return sb.ToString();
        }
    }

    internal static class GDI
    {
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern bool GdiFlush();

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFontIndirectW(ref LOGFONTW lplf);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct LOGFONTW
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string lfFaceName;
        }

        [DllImport("gdi32.dll")]
        public static extern int SetBkMode(IntPtr hdc, int mode);

        [DllImport("gdi32.dll")]
        public static extern uint SetTextColor(IntPtr hdc, int colorRef);

        [DllImport("gdi32.dll")]
        public static extern uint SetBkColor(IntPtr hdc, int colorRef);

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public int cx;
            public int cy;
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetTextExtentPoint32W(IntPtr hdc, string lpString, int c, out SIZE psizl);

        [StructLayout(LayoutKind.Sequential)]
        public struct TEXTMETRICW
        {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public char tmFirstChar;
            public char tmLastChar;
            public char tmDefaultChar;
            public char tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool GetTextMetricsW(IntPtr hdc, out TEXTMETRICW lptm);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern bool ExtTextOutW(
            IntPtr hdc,
            int x,
            int y,
            uint options,
            ref RECT lprect,
            string lpString,
            int c,
            int[] lpDx);

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public RGBQUAD[] bmiColors;
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDIBSection(
            IntPtr hdc,
            ref BITMAPINFO pbmi,
            uint usage,
            out IntPtr ppvBits,
            IntPtr hSection,
            uint offset);
    }
}
