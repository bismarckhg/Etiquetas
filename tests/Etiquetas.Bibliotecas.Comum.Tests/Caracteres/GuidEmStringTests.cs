using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class GuidEmStringTests
    {
        [Fact]
        public void Execute_ComGuidValido_RetornaStringCorreta()
        {
            // Arrange
            var guid = new Guid("12345678-1234-1234-1234-123456789abc");
            var expected = "12345678-1234-1234-1234-123456789abc";

            // Act
            var result = GuidEmString.Execute(guid);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComGuidVazio_RetornaStringDeZeros()
        {
            // Arrange
            var guid = Guid.Empty;
            var expected = "00000000-0000-0000-0000-000000000000";

            // Act
            var result = GuidEmString.Execute(guid);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
