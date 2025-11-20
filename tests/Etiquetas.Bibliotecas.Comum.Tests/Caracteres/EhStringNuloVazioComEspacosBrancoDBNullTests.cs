using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class EhStringNuloVazioComEspacosBrancoDBNullTests
    {
        [Fact]
        public void Execute_String_ComStringNula_RetornaTrue()
        {
            // Arrange
            string texto = null;

            // Act
            var result = EhStringNuloVazioComEspacosBrancoDBNull.Execute(texto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_String_ComStringVazia_RetornaTrue()
        {
            // Arrange
            var texto = "";

            // Act
            var result = EhStringNuloVazioComEspacosBrancoDBNull.Execute(texto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_String_ComStringDeEspacos_RetornaTrue()
        {
            // Arrange
            var texto = "   \t";

            // Act
            var result = EhStringNuloVazioComEspacosBrancoDBNull.Execute(texto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Execute_String_ComStringNormal_RetornaFalse()
        {
            // Arrange
            var texto = "abc";

            // Act
            var result = EhStringNuloVazioComEspacosBrancoDBNull.Execute(texto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Execute_DBNull_ComDBNullValue_RetornaTrue()
        {
            // Arrange
            var value = DBNull.Value;

            // Act
            var result = EhStringNuloVazioComEspacosBrancoDBNull.Execute(value);

            // Assert
            Assert.True(result);
        }
    }
}
