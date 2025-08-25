using System;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Arrays;
using System.Text;

namespace Etiquetas.Bibliotecas.Comum.Tests.Arrays
{
    public class ConverteCharOUEArrayCharEmArrayByteTests
    {
        [Fact]
        public void Execute_ComParamsObject_ComCharECharArrayEString_DeveConverterParaByteArray()
        {
            // Arrange
            char char1 = 'a';
            char[] charArray = { 'b', 'c' };
            string str = "d";
            byte[] expected = Encoding.UTF8.GetBytes("abcd");

            // Act
            byte[] result = ConverteCharOUEArrayCharEmArrayByte.Execute(char1, charArray, str);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComParamsObject_ComTipoInvalido_DeveLancarExcecao()
        {
            // Arrange
            char char1 = 'a';
            int numeroInvalido = 123;

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => ConverteCharOUEArrayCharEmArrayByte.Execute(char1, numeroInvalido));
            Assert.Equal("Tipo Int32 invalido em ConverteArrayCharEmArrayByte.", ex.Message);
        }

        [Fact]
        public void Execute_ComParamsObject_ComInputVazio_DeveRetornarByteArrayVazio()
        {
            // Arrange
            byte[] expected = new byte[0];

            // Act
            byte[] result = ConverteCharOUEArrayCharEmArrayByte.Execute(new object[0]);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComParamsCharArray_DeveConverterParaByteArray()
        {
            // Arrange
            char[] charArray1 = { 'a', 'b' };
            char[] charArray2 = { 'c', 'd' };
            byte[] expected = Encoding.UTF8.GetBytes("abcd");

            // Act
            byte[] result = ConverteCharOUEArrayCharEmArrayByte.Execute(charArray1, charArray2);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComParamsCharArray_ComInputVazio_DeveRetornarByteArrayVazio()
        {
            // Arrange
            byte[] expected = new byte[0];
            char[][] emptyArray = new char[0][];

            // Act
            byte[] result = ConverteCharOUEArrayCharEmArrayByte.Execute(emptyArray);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComParamsChar_DeveConverterParaByteArray()
        {
            // Arrange
            byte[] expected = Encoding.UTF8.GetBytes("abc");

            // Act
            byte[] result = ConverteCharOUEArrayCharEmArrayByte.Execute('a', 'b', 'c');

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComString_DeveConverterParaByteArray()
        {
            // Arrange
            string str = "abcd";
            byte[] expected = Encoding.UTF8.GetBytes(str);

            // Act
            byte[] result = ConverteCharOUEArrayCharEmArrayByte.Execute(str);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComString_ComStringVazia_DeveRetornarByteArrayVazio()
        {
            // Arrange
            string str = "";
            byte[] expected = new byte[0];

            // Act
            byte[] result = ConverteCharOUEArrayCharEmArrayByte.Execute(str);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
