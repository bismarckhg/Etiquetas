using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Core.Interfaces;

namespace Etiquetas.Application.Config
{
    /// <summary>
    /// Define a posição dos campos na etiqueta.
    /// </summary>
    public class PosicaoCamposEtiqueta : IPosicaoCamposEtiqueta
    {
        /// <inheritdoc/>
        public string MarcadorInicialTexto { get; set; }

        /// <inheritdoc/>
        public string MarcadorFinalTexto { get; set; }

        /// <inheritdoc/>
        public string CodigoMaterialCmd1 { get; set; }

        /// <inheritdoc/>
        public string CodigoMaterialCmd2 { get; set; }

        /// <inheritdoc/>
        public string CodigoBarrasCmd1 { get; set; }

        /// <inheritdoc/>
        public string CodigoBarrasCmd2 { get; set; }

        /// <inheritdoc/>
        public string DescricaoMedicamentoCmd1 { get; set; }

        /// <inheritdoc/>
        public string DescricaoMedicamentoCmd2 { get; set; }

        /// <inheritdoc/>
        public string PrincipioAtivo1Cmd1 { get; set; }

        /// <inheritdoc/>
        public string PrincipioAtivo1Cmd2 { get; set; }

        /// <inheritdoc/>
        public string PrincipioAtivo2Cmd1 { get; set; }

        /// <inheritdoc/>
        public string PrincipioAtivo2Cmd2 { get; set; }

        /// <inheritdoc/>
        public string EmbalagemCmd1 { get; set; }

        /// <inheritdoc/>
        public string EmbalagemCmd2 { get; set; }

        /// <inheritdoc/>
        public string LoteCmd1 { get; set; }

        /// <inheritdoc/>
        public string LoteCmd2 { get; set; }

        /// <inheritdoc/>
        public string ValidadeCmd1 { get; set; }

        /// <inheritdoc/>
        public string ValidadeCmd2 { get; set; }

        /// <inheritdoc/>
        public string CodigoUsuarioCmd1 { get; set; }

        /// <inheritdoc/>
        public string CodigoUsuarioCmd2 { get; set; }

        /// <inheritdoc/>
        public string CopiasCmd { get; set; }
    }
}
