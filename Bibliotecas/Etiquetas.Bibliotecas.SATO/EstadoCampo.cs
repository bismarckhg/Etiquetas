using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Representa o estado de análise de um campo durante o processamento da etiqueta.
    /// </summary>
    public class EstadoCampo
    {
        /// <summary>Tipo do campo identificado</summary>
        public EnumTipoCampo Tipo { get; set; }

        /// <summary>
        /// Comando de posicionamento do início do campo 1.
        /// </summary>
        public string Cmd1 { get; set; }

        /// <summary>
        /// Comando de posicionamento do início do campo 2.
        /// </summary>
        public string Cmd2 { get; set; }

        /// <summary>Indica se o primeiro comando de posicionamento foi encontrado</summary>
        public bool Cmd1Encontrado { get; set; }

        /// <summary>Indica se o segundo comando de posicionamento foi encontrado</summary>
        public bool Cmd2Encontrado { get; set; }

        /// <summary>Indica se o campo está pronto para extrair o texto</summary>
        public bool ProntoParaExtrair
        {
            get
            {
                // Se só tem Cmd1 configurado, precisa apenas dele
                // Se tem Cmd1 e Cmd2, precisa de ambos
                return Cmd1Encontrado;
            }
        }

        /// <summary>Reseta o estado do campo</summary>
        public void Reset()
        {
            Tipo = EnumTipoCampo.Nenhum;
            Cmd1Encontrado = false;
            Cmd2Encontrado = false;
        }
    }
}
