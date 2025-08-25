using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class StringEmArrayStringComSeparadorEmCadaItemTests
    {
        [Fact]
        public void Execute_ComSeparadorNormal_DeveSepararCorretamente()
        {
            // Arrange
            var texto = "Sitem1Sitem2Sitem3";
            var separador = 'S';
            var expected = new[] { "Sitem1", "Sitem2", "Sitem3" };

            // Act
            var result = StringEmArrayStringComSeparadorEmCadaItem.Execute(texto, separador);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTextoIniciandoComSeparador_DeveFuncionar()
        {
            // Arrange
            var texto = "Sitem1Sitem2";
            var separador = 'S';
            var expected = new[] { "Sitem1", "Sitem2" };

            // Act
            var result = StringEmArrayStringComSeparadorEmCadaItem.Execute(texto, separador);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComSeparadoresConsecutivos_DeveFuncionar()
        {
            // Arrange
            var texto = "Sitem1SSitem3";
            var separador = 'S';
            var expected = new[] { "Sitem1", "S", "Sitem3" };

            // Act
            var result = StringEmArrayStringComSeparadorEmCadaItem.Execute(texto, separador);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComTextoVazio_DeveRetornarArrayVazio()
        {
            // Arrange
            var texto = "";
            var separador = 'S';

            // Act
            var result = StringEmArrayStringComSeparadorEmCadaItem.Execute(texto, separador);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Execute_SemSeparadorNoTexto_DeveRetornarArrayVazio()
        {
            // Arrange
            var texto = "item1item2item3";
            var separador = 'S';

            // Act
            var result = StringEmArrayStringComSeparadorEmCadaItem.Execute(texto, separador);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Execute_ComSeparadorSendoCaractereEspecialRegex_DeveFuncionar()
        {
            // Arrange
            var texto = ".item1.item2";
            var separador = '.';
            var expected = new[] { ".item1", ".item2" };

            // Act
            var result = StringEmArrayStringComSeparadorEmCadaItem.Execute(texto, separador);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
