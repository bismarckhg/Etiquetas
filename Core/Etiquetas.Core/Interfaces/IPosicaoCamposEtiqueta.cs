using System.Net;
using Etiquetas.Application.Configuracao;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Interface que define o posicionamento dos campos na etiqueta.
    /// </summary>
    public interface IPosicaoCamposEtiqueta
    {

        /// <summary>
        /// Interface que define o contrato para configuração de posições de campos em etiquetas.
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
            ConfiguracaoCampo CodigoMaterial { get; }

            /// <summary>Obtém a configuração do campo Descrição do Medicamento</summary>
            ConfiguracaoCampo DescricaoMedicamento { get; }

            /// <summary>Obtém a configuração do campo Descrição do Medicamento 2</summary>
            ConfiguracaoCampo DescricaoMedicamento2 { get; }

            /// <summary>Obtém a configuração do campo Princípio Ativo 1</summary>
            ConfiguracaoCampo PrincipioAtivo1 { get; }

            /// <summary>Obtém a configuração do campo Princípio Ativo 2</summary>
            ConfiguracaoCampo PrincipioAtivo2 { get; }

            /// <summary>Obtém a configuração do campo Embalagem</summary>
            ConfiguracaoCampo Embalagem { get; }

            /// <summary>Obtém a configuração do campo Lote</summary>
            ConfiguracaoCampo Lote { get; }

            /// <summary>Obtém a configuração do campo Validade</summary>
            ConfiguracaoCampo Validade { get; }

            /// <summary>Obtém a configuração do campo Código do Usuário</summary>
            ConfiguracaoCampo CodigoUsuario { get; }

            /// <summary>Obtém a configuração do campo Código de Barras</summary>
            ConfiguracaoCampo CodigoBarras { get; }

            /// <summary>Obtém a configuração do campo Número de Cópias</summary>
            ConfiguracaoCampo Copias { get; }
        }

    }
}

