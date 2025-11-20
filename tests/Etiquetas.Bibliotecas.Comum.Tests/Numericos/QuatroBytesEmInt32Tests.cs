using Xunit;
using Etiquetas.Bibliotecas.Comum.Numericos;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Numericos
{
    public class QuatroBytesEmInt32Tests
    {
        [Fact]
        public void Execute_ComArrayDeBytes_RetornaInt32Correto()
        {
            // Arrange
            int valorOriginal = 123456789;
            var bytes = BitConverter.GetBytes(valorOriginal);

            // Act
            var result = QuatroBytesEmInt32.Execute(bytes);

            // Assert
            Assert.Equal(valorOriginal, result);
        }

        [Fact]
        public void Execute_ComBytesIndividuais_RetornaInt32Correto()
        {
            // Arrange
            int valorOriginal = 1;
            var bytes = BitConverter.GetBytes(valorOriginal);

            // Act
            var result = QuatroBytesEmInt32.Execute(bytes[3], bytes[2], bytes[1], bytes[0]);

            // Assert
            Assert.Equal(valorOriginal, result);
        }

        [Fact]
        public void Execute_ComArrayNulo_LancaArgumentNullException()
        {
            // Arrange
            byte[] bytes = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => QuatroBytesEmInt32.Execute(bytes));
        }

        [Fact]
        public void Execute_ComArrayCurto_LancaArgumentException()
        {
            // Arrange
            var bytes = new byte[] { 1, 2 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => QuatroBytesEmInt32.Execute(bytes));
        }
    }
}
