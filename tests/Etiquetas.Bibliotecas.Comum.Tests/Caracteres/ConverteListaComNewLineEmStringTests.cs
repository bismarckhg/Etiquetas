using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Collections.Generic;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ConverteListaComNewLineEmStringTests
    {
        [Fact]
        public void Execute_ComListaNormal_ConcatenaCorretamente()
        {
            // Arrange
            var lista = new List<string> { "linha1", "linha2", "linha3" };
            var expected = "linha1linha2linha3";

            // Act
            var result = ConverteListaComNewLineEmString.Execute(lista);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComListaNula_RetornaStringVazia()
        {
            // Arrange
            List<string> lista = null;
            var expected = "";

            // Act
            var result = ConverteListaComNewLineEmString.Execute(lista);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComListaVazia_RetornaStringVazia()
        {
            // Arrange
            var lista = new List<string>();
            var expected = "";

            // Act
            var result = ConverteListaComNewLineEmString.Execute(lista);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComElementosNulosEVazios_FiltraCorretamente()
        {
            // Arrange
            var lista = new List<string> { "linha1", null, " ", "linha2", "" };
            var expected = "linha1linha2";

            // Act
            var result = ConverteListaComNewLineEmString.Execute(lista);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComElementoNewLine_FiltraCorretamente()
        {
            // Arrange
            var lista = new List<string> { "linha1", Environment.NewLine, "linha2" };
            var expected = "linha1linha2";

            // Act
            var result = ConverteListaComNewLineEmString.Execute(lista);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
