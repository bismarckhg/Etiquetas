using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Collections.Generic;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ControlCharConvertTests
    {
        [Fact]
        public void Execute_ComCaracteresDeControle_ConverteCorretamente()
        {
            // Arrange
            var chrList = new Dictionary<string, char> { { "<SOH>", (char)1 }, { "<STX>", (char)2 } };
            var data = $"a{(char)1}b{(char)2}c";
            var expected = "a<SOH>b<STX>c";

            // Act
            var result = ControlCharConvert.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_SemCaracteresDeControle_NaoAlteraString()
        {
            // Arrange
            var chrList = new Dictionary<string, char> { { "<SOH>", (char)1 } };
            var data = "abc";
            var expected = "abc";

            // Act
            var result = ControlCharConvert.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComDadosNulos_RetornaNulo()
        {
            // Arrange
            var chrList = new Dictionary<string, char> { { "<SOH>", (char)1 } };
            string data = null;

            // Act
            var result = ControlCharConvert.Execute(data, chrList);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComListaNula_RetornaDadosOriginais()
        {
            // Arrange
            Dictionary<string, char> chrList = null;
            var data = "abc";
            var expected = "abc";

            // Act
            var result = ControlCharConvert.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComDadosVazios_RetornaStringVazia()
        {
            // Arrange
            var chrList = new Dictionary<string, char> { { "<SOH>", (char)1 } };
            var data = "";
            var expected = "";

            // Act
            var result = ControlCharConvert.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComListaVazia_NaoAlteraString()
        {
            // Arrange
            var chrList = new Dictionary<string, char>();
            var data = "a\u0001b";
            var expected = "a\u0001b";

            // Act
            var result = ControlCharConvert.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
