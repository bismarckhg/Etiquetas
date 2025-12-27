using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Enumeração interna que identifica o campo sendo processado na etiqueta.
    /// </summary>
    public enum TipoCampo
    {
        Nenhum = 0,
        CodigoMaterial,
        DescricaoMedicamento,
        DescricaoMedicamento2,
        PrincipioAtivo,
        PrincipioAtivo2,
        Embalagem,
        Lote,
        Validade,
        CodigoUsuario,
        CodigoBarras,
        Copias
    }
}
