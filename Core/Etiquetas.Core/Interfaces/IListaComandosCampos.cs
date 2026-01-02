using Etiquetas.Bibliotecas.SATO;
using System.Collections.Generic;

namespace Etiquetas.Domain.Modelo
{
    public interface IListaComandosCampos
    {
        List<ComandosPadraoImpressora> Comandos { get; set; }
    }
}
