using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using System.Text;
using System;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class ArrayBytesEmStringTests
    {
        // Testes para Execute(byte[], int, Encoding)
        [Fact]
        public void ExecuteComBytesLidos_ComArrayValido_RetornaString()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("teste");
            var expected = "tes";

            // Act
            var result = ArrayBytesEmString.Execute(bytes, 3, Encoding.UTF8);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExecuteComBytesLidos_ComArrayNulo_RetornaNulo()
        {
            // Arrange
            byte[] bytes = null;

            // Act
            var result = ArrayBytesEmString.Execute(bytes, 0, Encoding.UTF8);

            // Assert
            Assert.Null(result);
        }

        // Testes para Execute(byte[], Encoding)
        [Fact]
        public void ExecuteCompleto_ComArrayValido_RetornaStringCompleta()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("teste");
            var expected = "teste";

            // Act
            var result = ArrayBytesEmString.Execute(bytes, Encoding.UTF8);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExecuteCompleto_ComEncodingDiferente_RetornaStringDiferente()
        {
            // Arrange
            var bytes = Encoding.ASCII.GetBytes("tésté");
            var expected = "teste"; // ASCII não tem acentos

            // Act
            var result = ArrayBytesEmString.Execute(bytes, Encoding.UTF8);

            // Assert
            Assert.NotEqual(expected, result);
        }

        // Testes para Execute(byte[], int, int, Encoding)
        [Fact]
        public void ExecuteComRange_ComRangeValido_RetornaSubstring()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("teste");
            var expected = "est";

            // Act
            var result = ArrayBytesEmString.Execute(bytes, 1, 3, Encoding.UTF8);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExecuteComRange_ComRangeInvalido_LancaExcecao()
        {
            // Arrange
            var bytes = Encoding.UTF8.GetBytes("teste");

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => ArrayBytesEmString.Execute(bytes, 1, 5, Encoding.UTF8));
        }
    }
}
