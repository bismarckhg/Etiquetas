using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class EhArrayStringNuloVazioComEspacosBrancoTests
    {
        [Fact]
        public void Execute_ComArrayNulo_DeveRetornarTrue()
        {
            // Arrange
            string[] array = null;

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayVazio_DeveRetornarTrue()
        {
            // Arrange
            string[] array = new string[0];

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayComApenasNulos_DeveRetornarTrue()
        {
            // Arrange
            string[] array = new string[] { null, null };

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayComApenasEspacos_DeveRetornarTrue()
        {
            // Arrange
            string[] array = new string[] { " ", "   " };

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayMistoDeNulosEspacosEVazios_DeveRetornarTrue()
        {
            // Arrange
            string[] array = new string[] { null, "", " ", "   " };

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_ComArrayComPeloMenosUmElementoValido_DeveRetornarFalse()
        {
            // Arrange
            string[] array = new string[] { null, "", " a ", "   " };

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_ComArrayValido_DeveRetornarFalse()
        {
            // Arrange
            string[] array = new string[] { "a", "b" };

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBranco.Execute(array);

            // Assert
            Assert.False(result);
        }
    }
}
