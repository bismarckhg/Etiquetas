using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.LibStrBuilder
{
    public static class QuantidadeCaracteresNaStringBuilder
    {
        public static int Execute(StringBuilder builder, char caractere)
        {
            var numeroCaracteres = builder.Length;
            var quantidade = 0;
            var posicao = 0;
            var indice = Etiquetas.Bibliotecas.LibStrBuilder.IndexOf.PosicaoKMP(builder, caractere, posicao);
            while (indice > -1)
            {
                posicao = indice+1;
                quantidade++;
                indice = Etiquetas.Bibliotecas.LibStrBuilder.IndexOf.PosicaoKMP(builder, caractere, posicao);
            }
            return quantidade;
        }

        public static int Execute(StringBuilder builder, string texto)
        {
            var numeroCaracteres = builder.Length;
            var quantidade = 0;
            var posicao = 0;
            var indice = Etiquetas.Bibliotecas.LibStrBuilder.IndexOf.PosicaoKMP(builder, texto, posicao);
            while (indice > -1)
            {
                posicao = indice + 1;
                quantidade++;
                indice = Etiquetas.Bibliotecas.LibStrBuilder.IndexOf.PosicaoKMP(builder, texto, posicao);
            }
            return quantidade;
        }

    }
}