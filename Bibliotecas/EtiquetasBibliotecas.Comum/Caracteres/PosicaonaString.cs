using System;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class PosicaonaString
    {
        public static int Execute(string texto, string procura)
        {
            if (EhStringNuloVazioComEspacosBranco.Execute(texto))
            {
                return -1;
            }
            if (EhStringNuloVazioComEspacosBranco.Execute(procura))
            {
                return -1;
            }
            return texto.IndexOf(procura, StringComparison.InvariantCulture);
        }

        public static int Execute(string texto, string procura, int ocorencia)
        {
            // Rotia de procura de n-enesima ocorrencia de uma string dentro de um texto string maio.
            if (EhStringNuloVazioComEspacosBranco.Execute(texto))
            {
                return -1;
            }
            if (EhStringNuloVazioComEspacosBranco.Execute(procura))
            {
                return -1;
            }
            if (ocorencia <= 0)
            {
                return -1;
            }
            var posicao = -1;
            var contador = 0;
            var posicaoProcura = -1;
            do
            {
                posicaoProcura = texto.IndexOf(procura, posicaoProcura + 1, StringComparison.InvariantCulture);
                contador++;
                if (contador == ocorencia)
                {
                    posicao = posicaoProcura;
                    break;
                }
            }
            while (posicaoProcura != -1);
            return posicao;
        }
    }
}
