using Xunit;
using Etiquetas.Bibliotecas.Comum.Numericos;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Numericos
{
    public class ConverteInt64EmOitoBytesTests
    {
        [Fact]
        public void Execute_ComValorPositivo_RetornaArrayDeBytesCorreto()
        {
            // Arrange
            long valor = 1234567890;
            var expected = BitConverter.GetBytes(valor);

            // Act
            var result = ConverteInt64EmOitoBytes.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComValorNegativo_RetornaArrayDeBytesCorreto()
        {
            // Arrange
            long valor = -1234567890;
            var expected = BitConverter.GetBytes(valor);

            // Act
            var result = ConverteInt64EmOitoBytes.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComZero_RetornaArrayDeBytesCorreto()
        {
            // Arrange
            long valor = 0;
            var expected = BitConverter.GetBytes(valor);

            // Act
            var result = ConverteInt64EmOitoBytes.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComMaxValor_RetornaArrayDeBytesCorreto()
        {
            // Arrange
            long valor = long.MaxValue;
            var expected = BitConverter.GetBytes(valor);

            // Act
            var result = ConverteInt64EmOitoBytes.Execute(valor);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
