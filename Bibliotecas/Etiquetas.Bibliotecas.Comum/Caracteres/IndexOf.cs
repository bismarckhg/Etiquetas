using System;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Caracteres
{
    public static class IndexOf
    {
        public static int Posicao(StringBuilder texto, char caractere, int sequencia)
        {
            if (texto == null || sequencia <= 0)
            {
                return -1;
            }

            var sequenciaAtual = 0;
            var numCaracteres = texto.Length;
            for (int i = 0; i < numCaracteres; i++)
            {
                var caractereAtual = texto[i];
                if (caractereAtual == caractere)
                {
                    sequenciaAtual++;
                    if (sequenciaAtual == sequencia)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}