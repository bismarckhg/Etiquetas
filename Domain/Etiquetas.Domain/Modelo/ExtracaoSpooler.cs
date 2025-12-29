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
        public ComandosZPL ZPL { get; set; }

        // Configurações SBPL(ESC= Chr(27)) 
        public ComandosSBPL SBPL { get; set; }

        public ComandosEPL EPL { get; set; }

        // Campos da Etiqueta
        public ComandosCampos Campos { get; set; }
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
