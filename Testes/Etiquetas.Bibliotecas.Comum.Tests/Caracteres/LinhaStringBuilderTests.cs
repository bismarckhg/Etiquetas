using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class LinhaStringBuilderTests
    {
        [Fact]
        public void Execute_ComLinhaValida_RetornaLinhaCorreta()
        {
            // Arrange
            var sb = new StringBuilder("linha1\r\nlinha2\r\nlinha3");
            var linhaTexto = 2;
            var expected = "linha2";

            // Act
            var result = LinhaStringBuilder.Execute(sb, linhaTexto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComLinhaForaDosLimites_RetornaStringVazia()
        {
            // Arrange
            var sb = new StringBuilder("linha1\r\nlinha2");
            var linhaTexto = 3;

            // Act
            var result = LinhaStringBuilder.Execute(sb, linhaTexto);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComLinhaInvalida_RetornaStringVazia()
        {
            // Arrange
            var sb = new StringBuilder("linha1\r\nlinha2");
            var linhaTexto = 0;

            // Act
            var result = LinhaStringBuilder.Execute(sb, linhaTexto);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaStringVazia()
        {
            // Arrange
            StringBuilder sb = null;
            var linhaTexto = 1;

            // Act
            var result = LinhaStringBuilder.Execute(sb, linhaTexto);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComDiferentesFinaisDeLinha_FuncionaCorretamente()
        {
            // Arrange
            var sb = new StringBuilder("linha1\rlinha2\nlinha3");
            var expected2 = "linha2";
            var expected3 = "linha3";

            // Act
            var result2 = LinhaStringBuilder.Execute(sb, 2);
            var result3 = LinhaStringBuilder.Execute(sb, 3);

            // Assert
            Assert.Equal(expected2, result2);
            Assert.Equal(expected3, result3);
        }
    }
}
