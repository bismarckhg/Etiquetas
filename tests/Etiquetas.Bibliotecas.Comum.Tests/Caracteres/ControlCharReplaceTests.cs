using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Collections.Generic;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ControlCharReplaceTests
    {
        [Fact]
        public void Execute_ComRepresentacoesDeControle_SubstituiCorretamente()
        {
            // Arrange
            var chrList = new Dictionary<string, char> { { "[SOH]", (char)1 }, { "[STX]", (char)2 } };
            var data = "a[SOH]b[STX]c";
            var expected = $"a{(char)1}b{(char)2}c";

            // Act
            var result = ControlCharReplace.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_SemRepresentacoesDeControle_NaoAlteraString()
        {
            // Arrange
            var chrList = new Dictionary<string, char> { { "[SOH]", (char)1 } };
            var data = "abc";
            var expected = "abc";

            // Act
            var result = ControlCharReplace.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComRepresentacaoNaoReconhecida_MantemOriginal()
        {
            // Arrange
            var chrList = new Dictionary<string, char> { { "[SOH]", (char)1 } };
            var data = "a[FOO]b";
            var expected = "a[FOO]b";

            // Act
            var result = ControlCharReplace.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComDadosNulos_RetornaNulo()
        {
            // Arrange
            var chrList = new Dictionary<string, char> { { "[SOH]", (char)1 } };
            string data = null;

            // Act
            var result = ControlCharReplace.Execute(data, chrList);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ComListaNula_RetornaDadosOriginais()
        {
            // Arrange
            Dictionary<string, char> chrList = null;
            var data = "a[SOH]b";
            var expected = "a[SOH]b";

            // Act
            var result = ControlCharReplace.Execute(data, chrList);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
