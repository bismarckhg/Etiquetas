using Xunit;
using Etiquetas.Bibliotecas.LibChar;

namespace Etiquetas.Bibliotecas.LibChar.Tests
{
    public class CharSomenteLetraTests
    {
        [Theory]
        [InlineData('a', true)]
        [InlineData('Z', true)]
        [InlineData('ç', true)]
        [InlineData('5', false)]
        [InlineData(' ', false)]
        [InlineData('$', false)]
        public void Execute_ShouldCorrectlyIdentifyLetters(char input, bool expected)
        {
            // Act
            bool result = CharSomenteLetra.Execute(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
