using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Linq;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ControlCharListTests
    {
        [Fact]
        public void Execute_RetornaDicionarioNaoVazio()
        {
            // Arrange & Act
            var result = ControlCharList.Execute();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Execute_ContemMapeamentoConhecido()
        {
            // Arrange
            var expectedChar = '\u0001';
            var expectedKey = "[SOH]";

            // Act
            var result = ControlCharList.Execute();

            // Assert
            Assert.True(result.ContainsKey(expectedKey));
            Assert.Equal(expectedChar, result[expectedKey]);
        }

        [Fact]
        public void ObtemArrayControlChar_RetornaArrayNaoVazio()
        {
            // Arrange & Act
            var result = ControlCharList.ObtemArrayControlChar();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ObtemArrayControlChar_TamanhoCorrespondeAoDicionario()
        {
            // Arrange
            var dictionary = ControlCharList.Execute();
            var array = ControlCharList.ObtemArrayControlChar();

            // Act & Assert
            Assert.Equal(dictionary.Count, array.Length);
        }

        [Fact]
        public void ObtemArrayControlChar_ContemCaractereDeControleConhecido()
        {
            // Arrange
            var expectedChar = '\u001B'; // ESC

            // Act
            var result = ControlCharList.ObtemArrayControlChar();

            // Assert
            Assert.Contains(expectedChar, result);
        }
    }
}
