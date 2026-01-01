using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.SATO;
using Etiquetas.Core.Interfaces;
using Etiquetas.Domain.Configuracao;

namespace Etiquetas.ConsoleUI
{
    public class TesteEtiqueta
    {
        /// <summary>
        /// Processa uma etiqueta no formato ZPL.
        /// </summary>
        public static void ProcessarEtiqueta()
        {
            Console.WriteLine("=== Processando Etiqueta ZPL ===\n");

            //            var etiquetaZPL = @"
            //^XA
            //^FO0010,0005^FD123456^FS
            //^FO0010,0025^FDPARACETAMOL 500MG^FS
            //^FO0010,0045^FDCOMPRIMIDO^FS
            //^FO0010,0065^FDPARACETAMOL^FS
            //^FO0200,0005^FDLOTE123^FS
            //^FO0200,0025^FD12/2025^FS
            //^FO0200,0045^FDUSR001^FS
            //^FO0050,0150^BY2^BCN,100,Y,N,N^FD123456789012^FS
            //^PQ5
            //^XZ";

            var etiquetaZPL = @"
^XA
^BY2,,
^LH1,5
^LL144
^PQ00000001
^FO012,23^ABN,11,7^FDAGUA BIDESTILADA AMP 10 M^FS 
^FO012,36^ABN,11,7^FDL^FS 
^FO012,39^ABN,11,7^FDAGUA DESTILADA AMP 1^FS 
^FO012,52^ABN,11,7^FD0 ML^FS 
^FO135,68^ABN,11,7^FDV:31/12/2026^FS 
^FO012,68^ABN,11,7^FDCOD:127327^FS 
^FO170,85^ABN,11,7^FDF0114124^FS 
^FO012,85^ABN,1,1^FDLT:.25A0011I^FS 
^FO025,100^BEN,20,Y,N,Y^FD200004231912^FS 
^XZ
";
            var config = new PosicaoCamposEtiqueta(EnumTipoLinguagemImpressao.ZPL);
            //var dados = ExtratorInteligenteDadosEtiqueta.Extrair(etiquetaZPL, config);
            var dados = Etiquetas.Application.Mappers.EtiquetaMapper.SpolerToDto(etiquetaZPL, config);
            ImprimirDados(dados);
        }

        /// <summary>
        /// Imprime os dados extraídos no console.
        /// </summary>
        /// <param name="dados">Dados a serem impressos</param>
        private static void ImprimirDados(IEtiquetaImpressaoDto dados)
        {
            if (dados == null)
            {
                Console.WriteLine("Nenhum dado foi extraído.");
                return;
            }

            Console.WriteLine($"Código Material: {dados.CodigoMaterial}");
            Console.WriteLine($"Descrição 1: {dados.DescricaoMedicamento}");
            Console.WriteLine($"Descrição 2: {dados.DescricaoMedicamento2}");
            Console.WriteLine($"Princípio Ativo 1: {dados.PrincipioAtivo}");
            Console.WriteLine($"Princípio Ativo 2: {dados.PrincipioAtivo2}");
            Console.WriteLine($"Embalagem: {dados.Embalagem}");
            Console.WriteLine($"Lote: {dados.Lote}");
            Console.WriteLine($"Validade: {dados.Validade}");
            Console.WriteLine($"Usuário: {dados.CodigoUsuario}");
            Console.WriteLine($"Código de Barras: {dados.CodigoBarras}");
            Console.WriteLine($"Quantidade: {dados.QuantidadeSolicitada}");
        }
    }
}
