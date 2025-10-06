using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.TTFRenderizacao.Stable
{
    public static class TesteManus
    {
        public static void Run()
        {
            Console.WriteLine("=== TESTE DE IMPRESSÃO SATO CL4NX ===\n");

            bool success = SatoTtfPrinter.PrintTextAsGraphic(
                printerName: "SATO CL4NX 203dpi",
                text: "Bismarck",
                fontName: "Microsoft Sans Serif",
                fontSizePt: 10.0f,
                isBold: false,
                positionX_dots: 20,
                positionY_dots: 30,
                labelWidthMm: 33.0,
                labelHeightMm: 21.0
            );

            Console.WriteLine($"\n=== RESULTADO: {(success ? "SUCESSO" : "FALHA")} ===");
            Console.ReadKey();
        }
    }
}
