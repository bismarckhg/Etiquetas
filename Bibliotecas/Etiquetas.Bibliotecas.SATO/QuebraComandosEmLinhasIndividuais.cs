using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etiquetas.Bibliotecas.ControleFilaDados;

namespace Etiquetas.Bibliotecas.SATO
{
    /// <summary>
    /// Quebra comandos impressora em linhas individuais.
    /// </summary>
    public static class QuebraComandosEmLinhasIndividuais
    {
        /// <summary>
        /// Quebra comandos impressora em linhas individuais.
        /// </summary>
        /// <param name="comandosSpooler">Comandos recebidos do spooler.</param>
        /// <param name="tipoLinguagem">ZPL, EPL ou SBPL.</param>
        /// <returns>Array string com linhas de comandos individuais.</returns>
        public static string[] Execute(string comandosSpooler, TipoLinguagemImpressao tipoLinguagem)
        {
            /// var comandos = new List<string>();
            var arrayLinhas = Etiquetas.Bibliotecas.Comum.Arrays.StringEmArrayStringPorSeparador.Execute(comandosSpooler, new[] { "\r\n", "\n", "\r" }, true);
            var arrayLinhasPorComandos = new ConcurrentQueue<IReadOnlyList<string>>();

            switch (tipoLinguagem)
            {
                case TipoLinguagemImpressao.ZPL:
                    // ZPL: quebra por ^ (comandos começam com ^)
                    foreach (var linha in arrayLinhas)
                    {
                        // comandos.Add("^" + parte.Trim());
                        var arrayZPL = Etiquetas.Bibliotecas.Comum.Arrays.StringEmArrayStringComSeparadorEmCadaItem.Execute(linha, "^", true);
                        arrayLinhasPorComandos.EnqueueBatch(arrayZPL);
                    }

                    break;
                case TipoLinguagemImpressao.EPL:
                    // EPL: quebra por linha
                    arrayLinhasPorComandos.EnqueueBatch(arrayLinhas.Select(l => l.Trim()));
                    break;
                case TipoLinguagemImpressao.SBPL:
                    // SBPL: quebra por ESC (cada comando começa com ESC)
                    var esc = Convert.ToChar(27);
                    foreach (var linha in arrayLinhas)
                    {
                        // comandos.Add(esc + parte.Trim());
                        var arraySBPL = Etiquetas.Bibliotecas.Comum.Arrays.StringEmArrayStringComSeparadorEmCadaItem.Execute(linha, $"{esc}", true);
                        arrayLinhasPorComandos.EnqueueBatch(arraySBPL);
                    }

                    break;
            }

            return arrayLinhasPorComandos.ToFlattenedArraySnapshot();
        }
    }

    /// <summary>
    /// Quebra comandos ZPL em linhas individuais.
    /// </summary>
    public static class QuebraComandosZPLEmLinhasIndividuais
    {
        /// <summary>
        /// Quebra comandos ZPL em linhas individuais.
        /// </summary>
        /// <param name="texto">Sppoler comandos ZPL</param>
        /// <returns>Array string com linhas separadas.</returns>
        public static string[] Execute(string texto)
        {

            // Separa comandos por quebra de linhas, removendo linhas vazias
            //var arrayLinhas = Etiquetas.Bibliotecas.Comum.StringEmArrayStringPorSeparador.CriaDicionario(texto, new[] { "\r\n", "\n", "\r" }, true);
            var quebraLinhas = Etiquetas.Bibliotecas.Comum.Arrays.StringEmArrayStringPorSeparador.Execute(texto, new[] { "\r\n", "\n", "\r" }, true);

            var quebraComandosEmLinhas = new ConcurrentQueue<IReadOnlyList<string>>();
            foreach (var linha in quebraLinhas)
            {
                // Processa cada linha individualmente
                // Separa varios comandos que estão em uma mesma linhas, em comandos com linhas individuais, mantendo o inicio de comando "^"
                //var quebralinhaComandoEmLinhas = Etiquetas.Bibliotecas.StringEmArrayStringComSeparadorEmCadaItem.CriaDicionario(linha, "^", true);
                var quebralinhaComandoEmLinhas = Etiquetas.Bibliotecas.Comum.Arrays.StringEmArrayStringComSeparadorEmCadaItem.Execute(linha, "^", true);

                quebraComandosEmLinhas.EnqueueBatch(quebralinhaComandoEmLinhas);
            }

            return quebraComandosEmLinhas.ToFlattenedArraySnapshot();
        }
    }
}
