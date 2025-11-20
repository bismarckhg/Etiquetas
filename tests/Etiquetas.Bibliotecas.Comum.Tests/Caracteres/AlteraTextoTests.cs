using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class AlteraTextoTests
    {
        [Fact]
        public void Execute_QuandoParteTextoExiste_SubstituiCorretamente()
        {
            // Arrange
            var texto = "isto é um teste";
            var parteTexto = "um teste";
            var novoConteudo = "um exemplo";
            var expected = "isto é um exemplo";

            // Act
            var result = AlteraTexto.Execute(texto, parteTexto, novoConteudo);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoParteTextoNaoExiste_NaoAlteraTexto()
        {
            // Arrange
            var texto = "isto é um teste";
            var parteTexto = "nao existe";
            var novoConteudo = "um exemplo";
            var expected = "isto é um teste";

            // Act
            var result = AlteraTexto.Execute(texto, parteTexto, novoConteudo);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoEhNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;
            var parteTexto = "teste";
            var novoConteudo = "exemplo";

            // Act
            var result = AlteraTexto.Execute(texto, parteTexto, novoConteudo);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_QuandoParteTextoEhNulo_RetornaTextoOriginal()
        {
            // Arrange
            var texto = "isto é um teste";
            string parteTexto = null;
            var novoConteudo = "exemplo";
            var expected = "isto é um teste";

            // Act
            var result = AlteraTexto.Execute(texto, parteTexto, novoConteudo);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoNovoConteudoEhNulo_SubstituiPorVazio()
        {
            // Arrange
            var texto = "isto é um teste";
            var parteTexto = " um teste";
            string novoConteudo = null;
            var expected = "isto é";

            // Act
            var result = AlteraTexto.Execute(texto, parteTexto, novoConteudo);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_QuandoTextoEstaVazio_RetornaTextoVazio()
        {
            // Arrange
            var texto = "";
            var parteTexto = "teste";
            var novoConteudo = "exemplo";
            var expected = "";

            // Act
            var result = AlteraTexto.Execute(texto, parteTexto, novoConteudo);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
