using Etiquetas.Core.Enum;
using System.Net;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Interface que define o posicionamento dos campos na etiqueta.
    /// </summary>
    public interface IPosicaoCamposEtiqueta
    {

        /// <summary>Obtém o tipo de linguagem de impressão utilizada</summary>
        TipoLinguagemImpressao TipoLinguagem { get; }

        /// <summary>Obtém o marcador que indica o início do texto (ex: "^FD" no ZPL)</summary>
        string MarcadorInicialTexto { get; }

        /// <summary>Obtém o marcador que indica o fim do texto (ex: "^FS" no ZPL)</summary>
        string MarcadorFinalTexto { get; }

        /// <summary>Obtém a configuração do campo Código do Material</summary>
        IConfiguracaoCampo CodigoMaterial { get; }

        /// <summary>Obtém a configuração do campo Descrição do Medicamento</summary>
        IConfiguracaoCampo DescricaoMedicamento { get; }

        /// <summary>Obtém a configuração do campo Descrição do Medicamento 2</summary>
        IConfiguracaoCampo DescricaoMedicamento2 { get; }

        /// <summary>Obtém a configuração do campo Princípio Ativo 1</summary>
        IConfiguracaoCampo PrincipioAtivo { get; }

        /// <summary>Obtém a configuração do campo Princípio Ativo 2</summary>
        IConfiguracaoCampo PrincipioAtivo2 { get; }

        /// <summary>Obtém a configuração do campo Embalagem</summary>
        IConfiguracaoCampo Embalagem { get; }

        /// <summary>Obtém a configuração do campo Lote</summary>
        IConfiguracaoCampo Lote { get; }

        /// <summary>Obtém a configuração do campo Validade</summary>
        IConfiguracaoCampo Validade { get; }

        /// <summary>Obtém a configuração do campo Código do Usuário</summary>
        IConfiguracaoCampo CodigoUsuario { get; }

        /// <summary>Obtém a configuração do campo Código de Barras</summary>
        IConfiguracaoCampo CodigoBarras { get; }

        /// <summary>Obtém a configuração do campo Número de Cópias</summary>
        IConfiguracaoCampo Copias { get; }
    }
}

