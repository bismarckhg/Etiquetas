using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class IndexOfTests
    {
        [Fact]
        public void Posicao_Char_PrimeiraOcorrencia()
        {
            // Arrange
            var sb = new StringBuilder("ab-c-d");

            // Act
            var result = IndexOf.Posicao(sb, '-', 1);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Posicao_Char_SegundaOcorrencia()
        {
            // Arrange
            var sb = new StringBuilder("ab-c-d");

            // Act
            var result = IndexOf.Posicao(sb, '-', 2);

            // Assert
            Assert.Equal(4, result);
        }

        [Fact]
        public void Posicao_Char_NaoEncontrado()
        {
            // Arrange
            var sb = new StringBuilder("ab-c-d");

            // Act
            var result = IndexOf.Posicao(sb, 'x', 1);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Posicao_Char_SequenciaForaDosLimites()
        {
            // Arrange
            var sb = new StringBuilder("ab-c-d");

            // Act
            var result = IndexOf.Posicao(sb, '-', 3);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Posicao_Char_TextoNulo()
        {
            // Arrange
            StringBuilder sb = null;

            // Act
            var result = IndexOf.Posicao(sb, '-', 1);

            // Assert
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Posicao_Char_SequenciaInvalida()
        {
            // Arrange
            var sb = new StringBuilder("ab-c-d");

            // Act
            var result = IndexOf.Posicao(sb, '-', 0);

            // Assert
            Assert.Equal(-1, result);
        }
    }
}
