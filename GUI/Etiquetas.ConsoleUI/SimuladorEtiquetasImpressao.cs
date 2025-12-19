using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.ConsoleUI
{
    /// <summary>
    /// Simulador de Etiquetas para Impressão.
    /// </summary>
    public static class SimuladorEtiquetasImpressao
    {
        /// <summary>
        /// Lista DTO de Etiquetas para simulação.
        /// </summary>
        /// <returns>Lista DTO Etiqueta Impressao.</returns>
        public static List<IEtiquetaImpressaoDto> RetornoDto()
        {
            var lista = new List<IEtiquetaImpressaoDto>();
            // Simulação de preenchimento da lista com dados fictícios
            var etiqueta = new Etiquetas.Application.DTOs.EtiquetaImpressaoDto
            {
                Id = 1,
                DescricaoMedicamento = "Dipirona Sódica",
                PrincipioAtivo1 = "Dipirona monoidratada",
                PrincipioAtivo2 = "",
                CodigoMaterial = "1534",
                Validade = DateTime.Now.AddMonths(6).ToString("o"),
                Lote = "LoteA123",
                MatriculaFuncionario = "FUNC001",
                CodigoBarras = "7890123456789",
                DataHoraInicio = DateTime.Now.ToString("o"),
                DataHoraFim = DateTime.Now.AddMinutes(5).ToString("o"),
                StatusEtiqueta = 'P',
                QuantidadeSolicitada = 30
            };

            lista.Add(etiqueta);

            etiqueta = new Etiquetas.Application.DTOs.EtiquetaImpressaoDto
            {
                Id = 2,
                DescricaoMedicamento = "Ceftriaxona",
                PrincipioAtivo1 = "Ceftriaxona dissódica",
                PrincipioAtivo2 = "",
                CodigoMaterial = "6423",
                Validade = DateTime.Now.AddMonths(12).ToString("o"),
                Lote = "LoteB456",
                MatriculaFuncionario = "FUNC002",
                CodigoBarras = "9876543210987",
                DataHoraInicio = DateTime.Now.ToString("o"),
                DataHoraFim = DateTime.Now.AddMinutes(10).ToString("o"),
                StatusEtiqueta = 'P',
                QuantidadeSolicitada = 20
            };

            lista.Add(etiqueta);

            etiqueta = new Etiquetas.Application.DTOs.EtiquetaImpressaoDto
            {
                Id = 3,
                DescricaoMedicamento = "Omeprazol",
                PrincipioAtivo1 = "Omeprazol",
                PrincipioAtivo2 = "",
                CodigoMaterial = "1123",
                Validade = DateTime.Now.AddMonths(3).ToString("o"),
                Lote = "LoteC789",
                MatriculaFuncionario = "FUNC003",
                CodigoBarras = "1231231231231",
                DataHoraInicio = DateTime.Now.ToString("o"),
                DataHoraFim = DateTime.Now.AddMinutes(15).ToString("o"),
                StatusEtiqueta = 'P',
                QuantidadeSolicitada = 50
            };

            lista.Add(etiqueta);

            etiqueta = new Etiquetas.Application.DTOs.EtiquetaImpressaoDto
            {
                Id = 4,
                DescricaoMedicamento = "Paracetamol",
                PrincipioAtivo1 = "Paracetamol",
                PrincipioAtivo2 = "",
                CodigoMaterial = "3344",
                Validade = DateTime.Now.AddMonths(9).ToString("o"),
                Lote = "LoteD012",
                MatriculaFuncionario = "FUNC004",
                CodigoBarras = "4564564564564",
                DataHoraInicio = DateTime.Now.ToString("o"),
                DataHoraFim = DateTime.Now.AddMinutes(20).ToString("o"),
                StatusEtiqueta = 'P',
                QuantidadeSolicitada = 40
            };

            lista.Add(etiqueta);

            etiqueta = new Etiquetas.Application.DTOs.EtiquetaImpressaoDto
            {
                Id = 5,
                DescricaoMedicamento = "Amoxicilina",
                PrincipioAtivo1 = "Amoxicilina trihidratada",
                PrincipioAtivo2 = "",
                CodigoMaterial = "5566",
                Validade = DateTime.Now.AddMonths(4).ToString("o"),
                Lote = "LoteE345",
                MatriculaFuncionario = "FUNC005",
                CodigoBarras = "7897897897897",
                DataHoraInicio = DateTime.Now.ToString("o"),
                DataHoraFim = DateTime.Now.AddMinutes(25).ToString("o"),
                StatusEtiqueta = 'P',
                QuantidadeSolicitada = 25
            };

            lista.Add(etiqueta);

            return lista;
        }
    }
}
