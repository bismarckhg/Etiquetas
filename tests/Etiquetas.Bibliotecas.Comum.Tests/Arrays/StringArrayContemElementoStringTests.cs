using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class StringArrayContemElementoStringTests
    {
        [Fact]
        public void Execute_QuandoArrayContemString_RetornaTrue()
        {
            // Arrange
            var array = new string[] { "a", "b", "c" };
            var contem = "b";

            // Act
            var result = StringArrayContemElementoString.Execute(array, contem);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_QuandoArrayNaoContemString_RetornaFalse()
        {
            // Arrange
            var array = new string[] { "a", "b", "c" };
            var contem = "d";

            // Act
            var result = StringArrayContemElementoString.Execute(array, contem);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoArrayEhNulo_RetornaFalse()
        {
            // Arrange
            string[] array = null;
            var contem = "a";

            // Act
            var result = StringArrayContemElementoString.Execute(array, contem);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoArrayEstaVazio_RetornaFalse()
        {
            // Arrange
            var array = new string[] { };
            var contem = "a";

            // Act
            var result = StringArrayContemElementoString.Execute(array, contem);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoStringBuscadaEhNula_RetornaFalse()
        {
            // Arrange
            var array = new string[] { "a", "b", "c" };
            string contem = null;

            // Act
            var result = StringArrayContemElementoString.Execute(array, contem);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoStringBuscadaEstaVazia_RetornaFalse()
        {
            // Arrange
            var array = new string[] { "a", "b", "c" };
            var contem = "";

            // Act
            var result = StringArrayContemElementoString.Execute(array, contem);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoBuscaDiferenciaMaiusculas_RetornaFalse()
        {
            // Arrange
            var array = new string[] { "a", "b", "c" };
            var contem = "B";

            // Act
            var result = StringArrayContemElementoString.Execute(array, contem);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_QuandoArrayContemStringExata_RetornaTrue()
        {
            // Arrange
            var array = new string[] { "a", "B", "c" };
            var contem = "B";

            // Act
            var result = StringArrayContemElementoString.Execute(array, contem);

            // Assert
            Assert.True(result);
        }
    }
}
