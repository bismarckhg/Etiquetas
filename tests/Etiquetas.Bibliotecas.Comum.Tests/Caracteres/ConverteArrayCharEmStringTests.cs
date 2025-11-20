using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConverteArrayCharEmStringTests
    {
        [Fact]
        public void Execute_ComArrayDeChars_RetornaString()
        {
            // Arrange
            var array = new[] { 'a', 'b', 'c' };
            var expected = "abc";

            // Act
            var result = ConverteArrayCharEmString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_RetornaStringVazia()
        {
            // Arrange
            char[] array = null;
            var expected = "";

            // Act
            var result = ConverteArrayCharEmString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayVazio_RetornaStringVazia()
        {
            // Arrange
            var array = new char[] { };
            var expected = "";

            // Act
            var result = ConverteArrayCharEmString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComArrayDeEspacos_RetornaStringVazia()
        {
            // Arrange
            var array = new[] { ' ', '\t' };

            // A implementação atual com EhArrayCharNuloVazioComEspacosBranco deve retornar vazio
            var expected = "";

            // Act
            var result = ConverteArrayCharEmString.Execute(array);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
