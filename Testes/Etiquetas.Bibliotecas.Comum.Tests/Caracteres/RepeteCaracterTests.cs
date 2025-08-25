using System;
using Xunit;
using Etiquetas.Bibliotecas.Comum.Caracteres;

namespace Etiquetas.Bibliotecas.Comum.Tests.Caracteres
{
    public class RepeteCaracterTests
    {
        [Fact]
        public void Execute_ComRepeticaoMultipla_DeveRetornarStringRepetida()
        {
            // Arrange
            var caractere = 'a';
            var repeticoes = 5;
            var expected = "aaaaa";

            // Act
            var result = RepeteCaracter.Execute(caractere, repeticoes);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComRepeticaoZero_DeveRetornarStringVazia()
        {
            // Arrange
            var caractere = 'a';
            var repeticoes = 0;

            // Act
            var result = RepeteCaracter.Execute(caractere, repeticoes);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Execute_ComRepeticaoUnica_DeveRetornarStringComUmCaractere()
        {
            // Arrange
            var caractere = 'b';
            var repeticoes = 1;
            var expected = "b";

            // Act
            var result = RepeteCaracter.Execute(caractere, repeticoes);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_ComRepeticaoNegativa_DeveLancarExcecao()
        {
            // Arrange
            var caractere = 'c';
            var repeticoes = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => RepeteCaracter.Execute(caractere, repeticoes));
        }
    }
}
