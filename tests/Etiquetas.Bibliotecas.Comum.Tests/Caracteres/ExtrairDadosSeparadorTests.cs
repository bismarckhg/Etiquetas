using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ExtrairDadosSeparadorTests
    {
        [Fact]
        public void Execute_ComOcorrenciaValida_RetornaParteCorreta()
        {
            // Arrange
            var texto = "a-b-c";
            var ocorrencia = 2;
            var separadores = new[] { "-" };
            var expected = "b";

            // Act
            var result = ExtrairDadosSeparador.Execute(texto, ocorrencia, separadores);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComOcorrenciaForaDosLimites_RetornaNulo()
        {
            // Arrange
            var texto = "a-b-c";
            var ocorrencia = 4;
            var separadores = new[] { "-" };

            // Act
            var result = ExtrairDadosSeparador.Execute(texto, ocorrencia, separadores);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComOcorrenciaMenorQueUm_RetornaNulo()
        {
            // Arrange
            var texto = "a-b-c";
            var ocorrencia = 0;
            var separadores = new[] { "-" };

            // Act
            var result = ExtrairDadosSeparador.Execute(texto, ocorrencia, separadores);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComTextoNulo_RetornaNulo()
        {
            // Arrange
            string texto = null;
            var ocorrencia = 1;
            var separadores = new[] { "-" };

            // Act
            var result = ExtrairDadosSeparador.Execute(texto, ocorrencia, separadores);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComSeparadoresNulos_RetornaTextoOriginal()
        {
            // Arrange
            var texto = "a-b-c";
            var ocorrencia = 1;
            string[] separadores = null;
            var expected = "a-b-c";

            // Act
            var result = ExtrairDadosSeparador.Execute(texto, ocorrencia, separadores);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComMultiplosSeparadores_DivideCorretamente()
        {
            // Arrange
            var texto = "a-b_c";
            var ocorrencia = 2;
            var separadores = new[] { "-", "_" };
            var expected = "b";

            // Act
            var result = ExtrairDadosSeparador.Execute(texto, ocorrencia, separadores);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
