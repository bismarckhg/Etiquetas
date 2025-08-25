using Xunit;
using Etiquetas.Bibliotecas.Comum.Numericos;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Numericos
{
    public class OitoBytesEmInt64Tests
    {
        [Fact]
        public void Execute_ComArrayDeBytes_RetornaInt64Correto()
        {
            // Arrange
            long valorOriginal = 1234567890123456789;
            var bytes = BitConverter.GetBytes(valorOriginal);

            // Act
            var result = OitoBytesEmInt64.Execute(bytes);

            // Assert
            Assert.Equal(valorOriginal, result);
        }

        [Fact]
        public void Execute_ComBytesIndividuais_RetornaInt64Correto()
        {
            // Arrange
            long valorOriginal = 1;
            var bytes = BitConverter.GetBytes(valorOriginal);

            // Act
            var result = OitoBytesEmInt64.Execute(bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2], bytes[1], bytes[0]);

            // Assert
            Assert.Equal(valorOriginal, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_LancaArgumentNullException()
        {
            // Arrange
            byte[] bytes = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => OitoBytesEmInt64.Execute(bytes));
        }

        [Fact]
        public void Execute_ComArrayCurto_LancaArgumentException()
        {
            // Arrange
            var bytes = new byte[] { 1, 2, 3, 4 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => OitoBytesEmInt64.Execute(bytes));
        }
    }
}
