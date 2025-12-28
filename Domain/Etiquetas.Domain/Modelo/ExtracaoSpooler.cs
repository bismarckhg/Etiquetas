using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Modelo
{
    [Serializable]
    public class ExtracaoSpooler
    {
        // Configurações ZPL

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando ZPL.
        /// </summary>
        public string ZPL_MarcadorInicioTexto { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando ZPL.
        /// </summary>
        public string ZPL_MarcadorFimTexto { get; set; }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para definir a posição de um elemento na etiqueta.
        /// </summary>
        public string ZPL_ComandoPosicao { get; set; }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para especificar o número de cópias a serem impressas.
        /// </summary>
        public string ZPL_ComandoCopias { get; set; }

        /// <summary>
        /// Gets or sets - O comando ZPL usado para configurar a impressão de códigos de barras.
        /// </summary>
        public string ZPL_ComandoBarras { get; set; }

        // Configurações SBPL(ESC= Chr(27)) 

        /// <summary>
        /// Gets or sets - O marcador ESC (caractere de escape) usado em comandos SBPL.
        /// </summary>
        public string SBPL_MarcadorESC { get; set; }

        /// <summary>
        /// Gets or sets - O comando SBPL usado para definir a posição horizontal de um elemento na etiqueta.
        /// </summary>
        public string SBPL_ComandoHorizontal { get; set; }

        /// <summary>
        /// Gets or sets - O comando SBPL usado para definir a posição vertical de um elemento na etiqueta.
        /// </summary>
        public string SBPL_ComandoVertical { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando SBPL.
        /// </summary>
        public string SBPL_MarcadorInicioTexto { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando SBPL.
        /// </summary>
        public string SBPL_MarcadorFimTexto { get; set; }

        /// <summary>
        /// Gets or sets - O comando EPL usado para imprimir texto na etiqueta.
        /// </summary>
        public string EPL_ComandoTexto { get; set; }

        /// <summary>
        /// Gets or sets - O comando EPL usado para imprimir códigos de barras na etiqueta.
        /// </summary>
        public string EPL_ComandoBarras { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o início de uma seção de texto em um comando EPL.
        /// </summary>
        public string EPL_MarcadorInicioTexto { get; set; }

        /// <summary>
        /// Gets or sets - O marcador que indica o fim de uma seção de texto em um comando EPL.
        /// </summary>
        public string EPL_MarcadorFimTexto { get; set; }

        // Campos da Etiqueta

        /// <summary>
        /// Gets or sets - Código Material - Posição 1 única em ^FO.
        /// </summary>
        public string Campo_CodigoMaterial_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Código Material - Posição 2 em ^FO.
        /// </summary>
        public string Campo_CodigoMaterial_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Código Material é obrigatório.
        /// </summary>
        public bool Campo_CodigoMaterial_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Descrição 1 - Posição única.
        /// </summary>
        public string Campo_DescricaoMedicamento_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Descrição 1 - Posição 2.
        /// </summary>
        public string Campo_DescricaoMedicamento_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Descrição Medicamento é obrigatório.
        /// </summary>
        public bool Campo_DescricaoMedicamento_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Descrição 2 - Posição única.
        /// </summary>
        public string Campo_DescricaoMedicamento2_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Descrição 2 - Posição 2.
        /// </summary>
        public string Campo_DescricaoMedicamento2_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Descrição Medicamento 2 é obrigatório. 
        /// </summary>
        public bool Campo_DescricaoMedicamento2_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Princípio Ativo posicao Unica.
        /// </summary>
        public string Campo_PrincipioAtivo_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Princípio Ativo posicao 2.
        /// </summary>
        public string Campo_PrincipioAtivo_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Princípio Ativo é obrigatório. 
        /// </summary>
        public bool Campo_PrincipioAtivo_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Princípio Ativo 2, posicao única. 
        /// </summary>
        public string Campo_PrincipioAtivo2_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Princípio Ativo 2 posição 2.
        /// </summary>
        public string Campo_PrincipioAtivo2_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Princípio Ativo 2 é obrigatório. 
        /// </summary>
        public bool Campo_PrincipioAtivo2_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Embalagem posição única.
        /// </summary>
        public string Campo_Embalagem_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Embalagem posicao 2.
        /// </summary>
        public string Campo_Embalagem_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Embalagem é obrigatório. 
        /// </summary>
        public bool Campo_Embalagem_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Lote posicao única.
        /// </summary>
        public string Campo_Lote_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Lote posição 2.
        /// </summary>
        public string Campo_Lote_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Lote é obrigatório. 
        /// </summary>
        public bool Campo_Lote_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Validade posição única.
        /// </summary>
        public string Campo_Validade_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Validade posição 2.
        /// </summary>
        public string Campo_Validade_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Validade é obrigatório. 
        /// </summary>
        public bool Campo_Validade_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Usuário posição única.
        /// </summary>
        public string Campo_CodigoUsuario_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Usuário posição 2.
        /// </summary>
        public string Campo_CodigoUsuario_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Código Usuário é obrigatório. 
        /// </summary>
        public bool Campo_CodigoUsuario_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Código de Barras posição única.
        /// </summary>
        public string Campo_CodigoBarras_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Código de Barras posição 2.
        /// </summary>
        public string Campo_CodigoBarras_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Código de Barras é obrigatório. 
        /// </summary>
        public bool Campo_CodigoBarras_Obrigatorio { get; set; }

        /// <summary>
        /// Gets or sets - Cópias posição única.
        /// </summary>
        public string Campo_Copias_Cmd1 { get; set; }

        /// <summary>
        /// Gets or sets - Cópias.
        /// </summary>
        public string Campo_Copias_Cmd2 { get; set; }

        /// <summary>
        /// Gets or sets - Indica se o campo Cópias é obrigatório. 
        /// </summary>
        public bool Campo_Copias_Obrigatorio { get; set; }
    }

    //<!-- Configurações ZPL -->
    //    <add key = "ZPL_MarcadorInicioTexto" value="^FD"/>
    //    <add key = "ZPL_MarcadorFimTexto" value="^FS"/>
    //    <add key = "ZPL_ComandoPosicao" value="^FO"/>
    //    <add key = "ZPL_ComandoCopias" value="^PQ"/>
    //    <add key = "ZPL_ComandoBarras" value="^BY"/>

    //    <!-- Configurações SBPL(ESC= Chr(27)) -->
    //    <add key = "SBPL_MarcadorESC" value="27"/>
    //    <add key = "SBPL_ComandoHorizontal" value="H"/>
    //    <add key = "SBPL_ComandoVertical" value="V"/>
    //    <add key = "SBPL_MarcadorInicioTexto" value=""/>
    //    <add key = "SBPL_MarcadorFimTexto" value=""/>

    //    <!-- Configurações EPL -->
    //    <add key = "EPL_ComandoTexto" value="A"/>
    //    <add key = "EPL_ComandoBarras" value="B"/>
    //    <add key = "EPL_MarcadorInicioTexto" value="&quot;"/>
    //    <add key = "EPL_MarcadorFimTexto" value="&quot;"/>

    //    <!-- Exemplo: Campos ZPL -->
    //    <!-- Código Material - Posição única em ^FO -->
    //    <add key = "Campo_CodigoMaterial_Cmd1" value="^FO12,68"/>
    //    <add key = "Campo_CodigoMaterial_Cmd2" value=""/>
    //    <add key = "Campo_CodigoMaterial_Obrigatorio" value="true"/>

    //    <!-- Descrição 1 - Posição única -->
    //    <add key = "Campo_DescricaoMedicamento_Cmd1" value="^FO12,23"/>
    //    <add key = "Campo_DescricaoMedicamento_Cmd2" value=""/>
    //    <add key = "Campo_DescricaoMedicamento_Obrigatorio" value="true"/>

    //    <!-- Descrição 2 - Posição única -->
    //    <add key = "Campo_DescricaoMedicamento2_Cmd1" value="^FO12,36"/>
    //    <add key = "Campo_DescricaoMedicamento2_Cmd2" value=""/>
    //    <add key = "Campo_DescricaoMedicamento2_Obrigatorio" value="false"/>

    //    <!-- Princípio Ativo 1 -->
    //    <add key = "Campo_PrincipioAtivo_Cmd1" value="^FO12,39"/>
    //    <add key = "Campo_PrincipioAtivo_Cmd2" value=""/>
    //    <add key = "Campo_PrincipioAtivo_Obrigatorio" value="true"/>

    //    <!-- Princípio Ativo 2 -->
    //    <add key = "Campo_PrincipioAtivo2_Cmd1" value="^FO12,52"/>
    //    <add key = "Campo_PrincipioAtivo2_Cmd2" value=""/>
    //    <add key = "Campo_PrincipioAtivo2_Obrigatorio" value="false"/>

    //    <!-- Embalagem -->
    //    <add key = "Campo_Embalagem_Cmd1" value="^FO00,00"/>
    //    <add key = "Campo_Embalagem_Cmd2" value=""/>
    //    <add key = "Campo_Embalagem_Obrigatorio" value="false"/>

    //    <!-- Lote -->
    //    <add key = "Campo_Lote_Cmd1" value="^FO012,85"/>
    //    <add key = "Campo_Lote_Cmd2" value=""/>
    //    <add key = "Campo_Lote_Obrigatorio" value="true"/>

    //    <!-- Validade -->
    //    <add key = "Campo_Validade_Cmd1" value="^FO135,68"/>
    //    <add key = "Campo_Validade_Cmd2" value=""/>
    //    <add key = "Campo_Validade_Obrigatorio" value="true"/>

    //    <!-- Usuário -->
    //    <add key = "Campo_CodigoUsuario_Cmd1" value="^FO170,85"/>
    //    <add key = "Campo_CodigoUsuario_Cmd2" value=""/>
    //    <add key = "Campo_CodigoUsuario_Obrigatorio" value="true"/>

    //    <!-- Código de Barras -->
    //    <add key = "Campo_CodigoBarras_Cmd1" value="^FO025,100"/>
    //    <add key = "Campo_CodigoBarras_Cmd2" value=""/>
    //    <add key = "Campo_CodigoBarras_Obrigatorio" value="true"/>

    //    <!-- Cópias -->
    //    <add key = "Campo_Copias_Cmd1" value="^PQ"/>
    //    <add key = "Campo_Copias_Cmd2" value=""/>
    //    <add key = "Campo_Copias_Obrigatorio" value="true"/>

}
