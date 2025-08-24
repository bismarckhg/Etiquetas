using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ArrayCharPossuiAlgumCaractereEmStringTests
    {
        [Fact]
        public void Execute_QuandoTextoContemCaractereDoArray_RetornaTrue()
        {
            // Arrange
            var arrayChar = new[] { 'a', 'b', 'c' };
            var texto = "testing a string";

            // Act
            var result = ArrayCharPossuiAlgumCaractereEmString.Execute(arrayChar, texto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_QuandoTextoNaoContemCaractereDoArray_RetornaFalse()
        {
            // Arrange
            var arrayChar = new[] { 'x', 'y', 'z' };
            var texto = "testing a string";

            // Act
            var result = ArrayCharPossuiAlgumCaractereEmString.Execute(arrayChar, texto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoArrayCharEhNulo_RetornaFalse()
        {
            // Arrange
            char[] arrayChar = null;
            var texto = "testing a string";

            // Act
            var result = ArrayCharPossuiAlgumCaractereEmString.Execute(arrayChar, texto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoTextoEhNulo_RetornaFalse()
        {
            // Arrange
            var arrayChar = new[] { 'a', 'b', 'c' };
            string texto = null;

            // Act
            var result = ArrayCharPossuiAlgumCaractereEmString.Execute(arrayChar, texto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoAmbosSaoNulos_RetornaFalse()
        {
            // Arrange
            char[] arrayChar = null;
            string texto = null;

            // Act
            var result = ArrayCharPossuiAlgumCaractereEmString.Execute(arrayChar, texto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoTextoEstaVazio_RetornaFalse()
        {
            // Arrange
            var arrayChar = new[] { 'a', 'b', 'c' };
            var texto = "";

            // Act
            var result = ArrayCharPossuiAlgumCaractereEmString.Execute(arrayChar, texto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoArrayCharEstaVazio_RetornaFalse()
        {
            // Arrange
            var arrayChar = new char[] { };
            var texto = "testing a string";

            // Act
            var result = ArrayCharPossuiAlgumCaractereEmString.Execute(arrayChar, texto);

            // Assert
            Assert.False(result);
        }
    }
}
