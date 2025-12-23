using System.Net;

namespace Etiquetas.Core.Interfaces
{
    /// <summary>
    /// Interface que define o posicionamento dos campos na etiqueta.
    /// </summary>
    public interface IPosicaoCamposEtiqueta
    {
        /// <summary>
        /// Gets or Sets - Marcador Inicial do Texto em comando Impressoras ZPL, EPL ou SBPL.
        /// </summary>
        string MarcadorInicialTexto { get; set; }

        /// <summary>
        /// Gets or Sets - Marcador Final do Texto em comando Impressoras ZPL, EPL ou SBPL.
        /// </summary>
        string MarcadorFinalTexto { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Código do Material. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string CodigoMaterialCmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Código do Material. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string CodigoMaterialCmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Código de Barras. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string CodigoBarrasCmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Código de Barras. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string CodigoBarrasCmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Primeira Parte da Descrição do Medicamento. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string DescricaoMedicamentoCmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Primeira Parte da Descrição do Medicamento. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string DescricaoMedicamentoCmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Segunda Parte da Descrição do Medicamento. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string DescricaoMedicamento2Cmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Segunda Parte da Descrição do Medicamento. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string DescricaoMedicamento2Cmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Primeira parte do Pricipio ativo. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string PrincipioAtivo1Cmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Primeira parte do principio ativo. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string PrincipioAtivo1Cmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Segunda parte do principio ativo. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string PrincipioAtivo2Cmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Segunda parte do principio ativo. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string PrincipioAtivo2Cmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Embalagem do material. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string EmbalagemCmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento da Embalagem do material. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string EmbalagemCmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Lote do material. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string LoteCmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Lote do material. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string LoteCmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Data de Validade do Material. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string ValidadeCmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Data de Validade do Material. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string ValidadeCmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Código do Usuario. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string CodigoUsuarioCmd1 { get; set; }

        /// <summary>
        /// Gets or Sets - Posicionamento do Código do Usuario. Cmd1 e Cmd2 representam Posicao Coluna e linhas, podendo ser comandos separados ou não
        /// </summary>
        string CodigoUsuarioCmd2 { get; set; }

        /// <summary>
        /// Gets or Sets - Comando com a Quantidade de Copias da etiqueta. Cmd representa o comando para definir a quantidade de copias na impressora
        /// </summary>
        string CopiasCmd { get; set; }
    }
}
