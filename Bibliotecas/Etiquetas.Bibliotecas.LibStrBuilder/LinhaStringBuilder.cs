using System.Text;

namespace Etiquetas.Bibliotecas.LibStrBuilder
{
    public static class LinhaStringBuilder
    {
        public static string Execute(StringBuilder texto, int linhaTexto)
        {
            var linha = new StringBuilder();
            var posicaoInicialLinha = -1;
            var numeroLinha = 0;
            var numCaracteres = texto.Length;
            for (int i = 0; i < numCaracteres; i++)
            {
                if (posicaoInicialLinha > -1 && posicaoInicialLinha <= i)
                {
                    linha.Append(texto[i]);
                }
                var caractereAtual = texto[i];
                if (caractereAtual == '\r')
                {
                    numeroLinha++;
                    if (numeroLinha == linhaTexto)
                    {
                        posicaoInicialLinha = i + 1;
                    }
                }
            }
            return linha.ToString();
        }
    }
}
