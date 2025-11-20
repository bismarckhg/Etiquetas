using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConcatenarCaractereTextoTests
    {
        [Fact]
        public void Execute_ComCharEString_RetornaConcatenacao()
        {
            // Arrange
            var caractere = 'a';
            var texto = "bc";
            var expected = "abc";

            // Act
            var result = ConcatenarCaractereTexto.Execute(caractere, texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringNula_RetornaCharComoString()
        {
            // Arrange
            var caractere = 'a';
            string texto = null;
            var expected = "a";

            // Act
            var result = ConcatenarCaractereTexto.Execute(caractere, texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComStringVazia_RetornaCharComoString()
        {
            // Arrange
            var caractere = 'a';
            var texto = "";
            var expected = "a";

            // Act
            var result = ConcatenarCaractereTexto.Execute(caractere, texto);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComCharDeEspaco_ConcatenaCorretamente()
        {
            // Arrange
            var caractere = ' ';
            var texto = "abc";
            var expected = " abc";

            // Act
            var result = ConcatenarCaractereTexto.Execute(caractere, texto);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
