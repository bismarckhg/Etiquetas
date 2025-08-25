using System;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class EhArrayStringNuloVazioComEspacosBrancoDBNullTests
    {
        // Testes para EhArrayStringNuloVazioComEspacosBrancoDBNull
        [Fact]
        public void DBNull_Execute_ComArrayNulo_DeveRetornarTrue()
        {
            // Arrange
            string[] array = null;

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DBNull_Execute_ComArrayValido_DeveRetornarFalse()
        {
            // Arrange
            string[] array = { "a", "b" };

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(array);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DBNull_Execute_ComDBNullValue_DeveRetornarTrue()
        {
            // Arrange
            DBNull dbNull = DBNull.Value;

            // Act
            bool result = EhArrayStringNuloVazioComEspacosBrancoDBNull.Execute(dbNull);

            // Assert
            Assert.True(result);
        }


        // Testes para EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull (wrapper)
        [Fact]
        public void OuDBNull_Execute_ComArrayNulo_DeveRetornarTrue()
        {
            // Arrange
            string[] array = null;

            // Act
            bool result = EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void OuDBNull_Execute_ComArrayValido_DeveRetornarFalse()
        {
            // Arrange
            string[] array = { "a", "b" };

            // Act
            bool result = EhArrayStringNuloOuVazioOuComEspacosBrancoOuDBNull.Execute(array);

            // Assert
            Assert.False(result);
        }
    }
}
