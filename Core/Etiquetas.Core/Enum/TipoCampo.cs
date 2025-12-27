using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Core.Enum
{
    /// <summary>
    /// Enumeração interna que identifica o campo sendo processado na etiqueta.
    /// </summary>
    internal enum TipoCampo
    {
        Nenhum = 0,
        CodigoMaterial,
        DescricaoMedicamento,
        DescricaoMedicamento2,
        PrincipioAtivo1,
        PrincipioAtivo2,
        Embalagem,
        Lote,
        Validade,
        CodigoUsuario,
        CodigoBarras,
        Copias
    }
}
