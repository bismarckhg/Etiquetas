using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ExcluiStringTests
    {
        [Fact]
        public void Execute_QuandoSubstringExiste_ExcluiTodasOcorrencias()
        {
            // Arrange
            var texto = "abracadabra";
            var textoASerExcluido = "abra";
            var expected = "cad";

            // Act
            var result = ExcluiString.Execute(texto, textoASerExcluido);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoSubstringNaoExiste_NaoAlteraTexto()
        {
            // Arrange
            var texto = "abracadabra";
            var textoASerExcluido = "xyz";
            var expected = "abracadabra";

            // Act
            var result = ExcluiString.Execute(texto, textoASerExcluido);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoEhNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;
            var textoASerExcluido = "a";

            // Act
            var result = ExcluiString.Execute(texto, textoASerExcluido);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_QuandoTextoASerExcluidoEhNulo_RetornaTextoOriginal()
        {
            // Arrange
            var texto = "abracadabra";
            string textoASerExcluido = null;
            var expected = "abracadabra";

            // Act
            var result = ExcluiString.Execute(texto, textoASerExcluido);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComOcorrenciasSobrepostas_ExcluiCorretamente()
        {
            // Arrange
            var texto = "abababa";
            var textoASerExcluido = "aba";
            var expected = "b";

            // Act
            var result = ExcluiString.Execute(texto, textoASerExcluido);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
